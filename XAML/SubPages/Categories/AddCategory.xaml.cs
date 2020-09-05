using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using Salary_Control.Source.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddCategory : Page
    {
        private CategoriesList _categoriesList;
        public AddCategory()
        {
            this.InitializeComponent();
        }

        private void RedirectBack(object sender, RoutedEventArgs e)
        {
            Navigation.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _categoriesList = (CategoriesList)e.Parameter;
        }

        private void SaveCategory(object sender, RoutedEventArgs e)
        {
            if (categoryName.Text != "")
            {
                var eventCategory = new EventCategory()
                {
                    Name = categoryName.Text,
                    Color = ColorHelper.ToHex(categoryColor.Color)
                };

                using (var dbContext = new DBContext())
                {
                    dbContext.EventCategories.Add(eventCategory);

                    dbContext.SaveChanges();
                }

                _categoriesList.Categories.Add(eventCategory);

                Navigation.Navigate(typeof(Categories));
            }
        }
    }
}
