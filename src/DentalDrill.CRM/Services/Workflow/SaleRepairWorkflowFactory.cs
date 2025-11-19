using System;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services.Workflow
{
    public class SaleRepairWorkflowFactory : IRepairWorkflowFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SaleRepairWorkflowFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IRepairWorkflow Create()
        {
            return ActivatorUtilities.CreateInstance<SaleRepairWorkflow>(this.serviceProvider);
        }
    }
}
