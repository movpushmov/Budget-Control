using Budget_Control.Source.API;
using Budget_Control.Source.API.Entities;
using Budget_Control.Source.API.XAML_Bridges;
using Budget_Control.Source.API.XAML_Bridges.Utils;
using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.SubPages
{
    public sealed partial class AddTaskModal : ContentDialog
    {
        public string TaskNameError
        {
            get { return (string)GetValue(TaskNameErrorProperty); }
            set { SetValue(TaskNameErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskNameError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskNameErrorProperty =
            DependencyProperty.Register("TaskNameError", typeof(string), typeof(AddTaskModal), new PropertyMetadata(TranslationHelper.GetText(TextType.FieldRequiredError)));

        public string TaskCostError
        {
            get { return (string)GetValue(TaskCostErrorProperty); }
            set { SetValue(TaskCostErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskCostError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskCostErrorProperty =
            DependencyProperty.Register("TaskCostError", typeof(string), typeof(AddTaskModal), new PropertyMetadata(TranslationHelper.GetText(TextType.FieldRequiredError)));

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(AddTaskModal), new PropertyMetadata(""));

        EntitiesList<UserTask> _list;
        public int _currentAmount;

        public AddTaskModal(EntitiesList<UserTask> list, int currentAmount)
        {
            this.InitializeComponent();

            _list = list;
            _currentAmount = currentAmount;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string name = taskName.Text;
            bool success = int.TryParse(taskCost.Text, out int cost);

            if (success && cost > 0 && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
            {
                var task = new UserTask()
                {
                    Name = name,
                    Cost = cost,
                    CurrentAmount = _currentAmount,
                    ImagePath = FileName != "" ? Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FileName) : "ms-appx:///Assets/DefaultTaskImage.jpg",
                };

                using (var context = new DBContext())
                {
                    context.UserTasks.Add(task);
                    context.SaveChanges();

                    _list.Entities.Add(task);
                }
            }
            else
            {
                if (!success || cost < 0)
                {
                    TaskCostError = TranslationHelper.GetText(TextType.InvalidCost);
                }
                else
                {
                    TaskCostError = "";
                }

                if (string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(name))
                {
                    TaskNameError = TranslationHelper.GetText(TextType.FieldRequiredError);
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

                try
                {
                    await file.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
                }
                catch
                {
                    File.Delete(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, file.Name));
                    await file.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
                }

                FileName = file.Name;
            }
        }
    }
}
