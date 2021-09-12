using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Budget_Control.XAML.Settings
{
    public class AboutItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public AboutItem(string title, string description, string icon)
        {
            Title = title;
            Description = description;
            Icon = icon;
        }
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public List<AboutItem> AboutItems = new List<AboutItem>()
        {
            new AboutItem("Отправить отзыв", "Отправить отчёт о проблеме, чтобы предоставить разработчикам больше информации о ней", ""),
            new AboutItem("Заметки о выпуске", "Узнайте, что нового в Budget Control", ""),
            new AboutItem("Авторы", "Узнайте, кто учавстовал в разработке Budget Control", ""),
            new AboutItem("Поддержать нас", "Поддержите нас финансово", ""),
            new AboutItem("Политика конфиденциальности", "Прочитайте нашу политику конфиденциальности", ""),
        };

        public Settings()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
    }
}
