namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public enum InventoryMovementCreateType
    {
        /// <summary>Movement represents an order.</summary>
        Order = 1,

        /// <summary>Movement that corrects quantity by increasing it.</summary>
        Found = 2,

        /// <summary>Movement that corrects quantity by decreasing it.</summary>
        Lost = 1001,

        /// <summary>Movement that moves part between workshops (translates to MoveFromAnotherWorkshop and MoveToAnotherWorkshop).</summary>
        MoveBetweenWorkshops = 5000,

        /// <summary>Movement that transfer parts from another workshop.</summary>
        MoveFromAnotherWorkshop = 100,

        /// <summary>Movement that transfer parts to another workshop.</summary>
        MoveToAnotherWorkshop = 1100,
    }
}
