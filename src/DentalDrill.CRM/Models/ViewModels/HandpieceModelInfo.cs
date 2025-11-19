using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public sealed class HandpieceModelInfo : IEquatable<HandpieceModelInfo>
    {
        public HandpieceModelInfo(String brand, String model)
        {
            this.Brand = brand;
            this.Model = model;
        }

        public String Brand { get; }

        public String Model { get; }

        public override String ToString()
        {
            return $"{this.Brand} {this.Model}";
        }

        public Boolean Equals(HandpieceModelInfo other)
        {
            if (Object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return String.Equals(this.Brand, other.Brand) && String.Equals(this.Model, other.Model);
        }

        public override Boolean Equals(Object obj)
        {
            if (Object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is HandpieceModelInfo other && this.Equals(other);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ((this.Brand != null ? this.Brand.GetHashCode() : 0) * 397) ^ (this.Model != null ? this.Model.GetHashCode() : 0);
            }
        }
    }
}
