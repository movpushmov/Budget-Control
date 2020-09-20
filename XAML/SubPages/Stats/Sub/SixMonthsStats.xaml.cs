using Microsoft.EntityFrameworkCore;
using Salary_Control.Source.API;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages.Stats.Sub
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SixMonthsStats : Page
    {
        public SixMonthsStats()
        {
            this.InitializeComponent();

            using (var context = new DBContext())
            {
                var currentDate = DateTime.Now;

                var leftDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                leftDate = leftDate.AddMonths(-6);

                var rightDate = new DateTime(
                    currentDate.Year,
                    currentDate.Month,
                    DateTime.DaysInMonth(
                        currentDate.Year,
                        currentDate.Month
                    )
                );

                var eventsGroups = context.EventsGroups
                    .Include(eg => eg.Events)
                    .ThenInclude(e => e.Category)
                    .Where(eg => eg.TimeStamp <= rightDate && eg.TimeStamp >= leftDate)
                    .ToList();

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
        }
    }
}
