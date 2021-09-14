using Budget_Control.Source.API;
using Budget_Control.Source.API.XAML_Bridges;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages.Stats
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    ///

    public enum TimestampTemplate
    {
        ThreeMonths,
        SixMonths,
        Year
    }

    public sealed partial class Stats : Page
    {
        public Stats()
        {
            this.InitializeComponent();

            // TO DO: Realize runtime cross-page update without NavigationCacheMode disable.
            // NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void SetTimestamp(int monthsCount)
        {
            var currentDate = DateTime.Now;

            firstDate.Date = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-monthsCount);

            lastDate.Date = new DateTime(
                currentDate.Year,
                currentDate.Month,
                DateTime.DaysInMonth(
                    currentDate.Year,
                    currentDate.Month
                )
            );
        }

        private void SetTimestampWithTemplate(object sender, RoutedEventArgs e)
        {
            switch ((TimestampTemplate)(sender as Button).Tag)
            {
                case TimestampTemplate.ThreeMonths:
                    {
                        SetTimestamp(3);

                        break;
                    }
                case TimestampTemplate.SixMonths:
                    {
                        SetTimestamp(6);

                        break;
                    }
                case TimestampTemplate.Year:
                    {
                        SetTimestamp(12);

                        break;
                    }
            }
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
