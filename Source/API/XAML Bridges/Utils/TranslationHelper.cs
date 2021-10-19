using System;
using System.IO;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Budget_Control.Source.API.XAML_Bridges.Utils
{
    public enum TextType
    {
        FieldRequiredError,

        EventInvalidCategory,
        ExpensesCategory,

        InvalidCost,
        IncomesCategory,

        CategoryNameExists,
        CompletedTaskToastTitle,

        SettingsSendReport,
        SettingsSendReportDesc,
        SettingsReleases,
        SettingsReleasesDesc,
        SettingsAuthors,
        SettingsAuthorsDesc,
        SettingsDonations,
        SettingsDonationsDesc,
        SettingsPrivacyPolicy,
        SettingsPrivacyPolicyDesc,
        SearchNotFound,

        TaskWithoutEventCompleted,
        TaskWithEventCompleted,

        RemoveTaskDialogTitle,
        RemoveDialogDescription,
        RemoveDialogCancel,
        RemoveDialogSubmit,
        RemoveCategoryTitle,
        RemoveEventTitle,

        NothingNotFound,
    }

    public static class TranslationHelper
    {
        private static ResourceLoader _resourceLoader;

        static TranslationHelper()
        {
            _resourceLoader = new ResourceLoader();
        }

        public static string GetText(TextType type)
    {
            return _resourceLoader.GetString(type.ToString());
        }

        public static Visibility IsErrorBlockVisible(string error)
        {
            return !string.IsNullOrEmpty(error) && !string.IsNullOrWhiteSpace(error) ?
                Visibility.Visible : Visibility.Collapsed;
        }
    }
}
