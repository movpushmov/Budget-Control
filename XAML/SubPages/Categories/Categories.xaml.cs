﻿using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using Salary_Control.Source.API.XAML_Bridges.Utils;
using Salary_Control.Source.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            using (var dbContext = new DBContext())
            {
                CategoriesList = new CategoriesList()
                {
                    Categories = new ObservableCollection<EventCategory>(
                        dbContext.EventCategories.Where(c => true).ToList()
                    )
                };

                emptyCategoriesBlock.Visibility = CategoriesList.Categories.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public static SolidColorBrush GetCategoryIconColor(bool isConsumption)
        {
            return new SolidColorBrush(ColorUtils.GetCategoryIconColor(isConsumption));
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

                    emptyCategoriesBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void OpenCreateDialog(object sender, RoutedEventArgs e)
        {
            _ = new AddCategoryDialog((category) =>
            {
                CategoriesList.Categories.Add(category);
                emptyCategoriesBlock.Visibility = Visibility.Collapsed;
            }).ShowAsync();
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            _ = new EditCategoryDialog((sender as Button).Tag as EventCategory).ShowAsync();
        }

        private void RemoveCategory(object sender, RoutedEventArgs e)
        {
            var category = (sender as Button).Tag as EventCategory;

            using (var context = new DBContext())
            {
                context.EventCategories.Remove(category);
                context.SaveChanges();
            }

            CategoriesList.Categories.Remove(category);

            emptyCategoriesBlock.Visibility = CategoriesList.Categories.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
