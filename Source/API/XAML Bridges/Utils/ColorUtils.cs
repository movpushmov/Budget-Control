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
        public static Windows.UI.Color GetCategoryIconColor(bool isConsumption)
        {
            return isConsumption ? ColorHelper.ToColor("#F86161") : ColorHelper.ToColor("#38E272");
        }
    }
}
