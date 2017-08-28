using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Helpers
{
    public static class MvcRenderingExtensions
    {
        public static List<SelectListItem> ConvertToDropdown<T>(this IEnumerable<T> source, Func<T, string> value, Func<T, string> text)
        {
            var dropdown = source.Select(Q => new SelectListItem
            {
                Text = text(Q),
                Value = value(Q)
            }).ToList();

            return dropdown;
        }

        public static List<SelectListItem> AddDefaultNullValue(this IEnumerable<SelectListItem> source, string text)
        {
            var result = source.ToList();

            result.Insert(0, new SelectListItem
            {
                Text = text,
                Value = null,
                Selected = true,
                Disabled = true
            });

            return result;
        }
    }
}
