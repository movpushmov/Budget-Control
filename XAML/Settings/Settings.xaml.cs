using Budget_Control.Source.API.XAML_Bridges.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.ApplicationModel;
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

        public string Link { get; set; }

        public AboutItem(string title, string description, string icon, string link)
        {
            Title = title;
            Description = description;
            Icon = icon;
            Link = link;
        }
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public List<AboutItem> AboutItems = new List<AboutItem>()
        {
            new AboutItem(
                TranslationHelper.GetText(TextType.SettingsSendReport),
                TranslationHelper.GetText(TextType.SettingsSendReportDesc), "",
                "https://github.com/movpushmov/Budget-Control/issues/new"
            ),
            new AboutItem(
                TranslationHelper.GetText(TextType.SettingsReleases),
                TranslationHelper.GetText(TextType.SettingsReleasesDesc), "",
                "https://github.com/movpushmov/Budget-Control/releases"
            ),
            new AboutItem(
                TranslationHelper.GetText(TextType.SettingsAuthors),
                TranslationHelper.GetText(TextType.SettingsAuthorsDesc), "",
                "https://github.com/movpushmov/Budget-Control/graphs/contributors"
            ),
            new AboutItem(
                TranslationHelper.GetText(TextType.SettingsDonations),
                TranslationHelper.GetText(TextType.SettingsDonationsDesc), "",
                "https://www.donationalerts.com/r/movpushmov"
            ),
            new AboutItem(
                TranslationHelper.GetText(TextType.SettingsPrivacyPolicy),
                TranslationHelper.GetText(TextType.SettingsPrivacyPolicyDesc), "",
                "https://github.com/movpushmov/Budget-Control/blob/master/privacy-policy.md"
            ),
        };

        public Settings()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Enabled;

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            appVersion.Text = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private void Redirect(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _ = Windows.System.Launcher.LaunchUriAsync(new Uri((sender as Button).Tag as string));
        }
    }
}
