using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessGlobal
    {
        Boolean CanReadComponent(GlobalComponent component);

        Boolean CanWriteComponent(GlobalComponent component);

        Boolean CanReadAny();

        Boolean CanReadAnyOf(params GlobalComponent[] components);

        Boolean CanReadExact(GlobalComponent component);
    }
}
