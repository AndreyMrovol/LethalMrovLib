using BepInEx.Configuration;

namespace MrovLib
{
	internal static class ConfigManager
	{
		public static ConfigEntry<bool> Debug { get; private set; }

		public static void Init(ConfigFile config)
		{
			Debug = config.Bind("General", "Debug", false);
		}
	}
}
