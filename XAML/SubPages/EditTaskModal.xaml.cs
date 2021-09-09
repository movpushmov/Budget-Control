using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using Budget_Control.Source.API.XAML_Bridges.Utils;
using System;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Budget_Control.XAML.SubPages
{
    public sealed partial class EditTaskModal : ContentDialog
    {
        public string TaskNameError
        {
            get { return (string)GetValue(TaskNameErrorProperty); }
            set { SetValue(TaskNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskNameErrorProperty =
            DependencyProperty.Register("TaskNameError", typeof(string), typeof(EditTaskModal), new PropertyMetadata(""));

        public string TaskCostError
        {
            get { return (string)GetValue(TaskCostErrorProperty); }
            set { SetValue(TaskCostErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskCostError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskCostErrorProperty =
            DependencyProperty.Register("TaskCostError", typeof(string), typeof(EditTaskModal), new PropertyMetadata(""));


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

            taskName.Text = task.Name;
            taskCost.Text = task.Cost.ToString();

            if (task.ImagePath != "")
            {
                FileName = Path.GetFileName(task.ImagePath);
            }

            _task = task;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string name = taskName.Text;
            bool success = int.TryParse(taskCost.Text, out int cost);

            if (success && cost > 0 && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
            {
                using (var context = new DBContext())
                {
                    var task = context.UserTasks.FirstOrDefault(x => x.Id == _task.Id);

                    if (task != null)
                    {
                        task.Name = name;
                        task.Cost = cost;
                        task.ImagePath = FileName != "" ? Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FileName) : "ms-appx:///Assets/DefaultTaskImage.jpg";
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
            else
            {
                if (!success || cost < 0)
                {
                    TaskCostError = ValidationHelper.GetErrorText(ErrorType.InvalidCost);
                }
                else
                {
                    TaskCostError = "";
                }

                if (string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(name))
                {
                    TaskNameError = ValidationHelper.GetErrorText(ErrorType.FieldRequiredError);
                }
                else
                {
                    TaskNameError = "";
                }

                args.Cancel = true;
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
