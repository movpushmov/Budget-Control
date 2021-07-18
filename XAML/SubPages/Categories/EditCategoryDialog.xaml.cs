using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using System.Linq;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages.Categories
{
    public sealed partial class EditCategoryDialog : ContentDialog
    {
        private EventCategory _category;

        public EditCategoryDialog(EventCategory category)
        {
            this.InitializeComponent();

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
                    var category = dbContext.EventCategories.FirstOrDefault(c => c.Id == _category.Id);

                    category.Name = categoryName.Text;
                    category.Color = ColorHelper.ToHex(categoryColor.Color);
                    category.IsConsumption = categoryIsConsumption.IsChecked ?? false;

                    dbContext.SaveChanges();
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
