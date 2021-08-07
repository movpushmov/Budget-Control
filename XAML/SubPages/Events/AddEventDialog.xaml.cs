using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
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

namespace Salary_Control.XAML.SubPages.Events
{
    public sealed partial class AddEventDialog : ContentDialog
    {
        private DateTime _eventGroupTime;
        private EntitiesList<Event> _eventsList;

        public AddEventDialog(DateTime eventGroupTime, EntitiesList<Event> eventsList)
        {
            this.InitializeComponent();

            _eventGroupTime = eventGroupTime;
            _eventsList = eventsList;
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

                if (category == null || newEventName.Text == "" || newEventName.Text.Trim() == "")
                {
                    return;
                }

                if (eventsGroup != null && res)
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

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
