using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using Budget_Control.XAML.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Main : Page
    {
        public int TotalAmount
        {
            get { return (int)GetValue(TotalAmountProperty); }
            set { SetValue(TotalAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalAmountProperty =
            DependencyProperty.Register("TotalAmount", typeof(int), typeof(Main), new PropertyMetadata(0));

        public EntitiesList<UserTask> TasksList { get; set; }

        public Main()
        {
            this.InitializeComponent();

            GetStats();
        }

        public void GetStats()
        {
            using (var context = new DBContext())
            {
                var currentDate = DateTime.Now;

                var leftDate = new DateTime(currentDate.Year, currentDate.Month, 1);

                var rightDate = new DateTime(
                    currentDate.Year,
                    currentDate.Month,
                    DateTime.DaysInMonth(
                        currentDate.Year,
                        currentDate.Month
                    )
                );

                var eventsGroups = context.EventsGroups
                    .Include(eg => eg.Events)
                    .ThenInclude(e => e.Category)
                    .Where(eg => eg.TimeStamp <= rightDate && eg.TimeStamp >= leftDate)
                    .ToList();

                var countResult = ChartsHelper.CountEvents(eventsGroups);

                expensesBlock.UpdateExpenses(countResult.minusCategoriesMap, countResult.totalMinus);

                TotalAmount = countResult.totalCount;

                TasksList = new EntitiesList<UserTask>()
                {
                    Entities = new ObservableCollection<UserTask>(context.UserTasks.Where(t => !t.IsCompleted).ToList()),
                };

                foreach (var task in TasksList.Entities)
                {
                    task.CurrentAmount = TotalAmount;
                }

                if (TasksList.Entities.Count > 0)
                {
                    tasksList.Visibility = Visibility.Visible;
                    noTasks.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tasksList.Visibility = Visibility.Collapsed;
                    noTasks.Visibility = Visibility.Visible;
                }
            }
        }

        private async void AddTaskOpenModal(object sender, RoutedEventArgs e)
        {
            var response = await new AddTaskModal(TasksList, TotalAmount).ShowAsync();

            if (response == ContentDialogResult.Secondary)
            {
                tasksList.Visibility = Visibility.Visible;
                noTasks.Visibility = Visibility.Collapsed;
            }
        }

        private async void RemoveTaskOpenModal(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Удалить задачу?",
                Content = "Это действие нельзя будет отменить.",
                PrimaryButtonText = "Удалить",
                SecondaryButtonText = "Отменить",
                SecondaryButtonStyle = this.Resources["AccentButtonStyle"] as Style
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                using (var context = new DBContext())
                {
                    var task = (sender as Button).Tag as UserTask;

                    context.UserTasks.Remove(task);
                    context.SaveChanges();

                    TasksList.Entities.Remove(task);

                    if (TasksList.Entities.Count > 0)
                    {
                        tasksList.Visibility = Visibility.Visible;
                        noTasks.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        tasksList.Visibility = Visibility.Collapsed;
                        noTasks.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void EditTaskModalOpen(object sender, RoutedEventArgs e)
        {
            _ = new EditTaskModal(TasksList, (sender as Button).Tag as UserTask).ShowAsync();
        }

        private async void SetTaskAsCompleted(object sender, RoutedEventArgs e)
        {
            var userTask = (sender as Button).Tag as UserTask;

            using (var context = new DBContext())
            {
                var task = context.UserTasks.FirstOrDefault(x => x.Id == userTask.Id);

                if (task != null)
                {
                    task.IsCompleted = true;

                    context.SaveChanges();

                    var dialog = new CompleteTaskDialog();
                    
                    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        if (dialog.Category != null)
                        {
                            var fixedTS = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                0, 0, 0
                            );

                            var eventsGroup = context.EventsGroups.FirstOrDefault(eg => eg.TimeStamp == fixedTS);

                            context.EventCategories.Attach(dialog.Category);

                            var newEvent = new Event()
                            {
                                Category = dialog.Category,
                                EventsGroup = eventsGroup,
                                Name = userTask.Name,
                                Cost = userTask.Cost
                            };

                            context.Events.Add(newEvent);
                            context.SaveChanges();

                            TotalAmount -= userTask.Cost;
                            GetStats();
                        }

                        TasksList.Entities.Remove(userTask);

                        if (TasksList.Entities.Count > 0)
                        {
                            tasksList.Visibility = Visibility.Visible;
                            noTasks.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            tasksList.Visibility = Visibility.Collapsed;
                            noTasks.Visibility = Visibility.Visible;
                        }

                        string toastText = dialog.Category != null ?
                            $"и она обошлась вам в {userTask.Cost} ₽. Данное событие уже создано на сегодняшнюю дату." :
                            "и выбрали вариант \"Не создавать событие\", поэтому нигде не будет отмечено, что вы потратили деньги на неё.";

                        var toastContent = new ToastContent()
                        {
                            Visual = new ToastVisual()
                            {
                                BindingGeneric = new ToastBindingGeneric()
                                {
                                    Children =
                                    {
                                        new AdaptiveText()
                                        {
                                            Text = "Вы выполнили цель"
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = $"Поздравляем, вы выполнили цель \"{userTask.Name}\" {toastText}"
                                        },
                                        new AdaptiveImage()
                                        {
                                            Source = userTask.ImagePath
                                        }
                                    },
                                }
                            }
                        };

                        // Create the toast notification
                        var toastNotif = new ToastNotification(toastContent.GetXml());

                        // And send the notification
                        ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
                    }
                }
            }
        }

        public static bool IsTaskCanBeCompleted(int currentAmount, int cost)
        {
            return currentAmount >= cost;
        }

        public static Style GetCompleteButtonStyle(int currentAmount, int cost)
        {
            return IsTaskCanBeCompleted(currentAmount, cost) ?
                Application.Current.Resources["AccentButtonStyle"] as Style :
                Application.Current.Resources["ButtonRevealStyle"] as Style;
        }
    }
}
