namespace MrovLib.Compatibility
{
	public class WeatherRegistryCompat(string guid, string version = null) : CompatibilityHandler(guid, version)
	{
		public string GetWeather(SelectableLevel level)
		{
			return WeatherRegistry.WeatherManager.GetCurrentWeatherName(level);
		}
	}
}
