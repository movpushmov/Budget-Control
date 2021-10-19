namespace Budget_Control.Source.API.Managers.LocalizationManager
{
    class LocalizationManager : ILocalizationManager
    {
        private bool _isRussian = true;

        public LocalizationManager(string currentLanguageCode)
        {
            _isRussian = currentLanguageCode == "ru" || currentLanguageCode == "ru-ru" || currentLanguageCode == "";
        }

        public bool IsCurrentLanguageRussian()
        {
            return _isRussian;
        }
    }
}
