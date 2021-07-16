using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API;
using Salary_Control.Source.Navigation;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class EditCategory : Page
    {
        public EditCategoryParams Params
        {
            get { return (EditCategoryParams)GetValue(ParamsProperty); }
            set { SetValue(ParamsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Params.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParamsProperty =
            DependencyProperty.Register("Params", typeof(EditCategoryParams), typeof(EditCategory), new PropertyMetadata(0));

        public Windows.UI.Color HexToColor(string hex)
        {
            return ColorHelper.ToColor(hex);
        }

        public EditCategory()
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

            Params = (EditCategoryParams)e.Parameter;
        }

        private void SaveCategory(object sender, RoutedEventArgs e)
        {
            if (categoryName.Text != "")
            {
                using (var dbContext = new DBContext())
                {
                    var category = dbContext.EventCategories.FirstOrDefault(c => c.Id == Params.Category.Id);

                    if (category != null)
                    {
                        category.Name = categoryName.Text;
                        category.Color = ColorHelper.ToHex(categoryColor.Color);

                        dbContext.SaveChanges();
                    }
                }

                for (int i = 0; i < Params.List.Categories.Count; i++)
                {
                    if (Params.List.Categories[i].Id == Params.Category.Id)
                    {
                        Params.List.Categories[i] = Params.Category;

                        break;
                    }
                }

                Navigation.Navigate(typeof(Categories.Categories));
            }
        }
    }
}
