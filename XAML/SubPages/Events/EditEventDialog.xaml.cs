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
    public sealed partial class EditEventDialog : ContentDialog
    {
        private Event _event;
        private EventsList _eventsList;

        public EditEventDialog(EventsList eventsList, Event e)
        {
            this.InitializeComponent();

            _event = e;
            _eventsList = eventsList;

            eventName.Text = e.Name;
            eventCost.Text = e.Cost.ToString();
            eventCategory.Text = e.Category.Name;
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

                        for (int i = 0; i < _eventsList.Events.Count; i++)
                        {
                            if (_eventsList.Events[i].Id == _event.Id)
                            {
                                _eventsList.Events[i] = ev;
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
