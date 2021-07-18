using Salary_Control.Source.API;
using Salary_Control.XAML.SubPages.Stats.Sub;
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

namespace Salary_Control.XAML.SubPages.Stats
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 

    public sealed partial class Stats : Page
    {
        public Stats()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string statsType = e.AddedItems[0].ToString();

            switch (statsType)
            {
                case "Отчёт за 3 месяца":
                    chartPageFrame.Navigate(typeof(ThreeMonthsStats));
                    break;

                case "Отчёт за 6 месяцев":
                    chartPageFrame.Navigate(typeof(SixMonthsStats));
                    break;

                case "Отчёт за 12 месяцев":
                    chartPageFrame.Navigate(typeof(YearStats));
                    break;

                case "Отчёт за произвольный промежуток":
                    chartPageFrame.Navigate(typeof(CustomTimeStats));
                    break;

                default:
                    break;
            }
        }
    }
}
