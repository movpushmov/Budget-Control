using Microsoft.Toolkit.Uwp.Helpers;
using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using System;
using Windows.UI.Xaml.Controls;
using Budget_Control.Source.API.XAML_Bridges.Utils;
using Windows.UI.Xaml;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages.Categories
{
    public sealed partial class AddCategoryDialog : ContentDialog
    {
        public string CategoryNameError
        {
            get { return (string)GetValue(CategoryNameErrorProperty); }
            set { SetValue(CategoryNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryNameErrorProperty =
            DependencyProperty.Register("CategoryNameError", typeof(string), typeof(AddCategoryDialog), new PropertyMetadata(ValidationHelper.GetErrorText(ErrorType.FieldRequiredError)));



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
            else
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
