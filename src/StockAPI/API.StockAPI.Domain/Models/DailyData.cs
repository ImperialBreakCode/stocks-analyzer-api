using Newtonsoft.Json;

namespace API.StockAPI.Domain.Models
{
    public class DailyData
    {
        [JsonProperty("Time Series (Daily)")]
        public Dictionary<string, TimeSeriesData> TimeSeries { get; set; }
    }
}
