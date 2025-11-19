using System;
using System.Reflection;

namespace DentalDrill.CRM.Models.Permissions
{
    public abstract class PropertyAccessControl
    {
        protected PropertyAccessControl(Boolean canDisplay, Boolean canUpdate)
        {
            this.CanDisplay = canDisplay;
            this.CanUpdate = canUpdate;
        }

        public Boolean CanDisplay { get; }

        public Boolean CanUpdate { get; }
    }

    public class PropertyAccessControl<TModel, TProperty> : PropertyAccessControl
    {
        private readonly PropertyInfo property;

        public PropertyAccessControl(Boolean canDisplay, Boolean canUpdate, PropertyInfo property)
            : base(canDisplay, canUpdate)
        {
            this.property = property;
        }

        public TProperty TryDisplay(TProperty value)
        {
            if (this.CanDisplay)
            {
                return value;
            }
            else
            {
                return default(TProperty);
            }
        }

        public TProperty TryUpdate(TProperty originalValue, TProperty newValue)
        {
            if (this.CanUpdate)
            {
                return newValue;
            }
            else
            {
                return originalValue;
            }
        }

        public Boolean TryUpdate(TModel model, TProperty newValue)
        {
            if (this.CanUpdate)
            {
                this.property.SetValue(model, newValue);
                return true;
            }

            return false;
        }
    }
}