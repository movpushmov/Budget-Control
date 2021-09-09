using Microsoft.EntityFrameworkCore;
using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using Budget_Control.XAML.SubPages.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages
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

        public EntitiesList<Event> EventsList { get; set; }

        public EventGroup()
        {
            InitializeComponent();

            // TO DO: Realize runtime cross-page update without NavigationCacheMode disable.
            // NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            EventsList = new EntitiesList<Event>()
            {
                Entities = new ObservableCollection<Event>()
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

        private void CreateEvent(object sender, RoutedEventArgs e)
        {
            _ = new AddEventDialog(EventGroupTime, EventsList).ShowAsync();
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

                EventsList.Entities.Clear();

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
                        EventsList.Entities.Add(item);
                    }
                }
            }
        }

        private async void RemoveEvent(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Удалить данное событие?",
                Content = new TextBlock() { Text = "Это действие нельзя будет отменить." },

                SecondaryButtonStyle = this.Resources["AccentButtonStyle"] as Style,

                PrimaryButtonText = "Удалить",
                SecondaryButtonText = "Отменить"
            };

            var res = await dialog.ShowAsync();

            if (res == ContentDialogResult.Primary)
            {
                var ev = (sender as Button).Tag as Event;

                using (var context = new DBContext())
                {
                    context.Events.Remove(ev);
                    context.SaveChanges();
                }

                EventsList.Entities.Remove(ev);
            }
        }

        private void EditEvent(object sender, RoutedEventArgs e)
        {
            _ = new EditEventDialog(EventsList, (sender as Button).Tag as Event).ShowAsync();
        }
    }
}
