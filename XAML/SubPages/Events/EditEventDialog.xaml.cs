using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
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
                        sender.ItemsSource = new List<string>() { "Ничего не найдено" };
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

                if (category == null || eventName.Text == "" || eventName.Text.Trim() == "")
                {
                    return;
                }

                if (res)
                {
                    var ev = context.Events.FirstOrDefault(e => e.Id == _event.Id);

                    if (ev != null)
                    {
                        ev.Category = category;
                        ev.Cost = cost;
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
