using Microsoft.Toolkit.Uwp.Helpers;

namespace Budget_Control.Source.API.XAML_Bridges.Utils
{
    public static class ColorUtils
    {
        public static Windows.UI.Color GetCategoryIconColor(bool isConsumption)
        {
            return isConsumption ? ColorHelper.ToColor("#F86161") : ColorHelper.ToColor("#38E272");
        }
    }
}
