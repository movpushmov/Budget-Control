using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages.Categories
{
    public sealed partial class AddCategoryDialog : ContentDialog
    {
        private Action<EventCategory> _addCategory;

        public AddCategoryDialog(Action<EventCategory> action)
        {
            this.InitializeComponent();

            _addCategory = action;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (categoryName.Text != "")
            {
                var eventCategory = new EventCategory()
                {
                    Name = categoryName.Text,
                    Color = ColorHelper.ToHex(categoryColor.Color),
                    IsConsumption = categoryIsConsumption.IsChecked ?? false
            };

                using (var dbContext = new DBContext())
                {
                    dbContext.EventCategories.Add(eventCategory);

                    dbContext.SaveChanges();
                }

                _addCategory(eventCategory);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
