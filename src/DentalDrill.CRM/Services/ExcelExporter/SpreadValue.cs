using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Documents.SpreadsheetStreaming;

namespace DentalDrill.CRM.Services.ExcelExporter
{
    public enum SpreadValueType
    {
        Skipped,
        String,
        Double,
        Boolean,
        DateTime,
    }

    public struct SpreadValue
    {
        private readonly SpreadValueType type;
        private readonly String stringValue;
        private readonly Double doubleValue;
        private readonly Boolean booleanValue;
        private readonly DateTime dateTimeValue;

        private SpreadValue(SpreadValueType type, String stringValue, Double doubleValue, Boolean booleanValue, DateTime dateTimeValue)
        {
            this.type = type;
            this.stringValue = type == SpreadValueType.String ? stringValue : default;
            this.doubleValue = type == SpreadValueType.Double ? doubleValue : default;
            this.booleanValue = type == SpreadValueType.Boolean ? booleanValue : default;
            this.dateTimeValue = type == SpreadValueType.DateTime ? dateTimeValue : default;
        }

        public static SpreadValue Skipped { get; } = new SpreadValue(SpreadValueType.Skipped, default, default, default, default);

        public SpreadValueType Type => this.type;

        public Boolean IsSkipped => this.Type == SpreadValueType.Skipped;

        public String Value
        {
            get
            {
                switch (this.type)
                {
                    case SpreadValueType.Skipped:
                        return null;
                    case SpreadValueType.String:
                        return this.stringValue;
                    case SpreadValueType.Double:
                        return this.doubleValue.ToString(CultureInfo.InvariantCulture);
                    case SpreadValueType.Boolean:
                        return this.booleanValue.ToString();
                    case SpreadValueType.DateTime:
                        return this.dateTimeValue.ToString(CultureInfo.InvariantCulture);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static SpreadValue FromString(String value)
        {
            return new SpreadValue(SpreadValueType.String, value, default, default, default);
        }

        public static SpreadValue FromDouble(Double value)
        {
            return new SpreadValue(SpreadValueType.Double, default, value, default, default);
        }

        public static SpreadValue FromBoolean(Boolean value)
        {
            return new SpreadValue(SpreadValueType.Boolean, default, default, value, default);
        }

        public static SpreadValue FromDateTime(DateTime value)
        {
            return new SpreadValue(SpreadValueType.DateTime, default, default, default, value);
        }

        public void WriteToCell(ICellExporter cell)
        {
            switch (this.type)
            {
                case SpreadValueType.Skipped:
                    break;
                case SpreadValueType.String:
                    cell.SetValue(this.stringValue);
                    break;
                case SpreadValueType.Double:
                    cell.SetValue(this.doubleValue);
                    break;
                case SpreadValueType.Boolean:
                    cell.SetValue(this.booleanValue);
                    break;
                case SpreadValueType.DateTime:
                    cell.SetValue(this.dateTimeValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override String ToString()
        {
            return this.Value;
        }
    }
}
