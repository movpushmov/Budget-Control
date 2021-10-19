using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
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

namespace Budget_Control.XAML.SubPages.Events
{
    public sealed partial class EditEventDialog : ContentDialog
    {
        public string EventNameError
        {
            get { return (string)GetValue(EventNameErrorProperty); }
            set { SetValue(EventNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventNameErrorProperty =
            DependencyProperty.Register("EventNameError", typeof(string), typeof(EditEventDialog), new PropertyMetadata(""));



        public string EventCostError
        {
            get { return (string)GetValue(EventCostErrorProperty); }
            set { SetValue(EventCostErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventCostError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventCostErrorProperty =
            DependencyProperty.Register("EventCostError", typeof(string), typeof(EditEventDialog), new PropertyMetadata(""));



        public string EventCategoryError
        {
            get { return (string)GetValue(EventCategoryErrorProperty); }
            set { SetValue(EventCategoryErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventCategoryError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventCategoryErrorProperty =
            DependencyProperty.Register("EventCategoryError", typeof(string), typeof(EditEventDialog), new PropertyMetadata(""));





        private Event _event;
        private EntitiesList<Event> _eventsList;
        private List<EventCategory> _categories;

        public EditEventDialog(EntitiesList<Event> eventsList, Event e)
        {
            this.InitializeComponent();

            _event = e;
            _eventsList = eventsList;

            eventName.Text = e.Name;
            eventCost.Text = e.Cost.ToString();
            eventCategory.Text = e.Category.Name;

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
                    var itemsSource = _categories.Where(c => c.Name.ToLower().StartsWith(term)).ToList();

                    if (itemsSource.Count > 0)
                    {
                        sender.ItemsSource = itemsSource.Select(c => c.Name).ToList();
                    }
                    else
                    {
                        sender.ItemsSource = new List<string>() { TranslationHelper.GetText(TextType.SearchNotFound) };
                    }
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            using (var context = new DBContext())
            {
                var category = context.EventCategories.FirstOrDefault(c => c.Name == eventCategory.Text);
                var res = int.TryParse(eventCost.Text, out int cost);

                if (category == null || string.IsNullOrWhiteSpace(eventName.Text) || string.IsNullOrEmpty(eventName.Text) || !res)
                {
                    if (category == null)
                    {
                        EventCategoryError = TranslationHelper.GetText(TextType.EventInvalidCategory);
                    }
                    else
                    {
                        EventCategoryError = "";
                    }

                    if (string.IsNullOrWhiteSpace(eventName.Text) || string.IsNullOrEmpty(eventName.Text))
                    {
                        EventNameError = TranslationHelper.GetText(TextType.FieldRequiredError);
                    }
                    else
                    {
                        EventNameError = "";
                    }

                    if (!res)
                    {
                        EventCostError = TranslationHelper.GetText(TextType.InvalidCost);
                    }
                    else
                    {
                        EventCostError = "";
                    }

                    args.Cancel = true;
                    return;
                }

                if (res)
                {
                    var ev = context.Events.FirstOrDefault(e => e.Id == _event.Id);

                    if (ev != null)
                    {
                        ev.Category = category;
                        ev.Cost = Math.Abs(cost);
                        ev.Name = eventName.Text;

                        context.SaveChanges();

                        for (int i = 0; i < _eventsList.Entities.Count; i++)
                        {
                            if (_eventsList.Entities[i].Id == _event.Id)
                            {
                                _eventsList.Entities[i] = ev;
                            }
                        }
                    }
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
