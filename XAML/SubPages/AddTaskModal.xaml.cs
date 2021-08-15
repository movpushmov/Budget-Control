using Salary_Control.Source.API;
using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Salary_Control.XAML.SubPages
{
    public sealed partial class AddTaskModal : ContentDialog
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

        public AddTaskModal(EntitiesList<UserTask> list)
        {
            this.InitializeComponent();

            _list = list;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string name = taskName.Text;
            bool success = int.TryParse(taskCost.Text, out int cost);

            if (success)
            {
                var task = new UserTask()
                {
                    Name = name,
                    Cost = cost,
                    ImagePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FileName),
                };

                using (var context = new DBContext())
                {
                    context.UserTasks.Add(task);
                    context.SaveChanges();

                    _list.Entities.Add(task);
                }
            }
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
