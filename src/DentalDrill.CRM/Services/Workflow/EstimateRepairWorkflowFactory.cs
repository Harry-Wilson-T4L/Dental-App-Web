using System;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services.Workflow
{
    public class EstimateRepairWorkflowFactory : IRepairWorkflowFactory
    {
        private readonly IServiceProvider serviceProvider;

        public EstimateRepairWorkflowFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IRepairWorkflow Create()
        {
            return ActivatorUtilities.CreateInstance<EstimateRepairWorkflow>(this.serviceProvider);
        }
    }
}
