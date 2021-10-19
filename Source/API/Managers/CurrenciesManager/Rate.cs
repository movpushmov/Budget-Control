using Newtonsoft.Json;
using System.Collections.Generic;

namespace Budget_Control.Source.API.Managers.CurrenciesManager
{
    public class CountryName
    {
        [JsonProperty("ru")]
        public string Ru { get; set; }

        [JsonProperty("en")]
        public string En { get; set; }
    }

    public class Rate
    {
        [JsonProperty("nominal")]
        public int Nominal { get; set; }

        [JsonProperty("charCode")]
        public string CharCode { get; set; }

        [JsonProperty("countryName")]
        public CountryName CountryName { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates;
    }
}
