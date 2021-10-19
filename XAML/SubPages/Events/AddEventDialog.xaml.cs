using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using Budget_Control.Source.API.XAML_Bridges.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages.Events
{
    public sealed partial class AddEventDialog : ContentDialog
    {
        public string EventNameError
        {
            get { return (string)GetValue(EventNameErrorProperty); }
            set { SetValue(EventNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventNameErrorProperty =
            DependencyProperty.Register("EventNameError", typeof(string), typeof(AddEventDialog), new PropertyMetadata(TranslationHelper.GetText(TextType.FieldRequiredError)));



        public string EventCostError
        {
            get { return (string)GetValue(EventCostErrorProperty); }
            set { SetValue(EventCostErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventCostError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventCostErrorProperty =
            DependencyProperty.Register("EventCostError", typeof(string), typeof(AddEventDialog), new PropertyMetadata(TranslationHelper.GetText(TextType.FieldRequiredError)));



        public string EventCategoryError
        {
            get { return (string)GetValue(EventCategoryErrorProperty); }
            set { SetValue(EventCategoryErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventCategoryError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventCategoryErrorProperty =
            DependencyProperty.Register("EventCategoryError", typeof(string), typeof(AddEventDialog), new PropertyMetadata(TranslationHelper.GetText(TextType.FieldRequiredError)));





        private DateTime _eventGroupTime;
        private EntitiesList<Event> _eventsList;
        private List<EventCategory> _categories;

        public AddEventDialog(DateTime eventGroupTime, EntitiesList<Event> eventsList)
        {
            this.InitializeComponent();

            _eventGroupTime = eventGroupTime;
            _eventsList = eventsList;

            using (var context = new DBContext())
            {
                _categories = context.EventCategories.Where(c => true).ToList();
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            using (var context = new DBContext())
            {
                var category = context.EventCategories.FirstOrDefault(c => c.Name == newEventCategory.Text);
                var fixedTS = new DateTime(
                    _eventGroupTime.Year,
                    _eventGroupTime.Month,
                    _eventGroupTime.Day,
                    0, 0, 0, 0
                );

                var eventsGroup = context.EventsGroups.FirstOrDefault(eg => eg.TimeStamp == fixedTS);

                var res = int.TryParse(newEventCost.Text, out int cost);

                if (category == null || string.IsNullOrWhiteSpace(newEventName.Text) || string.IsNullOrEmpty(newEventName.Text) || !res)
                {
                    if (category == null)
                    {
                        EventCategoryError = TranslationHelper.GetText(TextType.EventInvalidCategory);
                    }
                    else
                    {
                        EventCategoryError = "";
                    }

                    if (string.IsNullOrWhiteSpace(newEventName.Text) || string.IsNullOrEmpty(newEventName.Text))
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

                if (eventsGroup != null)
                {
                    var newEvent = new Event()
                    {
                        Category = category,
                        EventsGroup = eventsGroup,
                        Name = newEventName.Text,
                        Cost = Math.Abs(cost)
                    };

                    context.Events.Add(newEvent);
                    context.SaveChanges();

                    _eventsList.Entities.Add(newEvent);
                }
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

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
