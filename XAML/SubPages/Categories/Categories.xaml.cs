using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using Salary_Control.Source.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages.Categories
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Categories : Page
    {
        public CategoriesList CategoriesList { get; set; }

        public Categories()
        {
            this.InitializeComponent();

            using (var dbContext = new DBContext())
            {
                CategoriesList = new CategoriesList()
                {
                    Categories = new ObservableCollection<EventCategory>(
                        dbContext.EventCategories.Where(c => true).ToList()
                    )
                };
            }
        }

        private async void DisplayRemoveAllCategoriesDialog(object sender, RoutedEventArgs e)
        {
            if (CategoriesList.Categories.Count < 1)
            {
                return;
            } 

            ContentDialog removeAllCategoriesDialog = new ContentDialog
            {
                Title = "Удалить все категории?",
                Content = "Если вы удалите все категории, то потом не сможете отменить это действие.",
                CloseButtonText = "Отменить",
                PrimaryButtonText = "Удалить"
            };

            ContentDialogResult result = await removeAllCategoriesDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                using (var context = new DBContext())
                {
                    context.EventCategories.RemoveRange(
                        context.EventCategories.Where(c => true).ToArray()
                    );

                    await context.SaveChangesAsync();
                    CategoriesList.Categories.Clear();
                }
            }
        }

        private async void DisplayRemoveSelectedCategoriesDialog(object sender, RoutedEventArgs e)
        {
            if (CategoriesList.Categories.Count < 1 || list.SelectedItems.Count < 1)
            {
                return;
            }

            ContentDialog removeAllCategoriesDialog = new ContentDialog
            {
                Title = "Удалить выбранные категории?",
                Content = "Если вы удалите выбранные категории, то потом не сможете отменить это действие.",
                CloseButtonText = "Отменить",
                PrimaryButtonText = "Удалить"
            };

            ContentDialogResult result = await removeAllCategoriesDialog.ShowAsync();

            if (result == ContentDialogResult.Primary && list.SelectedItems.Count > 0)
            {
                using (var context = new DBContext())
                {
                    var categories = list.SelectedItems.Cast<EventCategory>().ToList();

                    context.EventCategories.RemoveRange(categories.ToArray());
                    await context.SaveChangesAsync();


                    foreach (var category in categories)
                    {
                        CategoriesList.Categories.Remove(category);
                    }
                }
            }
        }

        private void OpenCreateDialog(object sender, RoutedEventArgs e)
        {
            _ = new AddCategoryDialog(CategoriesList).ShowAsync();
        }

        private void ClearSelection(object sender, RoutedEventArgs e)
        {
            list.SelectedItems.Clear();
        }

        private void RemoveCategory(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                context.EventCategories.Remove((sender as MenuFlyoutItem).Tag as EventCategory);
                context.SaveChanges();
            }

            CategoriesList.Categories.Remove((sender as MenuFlyoutItem).Tag as EventCategory);
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate(
                typeof(EditCategory),
                new EditCategoryParams()
                {
                    Category = (sender as MenuFlyoutItem).Tag as EventCategory,
                    List = CategoriesList
                }
            );
        }
    }
}
