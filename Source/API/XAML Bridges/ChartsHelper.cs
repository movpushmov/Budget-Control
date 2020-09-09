using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Salary_Control.Source.API.XAML_Bridges
{
    public static class ChartsHelper
    {
        public class FinancialStuff
        {
            public string CategoryName { get; set; }
            public int CategoryTotalAmount { get; set; }
        }

        public static (
            Dictionary<int, int> totalPlus,
            Dictionary<int, int> totalMinus,
            Dictionary<int, EventCategory> plusCategoriesMap,
            Dictionary<int, EventCategory> minusCategoriesMap
        ) CountEvents(List<EventsGroup> eventsGroups)
        {
            var totalPlus = new Dictionary<int, int>();
            var totalMinus = new Dictionary<int, int>();
            var plusCategoriesMap = new Dictionary<int, EventCategory>();
            var minusCategoriesMap = new Dictionary<int, EventCategory>();

            foreach (var eventsGroup in eventsGroups)
            {
                foreach (var e in eventsGroup.Events)
                {
                    if (e.Cost > 0)
                    {
                        if (totalPlus.ContainsKey(e.CategoryId))
                        {
                            totalPlus[e.CategoryId] += e.Cost;
                        }
                        else
                        {
                            totalPlus.Add(e.CategoryId, e.Cost);
                            plusCategoriesMap.Add(e.CategoryId, e.Category);
                        }
                    }
                    else if (e.Cost < 0)
                    {
                        if (totalMinus.ContainsKey(e.CategoryId))
                        {
                            totalMinus[e.CategoryId] += e.Cost;
                        }
                        else
                        {
                            totalMinus.Add(e.CategoryId, e.Cost);
                            minusCategoriesMap.Add(e.CategoryId, e.Category);
                        }
                    }
                }
            }

            return (totalPlus, totalMinus, plusCategoriesMap, minusCategoriesMap);
        }

        public static void FillChartPaletter(
            Dictionary<int, EventCategory> categoriesMap,
            ResourceDictionaryCollection resourceCollection
        )
        {
            foreach (var (key, category) in categoriesMap)
            {
                ResourceDictionary rd = new ResourceDictionary();

                var brush = new SolidColorBrush(
                    ColorHelper.ToColor(category.Color)
                );

                Style dataPointStyle = new Style(typeof(Control));
                dataPointStyle.Setters.Add(new Setter(Control.BackgroundProperty, brush));
                rd.Add("DataPointStyle", dataPointStyle);

                Style dataShapeStyle = new Style(typeof(Shape));
                dataShapeStyle.Setters.Add(new Setter(Shape.StrokeProperty, brush));
                dataShapeStyle.Setters.Add(new Setter(Shape.StrokeThicknessProperty, 2));
                dataShapeStyle.Setters.Add(new Setter(Shape.StrokeMiterLimitProperty, 1));
                dataShapeStyle.Setters.Add(new Setter(Shape.FillProperty, brush));
                rd.Add("DataShapeStyle", dataShapeStyle);

                resourceCollection.Add(rd);
            }
        }

        public static List<FinancialStuff> CreateChartData(
            Dictionary<int, int> countResult,
            Dictionary<int, EventCategory> categoriesMap
        )
        {
            var chartData = new List<FinancialStuff>();

            foreach (var (categoryId, categoryTotalAmount) in countResult)
            {
                chartData.Add(new FinancialStuff()
                {
                    CategoryName = categoriesMap[categoryId].Name,
                    CategoryTotalAmount = categoryTotalAmount
                });
            }

            return chartData;
        }
    }
}
