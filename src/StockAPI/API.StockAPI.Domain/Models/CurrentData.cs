using Newtonsoft.Json;

namespace API.StockAPI.Domain.Models
{
    public class CurrentData
    {
        [JsonProperty("Time Series (60min)")]
        public Dictionary<string, TimeSeriesData> TimeSeries { get; set; }
    }
}
