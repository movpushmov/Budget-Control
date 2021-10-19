using Budget_Control.Source.API.Managers.CurrenciesManager;
using Budget_Control.Source.API.Managers.LocalizationManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API.Managers
{
    public static class LogicManager
    {
        public static ICurrenciesManager CurrenciesManager { get; set; }
        public static ILocalizationManager LocalizationManager { get; set; }
    }
}
