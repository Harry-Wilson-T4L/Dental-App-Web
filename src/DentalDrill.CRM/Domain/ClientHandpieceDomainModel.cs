using System;
using System.Collections.Generic;
using System.Linq;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class ClientHandpieceDomainModel : IClientHandpiece
    {
        private readonly ClientHandpiece dataEntity;
        private readonly IClient client;
        private readonly List<IClientHandpieceComponent> components;

        public ClientHandpieceDomainModel(ClientHandpiece dataEntity, IClient client)
        {
            if (dataEntity.Components == null)
            {
                throw new ArgumentException("ClientHandpiece must include Components navigation collection", nameof(dataEntity));
            }

            this.dataEntity = dataEntity;
            this.client = client;
            this.components = this.dataEntity.Components.OrderBy(x => x.OrderNo).Select(x => new ComponentDomainModel(x)).Cast<IClientHandpieceComponent>().ToList();
        }

        public Guid Id => this.dataEntity.Id;

        public IClient Client => this.client;

        public String Brand => this.dataEntity.Brand;

        public String Model => this.dataEntity.Model;

        public String Serial => this.dataEntity.Serial;

        public IReadOnlyList<IClientHandpieceComponent> Components => this.components;

        public String ComponentsText => this.dataEntity.ComponentsText;

        private class ComponentDomainModel : IClientHandpieceComponent
        {
            private readonly ClientHandpieceComponent componentEntity;

            public ComponentDomainModel(ClientHandpieceComponent componentEntity)
            {
                this.componentEntity = componentEntity;
            }

            public String Brand => this.componentEntity.Brand;

            public String Model => this.componentEntity.Model;

            public String Serial => this.componentEntity.Serial;
        }
    }
}
