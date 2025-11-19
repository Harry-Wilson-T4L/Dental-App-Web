using System;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClient
    {
        Guid Id { get; }

        Client Entity { get; }

        IClientFeedbackCollection Feedback { get; }

        IClientReports Reports { get; }

        IClientRepairedHistory RepairedHistory { get; }
    }
}
