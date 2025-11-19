using System;
using System.Threading.Tasks;
using DentalDrill.SmartFreight.Client.Models;

namespace DentalDrill.SmartFreight.Client
{
    public interface ISmartFreightClient
    {
        Task<Consignment> CalculateRateAsync(String reference, Consignment consignment);

        Task<(Consignment Result, String Request, String Response)> CalculateRateExAsync(String reference, Consignment consignment);

        Task<Consignment> ImportAsync(String reference, Consignment consignment);

        Task<(Consignment Result, String Request, String Response)> ImportExAsync(String reference, Consignment consignment);
    }
}
