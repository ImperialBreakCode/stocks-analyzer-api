namespace API.Analyzer
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 40 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}