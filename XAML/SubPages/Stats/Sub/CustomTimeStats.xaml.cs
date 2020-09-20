using Microsoft.EntityFrameworkCore;
using Salary_Control.Source.API;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages.Stats.Sub
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CustomTimeStats : Page
    {
        public CustomTimeStats()
        {
            this.InitializeComponent();
        }

        private void Count(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                var currentDate = DateTime.Now;

                var leftDate = firstDate.Date.DateTime;
                var rightDate = lastDate.Date.DateTime;

                var eventsGroups = context.EventsGroups
                    .Include(eg => eg.Events)
                    .ThenInclude(ev => ev.Category)
                    .Where(eg => eg.TimeStamp <= rightDate && eg.TimeStamp >= leftDate)
                    .ToList();

                if (eventsGroups.Count > 0)
                {

                    var countResult = ChartsHelper.CountEvents(eventsGroups);

                    var plusResourceCollection = new ResourceDictionaryCollection();
                    var minusResourceCollection = new ResourceDictionaryCollection();

                    ChartsHelper.FillChartPaletter(countResult.plusCategoriesMap, plusResourceCollection);
                    ChartsHelper.FillChartPaletter(countResult.minusCategoriesMap, minusResourceCollection);

                    // Add series to chart
                    chartPlus.Series.Add(new PieSeries()
                    {
                        IndependentValuePath = "CategoryName",
                        DependentValuePath = "CategoryTotalAmount",
                        ItemsSource = ChartsHelper.CreateChartData(
                            countResult.totalPlus,
                            countResult.plusCategoriesMap
                        ),
                        Palette = plusResourceCollection
                    });

                    chartMinus.Series.Add(new PieSeries()
                    {
                        IndependentValuePath = "CategoryName",
                        DependentValuePath = "CategoryTotalAmount",
                        ItemsSource = ChartsHelper.CreateChartData(
                            countResult.totalMinus,
                            countResult.minusCategoriesMap
                        ),
                        Palette = minusResourceCollection
                    });

                    finalCount.Text = countResult.totalCount.ToString();
                } 
                else
                {
                    chartPlus.Series.Clear();
                    chartMinus.Series.Clear();
                }
            }
        }
    }
}
