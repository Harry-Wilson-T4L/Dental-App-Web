namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientInternalsFactory
    {
        IClientFeedbackCollection CreateFeedbackCollection(IClient client);

        IClientReports CreateReports(IClient client);

        IClientRepairedHistory CreateRepairedHistory(IClient client);
    }
}
