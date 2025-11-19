using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Documents.SpreadsheetStreaming;

namespace DentalDrill.CRM.Services.ExcelExporter
{
    public static partial class SpreadExtensions
    {
        public static IRowExporter CreateCell(this IRowExporter row, String value)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, String value, SpreadCellStyle style)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(new SpreadCellFormat
                {
                    CellStyle = style,
                });

                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, String value, SpreadCellFormat format)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(format);
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, Double value)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, Double value, SpreadCellStyle style)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(new SpreadCellFormat
                {
                    CellStyle = style,
                });

                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, Double value, SpreadCellFormat format)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(format);
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, Boolean value)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, Boolean value, SpreadCellStyle style)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(new SpreadCellFormat
                {
                    CellStyle = style,
                });

                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, Boolean value, SpreadCellFormat format)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(format);
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, DateTime value)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, DateTime value, SpreadCellStyle style)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(new SpreadCellFormat
                {
                    CellStyle = style,
                });

                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, DateTime value, SpreadCellFormat format)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(format);
                cell.SetValue(value);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, SpreadValue value)
        {
            using (var cell = row.CreateCellExporter())
            {
                value.WriteToCell(cell);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, SpreadValue value, SpreadCellStyle style)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(new SpreadCellFormat
                {
                    CellStyle = style,
                });

                value.WriteToCell(cell);
            }

            return row;
        }

        public static IRowExporter CreateCell(this IRowExporter row, SpreadValue value, SpreadCellFormat format)
        {
            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(format);
                value.WriteToCell(cell);
            }

            return row;
        }

        public static IRowExporter CreateCellIf(this IRowExporter row, Boolean condition, SpreadValue value)
        {
            if (!condition)
            {
                return row;
            }

            using (var cell = row.CreateCellExporter())
            {
                value.WriteToCell(cell);
            }

            return row;
        }

        public static IRowExporter CreateCellIf(this IRowExporter row, Boolean condition, SpreadValue value, SpreadCellStyle style)
        {
            if (!condition)
            {
                return row;
            }

            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(new SpreadCellFormat
                {
                    CellStyle = style,
                });

                value.WriteToCell(cell);
            }

            return row;
        }

        public static IRowExporter CreateCellIf(this IRowExporter row, Boolean condition, SpreadValue value, SpreadCellFormat format)
        {
            if (!condition)
            {
                return row;
            }

            using (var cell = row.CreateCellExporter())
            {
                cell.SetFormat(format);
                value.WriteToCell(cell);
            }

            return row;
        }

        public static IRowExporter CreateMultipleCells(this IRowExporter row, IEnumerable<SpreadValue> values)
        {
            foreach (var value in values)
            {
                if (value.IsSkipped)
                {
                    row.SkipCells(1);
                }
                else
                {
                    using (var cell = row.CreateCellExporter())
                    {
                        value.WriteToCell(cell);
                    }
                }
            }

            return row;
        }

        public static IRowExporter CreateMultipleCells(this IRowExporter row, IEnumerable<SpreadValue> values, SpreadCellStyle style)
        {
            foreach (var value in values)
            {
                using (var cell = row.CreateCellExporter())
                {
                    cell.SetFormat(new SpreadCellFormat { CellStyle = style, });
                    if (!value.IsSkipped)
                    {
                        value.WriteToCell(cell);
                    }
                }
            }

            return row;
        }

        public static IRowExporter CreateMultipleCells(this IRowExporter row, IEnumerable<SpreadValue> values, SpreadCellFormat format)
        {
            foreach (var value in values)
            {
                using (var cell = row.CreateCellExporter())
                {
                    cell.SetFormat(format);
                    if (!value.IsSkipped)
                    {
                        value.WriteToCell(cell);
                    }
                }
            }

            return row;
        }

        public static IRowExporter CreateMultipleCellsIf(this IRowExporter row, Boolean condition, IEnumerable<SpreadValue> values)
        {
            if (!condition)
            {
                return row;
            }

            foreach (var value in values)
            {
                if (value.IsSkipped)
                {
                    row.SkipCells(1);
                }
                else
                {
                    using (var cell = row.CreateCellExporter())
                    {
                        value.WriteToCell(cell);
                    }
                }
            }

            return row;
        }

        public static IRowExporter CreateMultipleCellsIf(this IRowExporter row, Boolean condition, IEnumerable<SpreadValue> values, SpreadCellStyle style)
        {
            if (!condition)
            {
                return row;
            }

            foreach (var value in values)
            {
                using (var cell = row.CreateCellExporter())
                {
                    cell.SetFormat(new SpreadCellFormat { CellStyle = style, });
                    if (!value.IsSkipped)
                    {
                        value.WriteToCell(cell);
                    }
                }
            }

            return row;
        }

        public static IRowExporter CreateMultipleCellsIf(this IRowExporter row, Boolean condition, IEnumerable<SpreadValue> values, SpreadCellFormat format)
        {
            if (!condition)
            {
                return row;
            }

            foreach (var value in values)
            {
                using (var cell = row.CreateCellExporter())
                {
                    cell.SetFormat(format);
                    if (!value.IsSkipped)
                    {
                        value.WriteToCell(cell);
                    }
                }
            }

            return row;
        }
    }
}
