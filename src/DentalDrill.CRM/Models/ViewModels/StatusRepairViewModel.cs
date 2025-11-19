using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class StatusRepairViewModel
    {
        public String Name { get; set; }

        public ExternalHandpieceStatus HandpieceStatus { get; set; }

        [Display(Name = " ")]
        public Int32 StatusVisualisationNumber { get; set; }

        public static IReadOnlyList<StatusRepairViewModel> CreateAll()
        {
            return new List<StatusRepairViewModel>
            {
                new StatusRepairViewModel
                {
                    Name = "Received",
                    HandpieceStatus = ExternalHandpieceStatus.Received,
                    StatusVisualisationNumber = 1,
                },
                new StatusRepairViewModel
                {
                    Name = "Being Estimated",
                    HandpieceStatus = ExternalHandpieceStatus.BeingEstimated,
                    StatusVisualisationNumber = 2,
                },
                new StatusRepairViewModel
                {
                    Name = "Estimate complete",
                    HandpieceStatus = ExternalHandpieceStatus.WaitingForApproval,
                    StatusVisualisationNumber = 3,
                },
                new StatusRepairViewModel
                {
                    Name = "Estimate sent",
                    HandpieceStatus = ExternalHandpieceStatus.EstimateSent,
                    StatusVisualisationNumber = 4,
                },
                new StatusRepairViewModel
                {
                    Name = "Being Repaired",
                    HandpieceStatus = ExternalHandpieceStatus.BeingRepaired,
                    StatusVisualisationNumber = 5,
                },
                new StatusRepairViewModel
                {
                    Name = "Ready for Return",
                    HandpieceStatus = ExternalHandpieceStatus.ReadyForReturn,
                    StatusVisualisationNumber = 6,
                },
                new StatusRepairViewModel
                {
                    Name = "Unrepaired",
                    HandpieceStatus = ExternalHandpieceStatus.Cancel,
                    StatusVisualisationNumber = -7,
                },
            };
        }
    }
}
