using Newtonsoft.Json;

namespace API.StockAPI.Models
{
    public class StockData
    {
        [JsonProperty("Time Series (60min)")]
        public Dictionary<string, TimeSeriesData> TimeSeries { get; set; }
    }
}
