using Budget_Control.Source.API.Managers;
using Budget_Control.Source.API.Managers.CurrenciesManager;
using Budget_Control.Source.API.XAML_Bridges.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.Presetting
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SelectCurrencies : Page
    {
        private PageParameters<List<Currency>> _pageParameters;

        public List<Currency> Currencies = new List<Currency>();
        public ObservableCollection<Currency> SelectedCurrencies = new ObservableCollection<Currency>();

        public int SelectedCurrenciesCount
        {
            get { return (int)GetValue(SelectedCurrenciesCountProperty); }
            set { SetValue(SelectedCurrenciesCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCurrenciesCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCurrenciesCountProperty =
            DependencyProperty.Register("SelectedCurrenciesCount", typeof(int), typeof(SelectCurrencies), new PropertyMetadata(0));


        public bool CanGoNext
        {
            get { return (bool)GetValue(CanGoNextProperty); }
            set { SetValue(CanGoNextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanGoNext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanGoNextProperty =
            DependencyProperty.Register("CanGoNext", typeof(bool), typeof(SelectCurrencies), new PropertyMetadata(false));

        public SelectCurrencies()
        {
            this.InitializeComponent();

            Currencies = LogicManager.CurrenciesManager.GetCurrencies();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _pageParameters = (PageParameters<List<Currency>>)e.Parameter;
        }

        public static string FormatName(Currency currency)
        {
            return $"1 {(LogicManager.LocalizationManager.IsCurrentLanguageRussian() ? currency.Name.Ru : currency.Name.En)}";
        }

        public static Style GetSetAsMainButtonStyles(bool isMainCurrency)
        {
            return !isMainCurrency ?
                Application.Current.Resources["AccentButtonStyle"] as Style :
                new Style();
        }

        public static bool GetSetAsMainIsEnabled(bool isMainCurrency)
        {
            return !isMainCurrency;
        }

        public static string GetSetAsMainText(bool isMainCurrency)
        {
            return isMainCurrency ? "Это основная валюта" : "Выбрать основной валютой";
        }

        public Visibility IsLimitTextVisibility(int count)
        {
            return count >= 3 ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool IsSearchFieldEnabled(int count)
        {
            return count < 3;
        }

        private void SearchCurrency(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent() && sender.Text != "")
            {
                var term = sender.Text.ToLower();
                var itemsSource = Currencies
                    .Where(c => c.Code.ToLower().StartsWith(term))
                    .ToList();

                foreach (var curr in SelectedCurrencies)
                {
                    itemsSource.Remove(curr);
                }

                if (itemsSource.Count > 0)
                {
                    searchField.ItemsSource = itemsSource.Select(c =>
                        $"{c.Code} ({c.Nominal} {(LogicManager.LocalizationManager.IsCurrentLanguageRussian() ? c.Name.Ru : c.Name.En)})"
                    ).ToList();
                }
                else
                {
                    searchField.ItemsSource = new List<string>() { TranslationHelper.GetText(TextType.SearchNotFound) };
                }
            }
        }

        private Currency GetCurrency(string code)
        {
            foreach (var currency in Currencies)
            {
                if (currency.Code == code)
                {
                    return currency;
                }
            }

            return null;
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var code = (args.SelectedItem as string).Split(' ')[0];
            var currency = GetCurrency(code);

            if (currency != null)
            {
                SelectedCurrencies.Add(currency);
                SelectedCurrenciesCount++;

                sender.Text = "";
            }
        }

        private void SelectAsMain(object sender, RoutedEventArgs e)
        {
            var currency = (sender as Button).Tag as Currency;
            currency.MainCurrency = true;

            for (int i = 0; i < SelectedCurrencies.Count; i++)
            {
                if (currency.Code != SelectedCurrencies[i].Code)
                {
                    var currWithNewStatus = SelectedCurrencies[i];
                    currWithNewStatus.MainCurrency = false;

                    SelectedCurrencies[i] = currWithNewStatus;
                }
                else
                {
                    SelectedCurrencies[i] = currency;
                }
            }

            currency.MainCurrency = true;
            CanGoNext = true;
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var currency = (sender as Button).Tag as Currency;

            if (currency.MainCurrency)
            {
                currency.MainCurrency = false;
                CanGoNext = false;
            }

            SelectedCurrencies.Remove(currency);
            SelectedCurrenciesCount--;
        }
    }
}
