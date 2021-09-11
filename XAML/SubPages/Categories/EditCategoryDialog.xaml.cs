using Microsoft.Toolkit.Uwp.Helpers;
using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Budget_Control.Source.API.XAML_Bridges.Utils;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages.Categories
{
    public sealed partial class EditCategoryDialog : ContentDialog
    {
        public string CategoryNameError
        {
            get { return (string)GetValue(CategoryNameErrorProperty); }
            set { SetValue(CategoryNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryNameErrorProperty =
            DependencyProperty.Register("CategoryNameError", typeof(string), typeof(EditCategoryDialog), new PropertyMetadata(""));



        private EventCategory _category;
        private EntitiesList<EventCategory> _categoriesList;

        public EditCategoryDialog(EventCategory category, EntitiesList<EventCategory> categoriesList)
        {
            this.InitializeComponent();

            _categoriesList = categoriesList;

            categoryName.Text = category.Name;
            categoryIsConsumption.IsChecked = category.IsConsumption;
            categoryColor.Color = ColorHelper.ToColor(category.Color);

            _category = category;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (categoryName.Text != "")
            {
                using (var dbContext = new DBContext())
                {
                    var categoryWithSameName = dbContext.EventCategories.FirstOrDefault(c => c.Name == categoryName.Text && c.Id != _category.Id);

                    if (categoryWithSameName != null)
                    {
                        CategoryNameError = ValidationHelper.GetErrorText(ErrorType.CategoryNameExists);
                        args.Cancel = true;
                        return;
                    }

                    var category = dbContext.EventCategories.FirstOrDefault(c => c.Id == _category.Id);

                    category.Name = categoryName.Text;
                    category.Color = ColorHelper.ToHex(categoryColor.Color);
                    category.IsConsumption = categoryIsConsumption.IsChecked ?? false;

                    dbContext.SaveChanges();

                    for (int i = 0; i < _categoriesList.Entities.Count; i++)
                    {
                        if (_categoriesList.Entities[i].Id == _category.Id)
                        {
                            _categoriesList.Entities[i] = category;
                        }
                    }
                }
            }
            else
            {
                CategoryNameError = ValidationHelper.GetErrorText(ErrorType.FieldRequiredError);
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
