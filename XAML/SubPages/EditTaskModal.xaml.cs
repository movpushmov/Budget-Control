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

namespace Salary_Control.XAML.SubPages
{
    public sealed partial class EditTaskModal : ContentDialog
    {
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(AddTaskModal), new PropertyMetadata(""));

        EntitiesList<UserTask> _list;
        UserTask _task;

        public EditTaskModal(EntitiesList<UserTask> list, UserTask task)
        {
            this.InitializeComponent();

            _list = list;
            FileName = Path.GetFileName(task.ImagePath);
            taskName.Text = task.Name;
            taskCost.Text = task.Cost.ToString();

            _task = task;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string name = taskName.Text;
            bool success = int.TryParse(taskCost.Text, out int cost);

            if (success)
            {
                using (var context = new DBContext())
                {
                    var task = context.UserTasks.FirstOrDefault(x => x.Id == _task.Id);

                    if (task != null)
                    {
                        task.Name = name;
                        task.Cost = cost;
                        task.ImagePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FileName);
                        task.CurrentAmount = _task.CurrentAmount;

                        context.SaveChanges();

                        for (int i = 0; i < _list.Entities.Count; i++)
                        {
                            if (_list.Entities[i].Id == task.Id)
                            {
                                _list.Entities[i] = task;
                            }
                        }
                    }
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void SelectFile(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SelectFileAsync();
        }

        private async void SelectFileAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                if (FileName != "")
                {
                    File.Delete(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FileName));
                    FileName = "";
                }

                await file.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
                FileName = file.Name;
            }
        }
    }
}
