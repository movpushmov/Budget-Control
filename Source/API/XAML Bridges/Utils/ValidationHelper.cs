using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Budget_Control.Source.API.XAML_Bridges.Utils
{
    public enum ErrorType
    {
        FieldRequiredError,
        EventInvalidCategory,
        InvalidCost
    }

    public static class ValidationHelper
    {
        private static ResourceLoader _resourceLoader;

        static ValidationHelper()
        {
            _resourceLoader = new ResourceLoader();
        }

        public static string GetErrorText(ErrorType type)
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
