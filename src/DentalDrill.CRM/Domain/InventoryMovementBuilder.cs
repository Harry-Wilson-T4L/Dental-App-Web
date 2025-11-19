using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using static DentalDrill.CRM.Domain.InventoryMovementBuilder;

namespace DentalDrill.CRM.Domain
{
    public static class InventoryMovementBuilder
    {
        public abstract class InventoryMovementBuilderBase<TMovement> : IInventoryMovementBuilder<TMovement>
            where TMovement : IInventoryMovement
        {
            private Boolean isBuilt;

            public InventoryMovement Build()
            {
                if (this.isBuilt)
                {
                    throw new InvalidOperationException("This builder was already used to build a movement");
                }

                this.isBuilt = true;
                return this.PerformBuild();
            }

            protected abstract InventoryMovement PerformBuild();
        }

        public class Initial : InventoryMovementBuilderBase<IInventoryInitialMovement>
        {
            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Increase,
                    Type = InventoryMovementType.Initial,
                    Status = InventoryMovementStatus.Completed,
                };
            }
        }

        public class Order : InventoryMovementBuilderBase<IInventoryOrderMovement>
        {
            private String comment;
            private InventoryMovementStatus status = InventoryMovementStatus.Requested;
            private Decimal? price;

            public Order WithComment(String comment)
            {
                this.comment = comment;
                return this;
            }

            public Order WithStatus(InventoryMovementStatus status)
            {
                switch (status)
                {
                    case InventoryMovementStatus.Requested:
                    case InventoryMovementStatus.Approved:
                    case InventoryMovementStatus.Ordered:
                    case InventoryMovementStatus.Completed:
                        this.status = status;
                        return this;
                    default:
                        throw new InvalidOperationException("Invalid status for an inventory movement with order type");
                }
            }

            public Order WithPrice(Decimal? price)
            {
                this.price = price;
                return this;
            }

            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Increase,
                    Type = InventoryMovementType.Order,
                    Status = this.status,
                    Comment = this.comment,
                    Price = this.price,
                };
            }
        }

        public class Found : InventoryMovementBuilderBase<IInventoryFoundMovement>
        {
            private String comment;

            public Found WithComment(String comment)
            {
                this.comment = comment;
                return this;
            }

            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Increase,
                    Type = InventoryMovementType.Found,
                    Status = InventoryMovementStatus.Completed,
                    Comment = this.comment,
                };
            }
        }

        public class Repair : InventoryMovementBuilderBase<IInventoryRepairMovement>
        {
            private String comment;
            private InventoryMovementStatus status = InventoryMovementStatus.Waiting;

            public Repair WithComment(String comment)
            {
                this.comment = comment;
                return this;
            }

            public Repair WithStatus(InventoryMovementStatus status)
            {
                switch (status)
                {
                    case InventoryMovementStatus.Waiting:
                    case InventoryMovementStatus.Allocated:
                    case InventoryMovementStatus.Completed:
                        this.status = status;
                        return this;
                    default:
                        throw new InvalidOperationException("Invalid status for an inventory movement with repair type");
                }
            }

            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Decrease,
                    Type = InventoryMovementType.Repair,
                    Status = this.status,
                    Comment = this.comment,
                };
            }
        }

        public class RepairFragment : InventoryMovementBuilderBase<IInventoryRepairFragmentMovement>
        {
            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Decrease,
                    Type = InventoryMovementType.RepairFragment,
                    Status = InventoryMovementStatus.Allocated,
                };
            }
        }

        public class Lost : InventoryMovementBuilderBase<IInventoryLostMovement>
        {
            private String comment;

            public Lost WithComment(String comment)
            {
                this.comment = comment;
                return this;
            }

            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Decrease,
                    Type = InventoryMovementType.Lost,
                    Status = InventoryMovementStatus.Completed,
                    Comment = this.comment,
                };
            }
        }

        public class MoveFromAnotherWorkshop : InventoryMovementBuilderBase<IInventoryMoveFromAnotherWorkshopMovement>
        {
            private String comment;

            public MoveFromAnotherWorkshop WithComment(String comment)
            {
                this.comment = comment;
                return this;
            }

            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Increase,
                    Type = InventoryMovementType.MoveFromAnotherWorkshop,
                    Status = InventoryMovementStatus.Completed,
                    Comment = this.comment,
                };
            }
        }

        public class MoveToAnotherWorkshop : InventoryMovementBuilderBase<IInventoryMoveToAnotherWorkshopMovement>
        {
            private String comment;

            public MoveToAnotherWorkshop WithComment(String comment)
            {
                this.comment = comment;
                return this;
            }

            protected override InventoryMovement PerformBuild()
            {
                return new InventoryMovement
                {
                    Direction = InventoryMovementDirection.Decrease,
                    Type = InventoryMovementType.MoveToAnotherWorkshop,
                    Status = InventoryMovementStatus.Completed,
                    Comment = this.comment,
                };
            }
        }
    }
}
