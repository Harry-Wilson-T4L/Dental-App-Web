using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Permissions
{
    public class PropertiesAccessControlList<TModel>
    {
        private readonly Dictionary<String, PropertyAccessControl> properties = new Dictionary<String, PropertyAccessControl>();
        private Boolean isSealed = false;

        public void Set<TProperty>(Expression<Func<TModel, TProperty>> expression, Boolean canDisplay, Boolean canUpdate)
        {
            if (this.isSealed)
            {
                throw new InvalidOperationException("Access Control List is sealed");
            }

            var property = this.GetPropertyFromExpression(expression) ?? throw new InvalidOperationException("Invalid expression");
            this.properties[property.Name] = new PropertyAccessControl<TModel, TProperty>(canDisplay, canUpdate, property);
        }

        public void Update<TProperty>(Expression<Func<TModel, TProperty>> expression, Boolean? canDisplay = null, Boolean? canUpdate = null)
        {
            if (this.isSealed)
            {
                throw new InvalidOperationException("Access Control List is sealed");
            }

            var property = this.GetPropertyFromExpression(expression) ?? throw new InvalidOperationException("Invalid expression");
            var existingCanDisplay = false;
            var existingCanUpdate = false;
            if (this.properties.TryGetValue(property.Name, out var acl))
            {
                existingCanDisplay = acl.CanDisplay;
                existingCanUpdate = acl.CanUpdate;
            }

            if (canDisplay.HasValue)
            {
                existingCanDisplay = canDisplay.Value;
            }

            if (canUpdate.HasValue)
            {
                existingCanUpdate = canUpdate.Value;
            }

            this.properties[property.Name] = new PropertyAccessControl<TModel, TProperty>(existingCanDisplay, existingCanUpdate, property);
        }

        public void Seal()
        {
            this.isSealed = true;
        }

        public PropertyAccessControl<TModel, TProperty> For<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var property = this.GetPropertyFromExpression(expression) ?? throw new InvalidOperationException("Invalid expression");
            return (PropertyAccessControl<TModel, TProperty>)this.properties[property.Name];
        }

        public PropertyAccessControl For(String name)
        {
            return this.properties[name];
        }

        private PropertyInfo GetPropertyFromExpression(LambdaExpression expression)
        {
            if (expression.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo property)
            {
                return property;
            }
            else
            {
                return null;
            }
        }
    }
}
