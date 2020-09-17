using Microsoft.EntityFrameworkCore;
using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class EventGroup : Page
    {
        public DateTime EventGroupTime
        {
            get { return (DateTime)GetValue(EventGroupTimeProperty); }
            set { 
                SetValue(EventGroupTimeProperty, value);
                GetEventGroup(value);
            }
        }

        // Using a DependencyProperty as the backing store for EventGroupTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventGroupTimeProperty =
            DependencyProperty.Register("EventGroupTime", typeof(DateTime), typeof(EventGroup), new PropertyMetadata(0));

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(EventGroup), new PropertyMetadata(0));

        public EventsList EventsList { get; set; }

        public Event CurrentEvent
        {
            get { return (Event)GetValue(CurrentEventProperty); }
            set { SetValue(CurrentEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEventProperty =
            DependencyProperty.Register("CurrentEvent", typeof(Event), typeof(EventGroup), new PropertyMetadata(0));



        public EventGroup()
        {
            InitializeComponent();

            EventsList = new EventsList()
            {
                Events = new ObservableCollection<Event>()
            };

            var dateTime = DateTime.Now;
            EventGroupTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);

            calendarDatePicker.Date = EventGroupTime;

            IsEditing = false;
        }

        private void CalendarDatePickerDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (sender.Date.HasValue)
            {
                var ts = sender.Date.Value.LocalDateTime;

                EventGroupTime = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
            }
        }

        private void ClearSelection(object sender, RoutedEventArgs e)
        {
            if (EventsList.Events.Count > 0)
            {
                list.SelectedItems.Clear();
            }
        }

        private void EnterEditMode(object sender, RoutedEventArgs e)
        {
            var ev = (sender as MenuFlyoutItem).Tag as Event;
            editEventName.Text = ev.Name;

            if (ev.Cost < 0)
            {
                editEventCost.Text = (ev.Cost * -1).ToString();
                isMinusEditForm.IsChecked = true;
            }
            else
            {
                editEventCost.Text = ev.Cost.ToString();
                isMinusEditForm.IsChecked = false;
            }
            editEventCategory.Text = ev.Category.Name;

            CurrentEvent = ev;
            IsEditing = true;
        }

        private void ClearEditForm()
        {
            editEventName.Text = "";
            editEventCost.Text = "";
            editEventCategory.Text = "";
        }

        private void ExitFromEditMode(object sender = null, RoutedEventArgs e = null)
        {
            ClearEditForm();

            CurrentEvent = null;
            IsEditing = false;
        }

        private void CreateEvent(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                var category = context.EventCategories.FirstOrDefault(c => c.Name == newEventCategory.Text);
                var fixedTS = new DateTime(
                    EventGroupTime.Year,
                    EventGroupTime.Month,
                    EventGroupTime.Day,
                    0, 0, 0, 0
                );

                var eventsGroup = context.EventsGroups.FirstOrDefault(eg => eg.TimeStamp == fixedTS);

                var res = int.TryParse(newEventCost.Text, out int cost);

                bool isMinusChecked = isMinus.IsChecked.HasValue && isMinus.IsChecked.Value;
                if (isMinusChecked)
                {
                    cost *= -1;
                }

                if (category != null && eventsGroup != null && res)
                {
                    var newEvent = new Event()
                    {
                        Category = category,
                        EventsGroup = eventsGroup,
                        Name = newEventName.Text,
                        Cost = cost
                    };

                    context.Events.Add(newEvent);
                    context.SaveChanges();

                    EventsList.Events.Add(newEvent);

                    newEventName.Text = "";
                    newEventCost.Text = "";
                    newEventCategory.Text = "";
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
                    var all = context.EventCategories.Where(c => true).ToList();

                    var itemsSource = all.Where(c => c.Name.ToLower().StartsWith(term)).ToList();

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

        private async void DisplayRemoveAllCategoriesDialog(object sender, RoutedEventArgs e)
        {
            if (EventsList.Events.Count > 0)
            {
                ContentDialog removeAllCategoriesDialog = new ContentDialog
                {
                    Title = "Удалить все события?",
                    Content = "Если вы удалите все категории, то потом не сможете отменить это действие.",
                    CloseButtonText = "Отменить",
                    PrimaryButtonText = "Удалить"
                };

                ContentDialogResult result = await removeAllCategoriesDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    using (var context = new DBContext())
                    {
                        context.Events.RemoveRange(
                            context.Events.Where(c => true).ToArray()
                        );

                        await context.SaveChangesAsync();
                        EventsList.Events.Clear();
                    }
                }
            }
        }

        private async void DisplayRemoveSelectedCategoriesDialog(object sender, RoutedEventArgs e)
        {
            if (EventsList.Events.Count > 0)
            {
                ContentDialog removeAllCategoriesDialog = new ContentDialog
                {
                    Title = "Удалить выбранные события?",
                    Content = "Если вы удалите выбранные события, то потом не сможете отменить это действие.",
                    CloseButtonText = "Отменить",
                    PrimaryButtonText = "Удалить"
                };

                ContentDialogResult result = await removeAllCategoriesDialog.ShowAsync();

                if (result == ContentDialogResult.Primary && list.SelectedItems.Count > 0)
                {
                    using (var context = new DBContext())
                    {
                        var events = list.SelectedItems.Cast<Event>().ToList();

                        context.Events.RemoveRange(events.ToArray());
                        await context.SaveChangesAsync();


                        foreach (var ev in events)
                        {
                            EventsList.Events.Remove(ev);
                        }
                    }
                }
            }
        }

        private Visibility GetAddFormVisibility(bool isEditing)
        {
            return isEditing ? Visibility.Collapsed : Visibility.Visible;
        }

        private Visibility GetEditFormVisibility(bool isEditing)
        {
            return isEditing ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GetEventGroup(DateTime time)
        {
            using (var context = new DBContext())
            {
                var fixedTS = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, 0);

                EventsList.Events.Clear();

                var eventsGroup = context.EventsGroups
                    .Include(e => e.Events)
                    .ThenInclude(e => e.Category)
                    .FirstOrDefault(eg => eg.TimeStamp == fixedTS);

                if (eventsGroup == null)
                {
                    eventsGroup = new EventsGroup()
                    {
                        Events = new List<Event>(),
                        TimeStamp = fixedTS
                    };

                    context.EventsGroups.Add(eventsGroup);
                    context.SaveChanges();
                }
                else if (eventsGroup.Events.Count > 0)
                {
                    foreach (var item in eventsGroup.Events)
                    {
                        EventsList.Events.Add(item);
                    }
                }
            }
        }

        private void RemoveEvent(object sender, RoutedEventArgs e)
        {
            if (EventsList.Events.Count > 0)
            {
                using (var context = new DBContext())
                {
                    context.Events.Remove((sender as MenuFlyoutItem).Tag as Event);
                    context.SaveChanges();
                }

                EventsList.Events.Remove((sender as MenuFlyoutItem).Tag as Event);
            }
        }
        private void EditEvent(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                var dbEvent = context.Events.FirstOrDefault(c => c.Id == CurrentEvent.Id);

                var category = context.EventCategories.FirstOrDefault(c => c.Name == editEventCategory.Text);
                var res = int.TryParse(editEventCost.Text, out int cost);

                if (res && CurrentEvent != null && dbEvent != null)
                {
                    dbEvent.Category = category;

                    bool isMinusChecked = isMinusEditForm.IsChecked.HasValue && isMinusEditForm.IsChecked.Value;
                    if (isMinusChecked && cost < 0)
                    {
                        dbEvent.Cost = cost;
                    }
                    else if (isMinusChecked && cost > 0)
                    {
                        dbEvent.Cost = cost * -1;
                    }
                    else if (!isMinusChecked && cost < 0)
                    {
                        dbEvent.Cost = cost * -1;
                    }
                    else if (!isMinusChecked && cost > 0)
                    {
                        dbEvent.Cost = cost;
                    }

                    dbEvent.Name = editEventName.Text;

                    for (int i = 0; i < EventsList.Events.Count; i++)
                    {
                        if (EventsList.Events[i].Id == dbEvent.Id)
                            EventsList.Events[i] = dbEvent;
                    }

                    context.SaveChanges();

                    ExitFromEditMode();
                }
            }
        }
    }
}
