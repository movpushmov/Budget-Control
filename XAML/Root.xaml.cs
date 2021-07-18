using Salary_Control.Source.Navigation;
using Salary_Control.XAML.SubPages;
using Salary_Control.XAML.SubPages.Categories;
using Salary_Control.XAML.SubPages.Stats;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Root : Page
    {
        public Root()
        {
            this.InitializeComponent();

            Navigation.Init(rootFrame);
            Navigation.Navigate(typeof(WelcomeScreen));
        }

        private void NavigationViewSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var item = (Microsoft.UI.Xaml.Controls.NavigationViewItem)sender.SelectedItem;

            switch(item.Tag)
            {
                case "stats":
                    {
                        Navigation.Navigate(typeof(Stats));
                        break;
                    }
                case "eventGroup":
                    {
                        Navigation.Navigate(typeof(EventGroup));
                        break;
                    }
                case "categories":
                    {
                        Navigation.Navigate(typeof(Categories));
                        break;
                    }
            }
        }
    }
}
