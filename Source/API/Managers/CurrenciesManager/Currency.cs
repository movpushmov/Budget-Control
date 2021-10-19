using Newtonsoft.Json;

namespace Budget_Control.Source.API.Managers.CurrenciesManager
{
    public class Currency
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public CountryName Name { get; set; }

        [JsonProperty("nominal")]
        public int Nominal { get; set; }

        public bool MainCurrency { get; set; } = false;
    }
}
