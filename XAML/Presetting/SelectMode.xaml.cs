using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.Presetting
{
    public enum CurrenciesUpdateMode
    {
        AppLaunch,
        Manually
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SelectMode : Page
    {
        private PageParameters<CurrenciesUpdateMode> _pageParameters;

        public bool ButtonEnabled
        {
            get { return (bool)GetValue(ButtonEnabledProperty); }
            set { SetValue(ButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonEnabledProperty =
            DependencyProperty.Register("ButtonEnabled", typeof(bool), typeof(SelectMode), new PropertyMetadata(false));

        private CurrenciesUpdateMode _updateMode;



        public SelectMode()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _pageParameters = (PageParameters<CurrenciesUpdateMode>)e.Parameter;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonEnabled = true;
            var selectedItem = (e.AddedItems[0]) as GridViewItem;

            if (selectedItem != null)
            {
                _updateMode = (CurrenciesUpdateMode)selectedItem.Tag;
            }

        }

        private void GoNext(object sender, RoutedEventArgs e)
        {
            _pageParameters.Next(_updateMode);
        }
    }
}
