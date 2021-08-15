using Microsoft.EntityFrameworkCore;
using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using Salary_Control.XAML.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages
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

                var block = new ExpensesBlock(countResult.minusCategoriesMap, countResult.totalMinus);

                block.Margin = new Thickness(24, 24, 0, 16);

                expensesBlockGrid.Children.Add(block);

                TotalAmount = countResult.totalCount;

                TasksList = new EntitiesList<UserTask>()
                {
                    Entities = new ObservableCollection<UserTask>(context.UserTasks.ToList()),
                };

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
            await (new AddTaskModal(TasksList).ShowAsync());

            tasksList.Visibility = Visibility.Visible;
            noTasks.Visibility = Visibility.Collapsed;
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

        private async void EditTaskModalOpen(object sender, RoutedEventArgs e)
        {
            await (new EditTaskModal(TasksList, (sender as Button).Tag as UserTask).ShowAsync());
        }
    }
}
