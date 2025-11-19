using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientRepairedItem
    {
        Guid Id { get; }

        String Brand { get; }

        String Model { get; }

        String Serial { get; }

        IHandpiece LastRepair { get; }

        IReadOnlyList<IHandpiece> Handpieces { get; }

        ClientRepairedItemStatus Status { get; }

        DateTime? RemindersLastDateTime { get; }

        Int32 RemindersCount { get; }

        Int32 TotalRemindersCount { get; }

        Task DisableAsync();

        Task EnableAsync();

        Task IncreaseSentCounterAsync();

        Task ResetRemindersCountAsync();
    }
}
