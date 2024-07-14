using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MrovLib.Compatibility;

namespace MrovLib
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("OldLLLLib", BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

		internal static ConfigEntry<bool> DebugLogging;

		public static LLL LLL;
		public static LLLOldPlugin LLLOldPlugin;
		public static WeatherTweaks WeatherTweaks;

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			LocalConfigManager.Init(Config);

			LLL = new("imabatby.lethallevelloader", "1.2.0.0");
			LLLOldPlugin = new("OldLLLLib");
			// LLLOldPlugin.IsTheOldLLLActive = LLLOldPlugin.IsModPresent;

			WeatherTweaks = new("WeatherTweaks");

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}

		internal static void LogDebug(string log)
		{
			if (LocalConfigManager.Debug.Value)
			{
				logger.LogDebug(log);
			}
		}
	}

	internal class LocalConfigManager : MrovLib.ConfigManager
	{
		public static ConfigEntry<bool> Debug { get; private set; }

		private LocalConfigManager(ConfigFile config)
			: base(config)
		{
			Debug = configFile.Bind("General", "Debug", false, "Enable debug logging");

			Plugin.DebugLogging = Debug;
		}

		public static new void Init(ConfigFile config)
		{
			Instance = new LocalConfigManager(config);
		}
	}
}
