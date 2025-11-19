using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Domain
{
    public class InventorySKUDomainModel : IInventorySKU
    {
        private readonly ILogger logger;
        private readonly IDateTimeService dateTimeService;
        private readonly IRepository repository;
        private readonly IInventorySKUFactory inventorySKUFactory;
        private readonly IInventorySKUTypeFactory inventorySKUTypeFactory;
        private readonly IInventoryMovementManager inventoryMovementManager;
        private readonly IInventoryMovementFactory inventoryMovementFactory;
        private readonly IWorkshopManager workshopManager;

        private readonly MovementCollection movements;

        private InventorySKU entity;
        private IInventorySKUType type;

        public InventorySKUDomainModel(
            InventorySKU entity,
            IInventorySKUType type,
            ILogger<InventorySKUDomainModel> logger,
            IDateTimeService dateTimeService,
            IRepository repository,
            IInventorySKUFactory inventorySKUFactory,
            IInventorySKUTypeFactory inventorySKUTypeFactory,
            IInventoryMovementManager inventoryMovementManager,
            IInventoryMovementFactory inventoryMovementFactory,
            IWorkshopManager workshopManager)
        {
            this.logger = logger;
            this.dateTimeService = dateTimeService;
            this.repository = repository;
            this.inventorySKUFactory = inventorySKUFactory;
            this.inventorySKUTypeFactory = inventorySKUTypeFactory;
            this.inventoryMovementManager = inventoryMovementManager;
            this.inventoryMovementFactory = inventoryMovementFactory;
            this.workshopManager = workshopManager;

            this.entity = entity;
            this.type = type;

            this.movements = new MovementCollection(this);
        }

        public Guid Id => this.entity.Id;

        public String Name => this.entity.Name;

        public IInventorySKUType Type => this.type;

        public Decimal? AveragePrice => this.entity.AveragePrice;

        public Decimal? WarningQuantity => this.entity.WarningQuantity;

        public String Description => this.entity.Description;

        public InventorySKUNodeType NodeType => this.entity.NodeType;

        public DeletionStatus DeletionStatus => this.entity.DeletionStatus;

        public DateTime? DeletionDate => this.entity.DeletionDate;

        public IInventorySKU.IMovementCollection Movements => this.movements;

        public async Task RefreshAsync()
        {
            var newEntity = await this.repository.QueryWithoutTracking<InventorySKU>()
                .Include(x => x.Type)
                .SingleOrDefaultAsync(x => x.Id == this.entity.Id);

            this.entity = newEntity ?? throw new InvalidOperationException("Entity is missing after refresh");

            if (this.entity.TypeId != this.type.Id)
            {
                this.type = this.inventorySKUTypeFactory.Create(this.entity.Type);
            }
        }

        [Obsolete("Obsolete", error: true)]
        public async Task<Decimal> GetAvailableQuantity()
        {
            return await this.repository.QueryWithoutTracking<InventoryMovement>()
                .Where(x => x.SKUId == this.entity.Id && (x.Status == InventoryMovementStatus.Completed || x.Status == InventoryMovementStatus.Allocated))
                .Select(x => x.Quantity)
                .SumAsync();
        }

        public async Task<Decimal> GetAvailableQuantity(IWorkshop workshop)
        {
            return await this.repository.QueryWithoutTracking<InventoryMovement>()
                .Where(x => x.WorkshopId == workshop.Id)
                .Where(x => x.SKUId == this.entity.Id && (x.Status == InventoryMovementStatus.Completed || x.Status == InventoryMovementStatus.Allocated))
                .Select(x => x.Quantity)
                .SumAsync();
        }

        [Obsolete("Obsolete", error: true)]
        public async Task<Decimal> GetMissingQuantity()
        {
            var entry = await this.repository.QueryWithoutTracking<InventorySKUMissingQuantity>()
                .SingleOrDefaultAsync(x => x.Id == this.Id);
            return entry?.MissingQuantity ?? 0m;
        }

        public async Task<Decimal> GetMissingQuantity(IWorkshop workshop)
        {
            var entry = await this.repository.QueryWithoutTracking<InventorySKUWorkshopMissingQuantity>()
                .Where(x => x.WorkshopId == workshop.Id)
                .SingleOrDefaultAsync(x => x.Id == this.Id);
            return entry?.MissingQuantity ?? 0m;
        }

        public async Task<IReadOnlyList<IInventorySKU>> GetDescendantsAndSelfAsync()
        {
            var descendants = await this.repository.QueryWithoutTracking<InventorySKUAscendant>()
                .Include(x => x.Descendant)
                .ThenInclude(x => x.Type)
                .Where(x => x.AscendantId == this.Id)
                .Where(x => x.DescendantId != this.Id)
                .Select(x => x.Descendant)
                .ToListAsync();

            var result = new List<IInventorySKU>();
            result.Add(this);
            foreach (var descendant in descendants)
            {
                result.Add(this.inventorySKUFactory.Create(descendant, this.inventorySKUTypeFactory.Create(descendant.Type)));
            }

            return result;
        }

        public async Task TryProcessMovementsChangesForAllAsync([CanBeNull] Func<IInventoryRepairMovement, Task<Int32>> allocationPriority = null)
        {
            var workshops = await this.workshopManager.ListAllAsync();
            foreach (var workshop in workshops)
            {
                await this.TryProcessMovementsChangesAsync(workshop, allocationPriority);
            }
        }

        public async Task TryProcessMovementsChangesAsync(IWorkshop workshop, [CanBeNull] Func<IInventoryRepairMovement, Task<Int32>> allocationPriority = null)
        {
            this.logger.LogInformation($"Started movements processing in workshop {workshop.Name} for SKU: {this.Id}");
            try
            {
                await this.TryAllocateRepairsAsync(workshop, allocationPriority);
                await this.TryUpdateOrdersAsync(workshop);
                this.logger.LogInformation($"Finished movements processing in workshop {workshop.Name} for SKU: {this.Id}");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Failed movements processing in workshop {workshop.Name} for SKU: {this.Id}");
                throw;
            }
        }

        public async Task TryAllocateRepairsAsync(IWorkshop workshop, [CanBeNull] Func<IInventoryRepairMovement, Task<Int32>> priorityPredicate = null)
        {
            var availableQuantity = await this.GetAvailableQuantity(workshop);
            if (availableQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                // Nothing to allocate
                return;
            }

            this.logger.LogInformation($"SKU({this.Id}).TryAllocateRepairsAsync: Available QTY = {availableQuantity}");

            var allWaitingRepairs = await this.Movements.GetMovementsByTypeAndStatusAsync<IInventoryRepairMovement>(workshop, InventoryMovementStatus.Waiting);
            var weightedRepairs = new List<(IInventoryRepairMovement Repair, IHandpieceRequiredPart Part, Int32 Weight, DateTime? ApprovedOn)>();
            foreach (var repair in allWaitingRepairs)
            {
                var part = await repair.GetHandpieceRequiredPartAsync();
                if (part == null)
                {
                    continue;
                }

                var weight = priorityPredicate == null ? 0 : await priorityPredicate(repair);
                weightedRepairs.Add((repair, part, weight, part.Handpiece.ApprovedOn));
            }

            var repairs = weightedRepairs
                .OrderByDescending(x => x.Weight)
                .ThenBy(x => x.ApprovedOn == null)
                .ThenBy(x => x.ApprovedOn)
                .ThenBy(x => x.Repair.CreatedOn)
                .ToList();
            foreach (var repair in repairs)
            {
                var missing = await repair.Part.GetMissingQuantity();
                if (missing.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                {
                    // TODO: Should not happen - fix allocation
                    continue;
                }

                var allocated = await repair.Part.IncreaseAllocationAsync(Math.Min(missing, availableQuantity));
                this.logger.LogInformation($"SKU({this.Id}).TryAllocateRepairsAsync: Allocated {allocated} to repair {repair.Repair.Id}: {repair.Part.Handpiece.Number}");
                await repair.Part.Handpiece.Parts.UpdateStockStatusAsync(trackChange: true);

                availableQuantity -= allocated;
                if (availableQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                {
                    return;
                }
            }
        }

        public async Task TryUpdateOrdersAsync(IWorkshop workshop)
        {
            var allMovements = await this.Movements.GetAllMovementsAsync(workshop);
            var repairMovements = allMovements.OfType<IInventoryRepairMovement>().ToList();
            var orderMovements = allMovements.OfType<IInventoryOrderMovement>().ToList();

            var waitingRepairs = repairMovements.ToList();
            foreach (var repair in waitingRepairs)
            {
                var part = await repair.GetHandpieceRequiredPartAsync();
                if (part == null)
                {
                    continue;
                }

                this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Processing part {part.Id}: {part.Handpiece.Number}");

                var handpiece = part.Handpiece;
                var missing = await part.GetMissingQuantity();
                var needsApprovalFirst = !((Int32)handpiece.Status >= (Int32)HandpieceStatus.BeingRepaired);

                var linkedOrders = await this.Movements.GetMovementsLinkedToPartAsync<IInventoryOrderMovement>(part.Id);
                linkedOrders = linkedOrders.Where(x => x.Status.IsOneOf(InventoryMovementStatus.Requested, InventoryMovementStatus.Approved, InventoryMovementStatus.Ordered)).ToList();
                var linkedOrdersQuantity = linkedOrders.Count > 0 ? linkedOrders.Sum(x => x.QuantityAbsolute) : 0m;

                if (handpiece.Status.IsOneOf(HandpieceStatus.SentComplete, HandpieceStatus.Cancelled) || repair.Status.IsOneOf(InventoryMovementStatus.Completed, InventoryMovementStatus.Cancelled))
                {
                    // Repair is complete or cancelled but some orders are still there - cancel them all (orders that already ordered are kept but unlinked from handpiece)
                    if (linkedOrdersQuantity.GreaterThan(0m, InventorySKU.QuantityPrecision))
                    {
                        foreach (var order in linkedOrders.Where(x => x.Status != InventoryMovementStatus.Ordered))
                        {
                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Cancelling order {order.Id}");
                            await order.CancelAsync();
                        }

                        foreach (var order in linkedOrders.Where(x => x.Status == InventoryMovementStatus.Ordered))
                        {
                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Unlinking order {order.Id} from part {part.Handpiece.Number}");
                            await order.UnlinkFromPartAsync();
                        }
                    }

                    continue;
                }

                if (linkedOrdersQuantity.Equals(missing, InventorySKU.QuantityPrecision))
                {
                    // All is good - no need for orders adjustments
                    continue;
                }

                if (linkedOrdersQuantity.LessThan(missing, InventorySKU.QuantityPrecision))
                {
                    // Need to create extra orders
                    var missingQuantity = missing - linkedOrdersQuantity;
                    this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Need to add {missingQuantity}");

                    var newOrderStatus = needsApprovalFirst ? InventoryMovementStatus.Requested : InventoryMovementStatus.Approved;
                    var newOrderExisting = linkedOrders.FirstOrDefault(x => x.Status == newOrderStatus);
                    if (newOrderExisting != null)
                    {
                        this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Adding {missingQuantity} QTY to order {newOrderExisting.Id}");
                        await newOrderExisting.UpdateRequestedQuantityAsync(newOrderExisting.QuantityAbsolute + missingQuantity);
                    }
                    else
                    {
                        var newOrder = await this.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                            workshop,
                            missingQuantity,
                            b => b.WithStatus(newOrderStatus));
                        await part.LinkMovementsAsync(new IInventoryMovement[] { newOrder });
                        this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Creating order for {missingQuantity}: {newOrder.Id}");
                    }
                }
                else if (linkedOrdersQuantity.GreaterThan(missing, InventorySKU.QuantityPrecision))
                {
                    // Need to remove qty from or even cancel existing orders (orders that already ordered are kept but unlinked from handpiece)
                    var extraQuantity = linkedOrdersQuantity - missing;
                    this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Need to remove {extraQuantity}");

                    foreach (var order in linkedOrders.Where(x => x.Status != InventoryMovementStatus.Ordered).OrderBy(x => x.Status))
                    {
                        if (extraQuantity.GreaterThanOrEqual(order.QuantityAbsolute, InventorySKU.QuantityPrecision))
                        {
                            extraQuantity -= order.QuantityAbsolute;
                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Cancelling order {order.Id}");
                            await order.CancelAsync();
                        }
                        else if (order.QuantityAbsolute.GreaterThanOrEqual(extraQuantity, InventorySKU.QuantityPrecision))
                        {
                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Updating order {order.Id} to request {extraQuantity} less");
                            await order.UpdateRequestedQuantityAsync(order.QuantityAbsolute - extraQuantity);
                            extraQuantity = 0m;
                        }

                        if (extraQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                        {
                            break;
                        }
                    }

                    if (extraQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                    {
                        continue;
                    }

                    foreach (var order in linkedOrders.Where(x => x.Status == InventoryMovementStatus.Ordered))
                    {
                        if (extraQuantity.GreaterThanOrEqual(order.QuantityAbsolute, InventorySKU.QuantityPrecision))
                        {
                            extraQuantity -= order.QuantityAbsolute;

                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Unlinking order {order.Id} from part {part.Handpiece.Number}");
                            await order.UnlinkFromPartAsync();
                        }
                        else if (order.QuantityAbsolute.GreaterThanOrEqual(extraQuantity, InventorySKU.QuantityPrecision))
                        {
                            var remainingQuantity = order.QuantityAbsolute - extraQuantity;
                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Updating order {order.Id} to verify {extraQuantity} less");
                            await order.UpdateOrderedQuantityAsync(remainingQuantity);

                            this.logger.LogInformation($"SKU({this.Id}).TryUpdateOrdersAsync: Creating new non-linked order {order.Id} for {extraQuantity}");
                            var newOrder = await this.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                                workshop,
                                extraQuantity,
                                b => b.WithStatus(InventoryMovementStatus.Ordered));
                            extraQuantity = 0m;
                        }

                        if (extraQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private class MovementCollection : IInventorySKU.IMovementCollection
        {
            private static readonly IDictionary<Type, InventoryMovementType> TypeToMovementTypeMap = new Dictionary<Type, InventoryMovementType>
            {
                [typeof(IInventoryInitialMovement)] = InventoryMovementType.Initial,
                [typeof(IInventoryFoundMovement)] = InventoryMovementType.Found,
                [typeof(IInventoryOrderMovement)] = InventoryMovementType.Order,
                [typeof(IInventoryLostMovement)] = InventoryMovementType.Lost,
                [typeof(IInventoryRepairMovement)] = InventoryMovementType.Repair,
                [typeof(IInventoryRepairFragmentMovement)] = InventoryMovementType.RepairFragment,
            };

            private readonly InventorySKUDomainModel sku;

            public MovementCollection(InventorySKUDomainModel sku)
            {
                this.sku = sku;
            }

            public async Task<IReadOnlyList<IInventoryMovement>> GetAllMovementsAsync(IWorkshop workshop)
            {
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.WorkshopId == workshop.Id)
                    .Where(x => x.SKUId == this.sku.Id)
                    .ToListAsync();

                var workshopResolver = new Dictionary<Guid, IWorkshop> { [workshop.Id] = workshop };
                return this.CreateMovementsList(movements, workshopResolver);
            }

            public async Task<IInventoryMovement> GetMovementByIdAsync(Guid id)
            {
                var movement = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (movement == null)
                {
                    return null;
                }

                var workshop = await this.sku.workshopManager.GetByIdAsync(movement.WorkshopId);
                return this.sku.inventoryMovementFactory.Create(movement, workshop);
            }

            public async Task<TMovement> GetMovementByIdAsync<TMovement>(Guid id)
                where TMovement : class, IInventoryMovement
            {
                var movement = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (movement == null)
                {
                    return null;
                }

                var workshop = await this.sku.workshopManager.GetByIdAsync(movement.WorkshopId);
                return this.sku.inventoryMovementFactory.Create<TMovement>(movement, workshop);
            }

            public async Task<IReadOnlyList<IInventoryMovement>> GetMovementsByIdsAsync(IEnumerable<Guid> ids)
            {
                var idsArray = ids.ToArray();
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => idsArray.Contains(x.Id))
                    .ToListAsync();

                var workshopResolver = await this.sku.workshopManager.LoadResolverAsync();
                return this.CreateMovementsList(movements, workshopResolver);
            }

            public async Task<IReadOnlyList<TMovement>> GetMovementsByIdsAsync<TMovement>(IEnumerable<Guid> ids)
                where TMovement : class, IInventoryMovement
            {
                var idsArray = ids.ToArray();
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => idsArray.Contains(x.Id))
                    .ToListAsync();

                var workshopResolver = await this.sku.workshopManager.LoadResolverAsync();
                return this.CreateMovementsList<TMovement>(movements, workshopResolver);
            }

            public async Task<IReadOnlyList<IInventoryMovement>> GetMovementsByTypeAndStatusAsync(IWorkshop workshop, InventoryMovementType type, InventoryMovementStatus status)
            {
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.WorkshopId == workshop.Id)
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.Type == type && x.Status == status)
                    .ToListAsync();

                return this.CreateMovementsList(movements, new Dictionary<Guid, IWorkshop> { [workshop.Id] = workshop });
            }

            public async Task<IReadOnlyList<TMovement>> GetMovementsByTypeAndStatusAsync<TMovement>(IWorkshop workshop, InventoryMovementStatus status) where TMovement : class, IInventoryMovement
            {
                var type = this.GetMovementType<TMovement>();
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.WorkshopId == workshop.Id)
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.Type == type && x.Status == status)
                    .ToListAsync();

                return this.CreateMovementsList<TMovement>(movements, new Dictionary<Guid, IWorkshop> { [workshop.Id] = workshop });
            }

            public async Task<IReadOnlyList<IInventoryMovement>> GetMovementsByTypeAndStatusAsync(InventoryMovementType type, InventoryMovementStatus status)
            {
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.Type == type && x.Status == status)
                    .ToListAsync();

                var workshopResolver = await this.sku.workshopManager.LoadResolverAsync();
                return this.CreateMovementsList(movements, workshopResolver);
            }

            public async Task<IReadOnlyList<TMovement>> GetMovementsByTypeAndStatusAsync<TMovement>(InventoryMovementStatus status)
                where TMovement : class, IInventoryMovement
            {
                var type = this.GetMovementType<TMovement>();
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.Type == type && x.Status == status)
                    .ToListAsync();

                var workshopResolver = await this.sku.workshopManager.LoadResolverAsync();
                return this.CreateMovementsList<TMovement>(movements, workshopResolver);
            }

            public async Task<IReadOnlyList<IInventoryMovement>> GetMovementsLinkedToPartAsync(Guid partId)
            {
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.RequiredParts.Any(y => y.RequiredPartId == partId))
                    .ToListAsync();

                var workshopResolver = await this.sku.workshopManager.LoadResolverAsync();
                return this.CreateMovementsList(movements, workshopResolver);
            }

            public async Task<IReadOnlyList<TMovement>> GetMovementsLinkedToPartAsync<TMovement>(Guid partId)
                where TMovement : class, IInventoryMovement
            {
                var movementType = this.GetMovementType<TMovement>();
                var movements = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.RequiredParts.Any(y => y.RequiredPartId == partId))
                    .Where(x => x.Type == movementType)
                    .ToListAsync();

                var workshopResolver = await this.sku.workshopManager.LoadResolverAsync();
                return this.CreateMovementsList<TMovement>(movements, workshopResolver);
            }

            public async Task<TMovement> GetSingleMovementLinkedToPartAsync<TMovement>(Guid partId)
                where TMovement : class, IInventoryMovement
            {
                var movementType = this.GetMovementType<TMovement>();
                var movement = await this.sku.repository.QueryWithoutTracking<InventoryMovement>()
                    .Where(x => x.SKUId == this.sku.Id)
                    .Where(x => x.RequiredParts.Any(y => y.RequiredPartId == partId))
                    .Where(x => x.Type == movementType)
                    .SingleOrDefaultAsync();

                if (movement == null)
                {
                    return null;
                }

                var workshop = await this.sku.workshopManager.GetByIdAsync(movement.WorkshopId);
                return this.sku.inventoryMovementFactory.Create<TMovement>(movement, workshop);
            }

            public async Task<TMovement> CreateAsync<TMovement, TBuilder>(IWorkshop workshop, Decimal quantity, Action<TBuilder> builder = null)
                where TBuilder : IInventoryMovementBuilder<TMovement>, new()
                where TMovement : class, IInventoryMovement
            {
                if (this.sku.NodeType != InventorySKUNodeType.Leaf)
                {
                    throw new InvalidOperationException($"Unable to create movement for the SKU with NodeType {this.sku.NodeType}");
                }

                if (this.sku.DeletionStatus != DeletionStatus.Normal)
                {
                    throw new InvalidOperationException($"Unable to create movement for deleted SKU");
                }

                if (quantity == 0)
                {
                    throw new InvalidOperationException("Quantity cannot be 0");
                }

                if (quantity < 0)
                {
                    throw new InvalidOperationException("Quantity cannot be negative");
                }

                var builderInstance = new TBuilder();
                builder?.Invoke(builderInstance);

                var movementInstance = builderInstance.Build();
                movementInstance.WorkshopId = workshop.Id;
                movementInstance.SKUId = this.sku.Id;
                movementInstance.CreatedOn = this.sku.dateTimeService.CurrentUtcTime;
                movementInstance.QuantityAbsolute = quantity;
                movementInstance.Quantity = movementInstance.Direction switch
                {
                    InventoryMovementDirection.Increase => quantity,
                    InventoryMovementDirection.Decrease => -quantity,
                    _ => throw new InvalidOperationException("Invalid movement direction"),
                };

                if (movementInstance.Status == InventoryMovementStatus.Completed)
                {
                    movementInstance.CompletedOn = movementInstance.CreatedOn;
                }

                await this.sku.repository.InsertAsync(movementInstance);
                await this.sku.repository.SaveChangesAsync();

                var movement = await this.sku.inventoryMovementManager.GetById<TMovement>(movementInstance.Id);
                await movement.TrackCreation();
                await this.sku.repository.SaveChangesAsync();

                return movement;
            }

            public async Task ReallocateRepairsAsync(IWorkshop workshop, IReadOnlyList<Guid> sourceMovements, IReadOnlyList<Guid> destinationMovements)
            {
                var availableQuantity = await this.sku.GetAvailableQuantity(workshop);
                var waitingRepairs = await this.GetMovementsByTypeAndStatusAsync<IInventoryRepairMovement>(workshop, InventoryMovementStatus.Waiting);
                var allocatedRepairs = await this.GetMovementsByTypeAndStatusAsync<IInventoryRepairMovement>(workshop, InventoryMovementStatus.Allocated);

                var source = sourceMovements.Select(x => allocatedRepairs.SingleOrDefault(y => y.Id == x)).ToArray();
                var destination = destinationMovements.Select(x => waitingRepairs.SingleOrDefault(y => y.Id == x)).ToArray();
                if (source.Any(x => x == null) || destination.Any(x => x == null))
                {
                    throw new DomainOperationException("Source or destination composition changed unexpectedly");
                }

                var availableAndReleasedQuantity = availableQuantity;
                foreach (var repair in source)
                {
                    var part = await repair.GetHandpieceRequiredPartAsync();
                    var allocated = await part.GetAllocatedQuantity();
                    var released = await part.DecreaseAllocationAsync(allocated);
                    await part.Handpiece.Parts.UpdateStockStatusAsync(trackChange: true);
                    availableAndReleasedQuantity += released;
                }

                foreach (var repair in destination)
                {
                    var part = await repair.GetHandpieceRequiredPartAsync();
                    var missing = await part.GetMissingQuantity();
                    var allocated = await part.IncreaseAllocationAsync(Math.Min(availableAndReleasedQuantity, missing));
                    await part.Handpiece.Parts.UpdateStockStatusAsync(trackChange: true);
                    availableAndReleasedQuantity -= allocated;
                    if (availableAndReleasedQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                    {
                        return;
                    }
                }
            }

            private IReadOnlyList<IInventoryMovement> CreateMovementsList(IReadOnlyList<InventoryMovement> movements, IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver)
            {
                var result = new List<IInventoryMovement>();
                foreach (var movement in movements)
                {
                    result.Add(this.sku.inventoryMovementFactory.Create(movement, workshopsResolver[movement.WorkshopId]));
                }

                return result;
            }

            private IReadOnlyList<TMovement> CreateMovementsList<TMovement>(IReadOnlyList<InventoryMovement> movements, IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver)
                where TMovement : class, IInventoryMovement
            {
                var result = new List<TMovement>();
                foreach (var movement in movements)
                {
                    result.Add(this.sku.inventoryMovementFactory.Create<TMovement>(movement, workshopsResolver[movement.WorkshopId]));
                }

                return result;
            }

            private InventoryMovementType GetMovementType<TMovement>()
                where TMovement : class, IInventoryMovement
            {
                if (!MovementCollection.TypeToMovementTypeMap.TryGetValue(typeof(TMovement), out var movementType))
                {
                    throw new InvalidOperationException($"Invalid movement type: {typeof(TMovement).FullName}");
                }

                return movementType;
            }
        }
    }
}
