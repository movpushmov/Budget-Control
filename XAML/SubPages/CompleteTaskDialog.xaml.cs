using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages
{
    public sealed partial class CompleteTaskDialog : ContentDialog
    {
        private List<EventCategory> _categories;
        public EventCategory Category { get; set; }

        public CompleteTaskDialog()
        {
            this.InitializeComponent();

            using (var context = new DBContext())
            {
                _categories = context.EventCategories.Where(c => true).ToList();
            }
        }

        private void CategoryNameFieldChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent() && sender.Text != "")
            {
                var term = sender.Text.ToLower();
                using (var context = new DBContext())
                {
                    var itemsSource = _categories.Where(c => c.Name.ToLower().StartsWith(term) && c.IsConsumption).ToList();

                    if (itemsSource.Count > 0)
                    {
                        sender.ItemsSource = itemsSource.Select(c => c.Name).ToList();
                    }
                    else
                    {
                        sender.ItemsSource = new List<string>() { "Ничего не найдено" };
                    }
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            using (var context = new DBContext())
            {
                Category = context.EventCategories.FirstOrDefault(c => c.Name == newEventCategory.Text);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
