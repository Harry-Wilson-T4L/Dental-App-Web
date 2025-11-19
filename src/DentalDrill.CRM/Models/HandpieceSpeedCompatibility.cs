using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    [Flags]
    public enum HandpieceSpeedCompatibility
    {
        None = 0,

        Other = 1,

        LowSpeed = 2,

        HighSpeed = 4,

        All = HandpieceSpeedCompatibility.Other |
              HandpieceSpeedCompatibility.LowSpeed |
              HandpieceSpeedCompatibility.HighSpeed,
    }
}
