using Newtonsoft.Json;

public static class FormatJsonHelper
{
	public static string FormatJson(string json)
	{
		try
		{
			dynamic parsedJson = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}
		catch (JsonException)
		{
			return json;
		}
	}
}