using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Extensions
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> Prepend(this IEnumerable<SelectListItem> selectList, String value, String title)
        {
            return new[] { new SelectListItem { Value = value, Text = title } }.Union(selectList).ToList();
        }

        public static IEnumerable<SelectListItem> Append(this IEnumerable<SelectListItem> selectList, String value, String title)
        {
            return selectList.Union(new[] { new SelectListItem { Value = value, Text = title } }).ToList();
        }

        public static IEnumerable<SelectListItem> WithNull(this IEnumerable<SelectListItem> selectList, String title)
        {
            return new[] { new SelectListItem { Value = String.Empty, Text = title } }.Union(selectList).ToList();
        }

        public static IEnumerable<SelectListItem> Rename(this IEnumerable<SelectListItem> selectList, String value, String newTitle)
        {
            var result = new List<SelectListItem>();
            var renamed = false;
            foreach (var item in selectList)
            {
                if (item.Value == value && !renamed)
                {
                    result.Add(new SelectListItem { Value = value, Text = newTitle });
                    renamed = true;
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
