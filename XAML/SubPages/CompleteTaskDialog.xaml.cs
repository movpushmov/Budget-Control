using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges.Utils;
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

namespace Budget_Control.XAML.SubPages
{
    public sealed partial class CompleteTaskDialog : ContentDialog
    {
        public string CategoryNameError
        {
            get { return (string)GetValue(CategoryNameErrorProperty); }
            set { SetValue(CategoryNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryNameErrorProperty =
            DependencyProperty.Register("CategoryNameError", typeof(string), typeof(CompleteTaskDialog), new PropertyMetadata(""));



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
            bool isChecked = createEvent.IsChecked ?? false;

            if (isChecked && !string.IsNullOrEmpty(newEventCategory.Text) && !string.IsNullOrWhiteSpace(newEventCategory.Text))
            {
                using (var context = new DBContext())
                {
                    Category = context.EventCategories.FirstOrDefault(c => c.Name == newEventCategory.Text);
                }

                if (Category == null)
                {
                    args.Cancel = true;
                    CategoryNameError = TranslationHelper.GetText(TextType.EventInvalidCategory);
                }
            }

            if (isChecked && (string.IsNullOrEmpty(newEventCategory.Text) || string.IsNullOrWhiteSpace(newEventCategory.Text)))
            {
                args.Cancel = true;
                CategoryNameError = TranslationHelper.GetText(TextType.FieldRequiredError);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public Visibility GetTextVisibility(bool isCreateEvent, string errorText)
        {
            return isCreateEvent && errorText != "" ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
