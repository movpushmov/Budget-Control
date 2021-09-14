using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using Budget_Control.Source.API.XAML_Bridges.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages.Categories
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Categories : Page
    {
        public EntitiesList<EventCategory> CategoriesList { get; set; }

        public Categories()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            using (var dbContext = new DBContext())
            {
                CategoriesList = new EntitiesList<EventCategory>()
                {
                    Entities = new ObservableCollection<EventCategory>(
                        dbContext.EventCategories.Where(c => true).ToList()
                    )
                };

                emptyCategoriesBlock.Visibility = CategoriesList.Entities.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public static SolidColorBrush GetCategoryIconColor(bool isConsumption)
        {
            return new SolidColorBrush(ColorUtils.GetCategoryIconColor(isConsumption));
        }

        private void OpenCreateDialog(object sender, RoutedEventArgs e)
        {
            _ = new AddCategoryDialog((category) =>
            {
                CategoriesList.Entities.Add(category);
                emptyCategoriesBlock.Visibility = Visibility.Collapsed;
            }).ShowAsync();
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            _ = new EditCategoryDialog((sender as Button).Tag as EventCategory, CategoriesList).ShowAsync();
        }

        private async void RemoveCategory(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = TranslationHelper.GetText(TextType.RemoveCategoryTitle),
                Content = new TextBlock() { Text = TranslationHelper.GetText(TextType.RemoveDialogDescription) },

                SecondaryButtonStyle = Resources["AccentButtonStyle"] as Style,

                PrimaryButtonText = TranslationHelper.GetText(TextType.RemoveDialogSubmit),
                SecondaryButtonText = TranslationHelper.GetText(TextType.RemoveDialogCancel)
            };

            var res = await dialog.ShowAsync();

            if (res == ContentDialogResult.Primary)
            {
                var category = (sender as Button).Tag as EventCategory;

                using (var context = new DBContext())
                {
                    context.EventCategories.Remove(category);
                    context.SaveChanges();
                }

                CategoriesList.Entities.Remove(category);

                emptyCategoriesBlock.Visibility = CategoriesList.Entities.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public static string GetCategoryTooltip(bool isConsumption)
        {
            return isConsumption ?
                TranslationHelper.GetText(TextType.ExpensesCategory) : TranslationHelper.GetText(TextType.IncomesCategory);
        }
    }
}
