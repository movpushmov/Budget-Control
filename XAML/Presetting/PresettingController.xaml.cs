using Budget_Control.Source.API.Managers.CurrenciesManager;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.Presetting
{
    public class PageParameters<T>
    {
        public Action Previous { get; set; }
        public Action<T> Next { get; set; }

        public PageParameters(Action prev, Action<T> next)
        {
            Previous = prev;
            Next = next;
        }
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PresettingController : Page
    {
        private CurrenciesUpdateMode _mode = CurrenciesUpdateMode.AppLaunch;
        private List<Currency> _selectedCurrencies = new List<Currency>();

        public PresettingController()
        {
            InitializeComponent();

            currentPage.Navigate(
                typeof(SelectMode),
                new PageParameters<CurrenciesUpdateMode>(null, Next)
            );
        }

        public void Previous()
        {
            currentPage.Navigate(
                typeof(SelectMode),
                new PageParameters<CurrenciesUpdateMode>(null, Next),
                new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }
            );
        }

        public void Next(CurrenciesUpdateMode mode)
        {
            _mode = mode;
            currentPage.Navigate(
                typeof(SelectCurrencies),
                new PageParameters<List<Currency>>(Previous, Finish),
                new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight }
            );
        }

        public void Finish(List<Currency> currencies)
        {

        }
    }
}
