using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Workflow
{
    public class HandpieceStatusChangesList
    {
        public HandpieceStatusChangesList(HandpieceStatus source, IEnumerable<HandpieceStatus> destinations)
        {
            this.Source = source;
            this.Destinations = destinations.ToList();
        }

        public HandpieceStatus Source { get; }

        public IReadOnlyList<HandpieceStatus> Destinations { get; }
    }
}
