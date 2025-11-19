using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Documents.SpreadsheetStreaming;

namespace DentalDrill.CRM.Services.ExcelExporter
{
    public static partial class SpreadExtensions
    {
        public static IWorksheetExporter ConfigureColumn(this IWorksheetExporter sheet, Double? widthInPixels = null, Double? widthInCharacters = null, Boolean? hidden = null, Int32? outlineLevel = null)
        {
            using (var column = sheet.CreateColumnExporter())
            {
                if (widthInPixels.HasValue)
                {
                    column.SetWidthInPixels(widthInPixels.Value);
                }

                if (widthInCharacters.HasValue)
                {
                    column.SetWidthInCharacters(widthInCharacters.Value);
                }

                if (hidden.HasValue)
                {
                    column.SetHidden(hidden.Value);
                }

                if (outlineLevel.HasValue)
                {
                    column.SetOutlineLevel(outlineLevel.Value);
                }
            }

            return sheet;
        }
    }
}
