using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API.Managers.CurrenciesManager
{
    public class CurrenciesManager: ICurrenciesManager
    {
        private readonly string _url = "http://localhost:8005";

        private List<Rate> _rates = new List<Rate>();
        private List<Currency> _currencies = new List<Currency>();

        private List<Currency> _selectedCurrencies = new List<Currency>();
        private Currency _mainCurrence = new Currency();

        private string GetRequest(string url)
        {
            string json = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public CurrenciesManager()
        {
            _currencies = JsonConvert.DeserializeObject<List<Currency>>(GetRequest($"{_url}/multicurrency/getCurrencies"));
        }

        public List<Currency> GetCurrencies()
        {
            return _currencies;
        }

        public void UpdateRates()
        {
            _currencies = JsonConvert.DeserializeObject<List<Currency>>(GetRequest($"{_url}/multicurrency/getCurrencies"));
        }

        public List<Currency> SearchCurrenciesByCode(string code)
        {
            return JsonConvert.DeserializeObject<List<Currency>>(GetRequest($"{_url}/multicurrency/search?name={code}"));
        }
    }
}
