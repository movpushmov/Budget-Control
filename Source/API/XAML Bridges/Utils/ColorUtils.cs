using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salary_Control.Source.API.XAML_Bridges.Utils
{
    public static class ColorUtils
    {
        public static Windows.UI.Color GetCategoryIconColor(EventCategory category)
        {
            if (category == null)
            {
                return Windows.UI.Colors.Transparent;
            }


            return category.IsConsumption ? ColorHelper.ToColor("#F86161") : ColorHelper.ToColor("#38E272");
        }
    }
}
