using System;
using DentalDrill.CRM.Models.ViewModels.ClientEmails;

namespace DentalDrill.CRM.Models
{
    public static class EmailTemplates
    {
        public static Guid NewClient { get; } = Guid.Parse("{CFA4A9E5-8723-4063-A632-B6FDFDFD4AC2}");

        public static Guid NewInterstateClient { get; } = Guid.Parse("{A7C9CCC6-727F-4D34-BE92-AC808C9F76B8}");

        public static Guid NewQueenslandClient { get; } = Guid.Parse("{8CD7897B-7F59-4D06-AB1D-33926DDCF15B}");

        public static Guid MaintenanceInstructions { get; } = Guid.Parse("{A1C85779-0F00-4BF1-A202-8207E132A2D6}");

        public static Guid ScalerInformation { get; } = Guid.Parse("{95D438C2-DFD1-4314-8B88-16C24572B7A3}");

        public static Guid ChairTechnicianDetails { get; } = Guid.Parse("{888EF2ED-1DCE-44CE-9395-584808441772}");

        public static class Defaults
        {
            public static ClientEmailSendNewClientTemplateModel NewClient { get; } = new ClientEmailSendNewClientTemplateModel
            {
                NewClientOfferText = "As you would be classed as a new client there are several other offers valid if you happen to miss out on the monthly special, "
                                     + "such as 10% off your first handpiece repair bill.",
            };

            public static ClientEmailSendNewInterstateClientTemplateModel NewInterstateClient { get; } = new ClientEmailSendNewInterstateClientTemplateModel();

            public static ClientEmailSendNewQueenslandClientTemplateModel NewQueenslandClient { get; } = new ClientEmailSendNewQueenslandClientTemplateModel
            {
                LocationText = "Dental Drill Solutions has now opened a NEW Workshop in South Brisbane (as well as the original Sydney Workshop). We are your local dental handpiece repair centre - we pick-up, repair and deliver your handpieces personally where possible to Brisbane, Gold Coast and Sunshine Coast locations.",
                NewClientOfferText = "As you would be classed as a new client there are several other offers valid if you happen to miss out on the monthly special, "
                                     + "such as 10% off your first handpiece repair bill.",
            };

            public static ClientEmailSendMaintenanceInstructionsTemplateModel MaintenanceInstructions { get; } = new ClientEmailSendMaintenanceInstructionsTemplateModel();

            public static ClientEmailSendScalerInformationTemplateModel ScalerInformation { get; } = new ClientEmailSendScalerInformationTemplateModel();

            public static ClientEmailSendChairTechnicianDetailsTemplateModel ChairTechnicianDetails { get; } = new ClientEmailSendChairTechnicianDetailsTemplateModel
            {
                Following = "my call just now",
            };
        }
    }
}
