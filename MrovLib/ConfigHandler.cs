using BepInEx.Configuration;

namespace MrovLib
{
	public abstract class ConfigHandler<T, CT>
	{
		public virtual ConfigEntry<CT> ConfigEntry { get; set; }

		public virtual CT DefaultValue { get; set; }

		public abstract T Value { get; }

		public ConfigHandler(string configCategory, string configTitle, CT defaultValue, ConfigDescription configDescription = null)
		{
			DefaultValue = defaultValue;
			ConfigEntry = ConfigManager.configFile.Bind(configCategory, configTitle, DefaultValue, configDescription);
		}
	}
}
