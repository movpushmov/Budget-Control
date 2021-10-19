using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API.Managers.CurrenciesManager
{
    public interface ICurrenciesManager
    {
        List<Currency> GetCurrencies();
        void UpdateRates();
        List<Currency> SearchCurrenciesByCode(string code);
    }
}
