using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class ClientRepairedItem : IClientRepairedItem
    {
        private readonly IRepository repository;
        private readonly IDateTimeService dateTimeService;

        private readonly IClient client;

        private readonly ClientHandpiece dataEntity;

        public ClientRepairedItem(IClient client, ClientHandpiece dataEntity, IReadOnlyList<IHandpiece> handpieces, IDateTimeService dateTimeService, IRepository repository)
        {
            this.repository = repository;
            this.dateTimeService = dateTimeService;

            this.client = client;
            this.dataEntity = dataEntity;

            this.LastRepair = handpieces.OrderByDescending(x => x.ReceivedOn).First();
            this.Handpieces = handpieces;

            var lastRepairDate = this.LastRepair.CompletedOn ?? this.LastRepair.RepairedOn ?? this.LastRepair.ReceivedOn;
            if (dataEntity.RemindersDisabled || this.LastRepair.Entity.ServiceLevel?.DisableReminders == true)
            {
                this.Status = ClientRepairedItemStatus.Disabled;
            }
            else if (WasRemindedRecently(out var recentReminderStatus))
            {
                this.Status = recentReminderStatus;
            }
            else if (this.LastRepair.Status.IsActive())
            {
                this.Status = ClientRepairedItemStatus.Active;
            }
            else if (this.LastRepair.Status.IsOneOf(HandpieceStatus.Cancelled, HandpieceStatus.Unrepairable, HandpieceStatus.ReturnUnrepaired, HandpieceStatus.TradeIn))
            {
                this.Status = ClientRepairedItemStatus.Cancelled;
            }
            else if (dateTimeService.CurrentUtcTime > lastRepairDate.AddYears(2))
            {
                this.Status = ClientRepairedItemStatus.ReminderExpired;
            }
            else if (dateTimeService.CurrentUtcTime > lastRepairDate.AddYears(1))
            {
                this.Status = ClientRepairedItemStatus.RequiresMaintenance;
            }
            else
            {
                this.Status = ClientRepairedItemStatus.Complete;
            }

            Boolean WasRemindedRecently(out ClientRepairedItemStatus resultingStatus)
            {
                resultingStatus = ClientRepairedItemStatus.RemindedRecently;
                if (dataEntity.RemindersLastHandpieceId != this.LastRepair.Id || dataEntity.RemindersLastDateTime == null || dataEntity.RemindersRecentCount == 0)
                {
                    return false;
                }

                var currentMonth = this.dateTimeService.CurrentUtcTime.GetMonthIndex();
                var lastMonth = dataEntity.RemindersLastDateTime.Value.GetMonthIndex();

                if (dataEntity.RemindersRecentCount > 3)
                {
                    resultingStatus = ClientRepairedItemStatus.ReminderExpired;
                    return true;
                }

                return currentMonth - lastMonth < 3;
            }
        }

        public Guid Id => this.dataEntity.Id;

        public String Brand => this.dataEntity.Brand;

        public String Model => this.dataEntity.Model;

        public String Serial => this.dataEntity.Serial;

        public IHandpiece LastRepair { get; }

        public IReadOnlyList<IHandpiece> Handpieces { get; }

        public ClientRepairedItemStatus Status { get; }

        public DateTime? RemindersLastDateTime => this.dataEntity.RemindersLastDateTime;

        public Int32 RemindersCount => this.dataEntity.RemindersRecentCount;

        public Int32 TotalRemindersCount => this.dataEntity.RemindersTotalCount;

        public async Task DisableAsync()
        {
            this.dataEntity.RemindersDisabled = true;
            await this.repository.UpdateAsync(this.dataEntity);
        }

        public async Task EnableAsync()
        {
            this.dataEntity.RemindersDisabled = false;
            await this.repository.UpdateAsync(this.dataEntity);
        }

        public async Task IncreaseSentCounterAsync()
        {
            if (this.dataEntity.RemindersLastHandpieceId == this.LastRepair.Id)
            {
                this.dataEntity.RemindersRecentCount++;
            }
            else
            {
                this.dataEntity.RemindersRecentCount = 1;
                this.dataEntity.RemindersLastHandpieceId = this.LastRepair.Id;
            }

            this.dataEntity.RemindersTotalCount++;
            this.dataEntity.RemindersLastDateTime = this.dateTimeService.CurrentUtcTime;
            await this.repository.UpdateAsync(this.dataEntity);
        }

        public async Task ResetRemindersCountAsync()
        {
            this.dataEntity.RemindersRecentCount = 0;
            await this.repository.UpdateAsync(this.dataEntity);
        }
    }
}
