using System;

namespace DentalDrill.CRM.Domain
{
    public class DomainOperationException : Exception
    {
        public DomainOperationException(String message)
            : base(message)
        {
        }
    }
}
