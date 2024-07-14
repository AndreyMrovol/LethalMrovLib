using BepInEx.Configuration;

namespace MrovLib
{
	public class ConfigManager
	{
		public static ConfigManager Instance { get; internal set; }
		public static ConfigFile configFile;

		public virtual void Init(ConfigFile config)
		{
			Instance = new ConfigManager(config);
		}

		public ConfigManager(ConfigFile config)
		{
			configFile = config;
		}
	}
}
