using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessClient
    {
        Boolean CanReadComponent(ClientEntityComponent clientComponent);

        Boolean CanReadAnyComponentOf(params ClientEntityComponent[] clientComponents);

        Boolean CanWriteComponent(ClientEntityComponent clientComponent);

        Boolean CanReadField(ClientEntityField clientField);

        Boolean CanWriteField(ClientEntityField clientField);

        Boolean CanInitField(ClientEntityField clientField);

        Boolean CanWriteOrInitField(ClientEntityField clientField, Boolean init);
    }
}
