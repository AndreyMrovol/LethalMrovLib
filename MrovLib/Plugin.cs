using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MrovLib.Compatibility;
using UnityEngine.SceneManagement;

namespace MrovLib
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Logger DebugLogger;
		internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

		internal static ConfigEntry<LoggingType> DebugLogging;

		public static LLL LLL;
		public static WeatherTweaks WeatherTweaks;
		public static MapperRestoreCompat MapperRestoreCompat;
		public static ShipInventoryCompat ShipInventoryCompat;
		public static ItemWeightsCompat ItemWeightsCompat;

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			SceneManager.sceneLoaded += Patches.SceneManagerPatches.OnSceneLoaded;

			LocalConfigManager.Init(Config);

			DebugLogger = new("Debug", LoggingType.Debug);

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			DebugLogger.LogWarning($"Debug logs enabled!");

			EventManager.LobbyDisabled.AddListener(StringResolver.Reset);
			EventManager.LobbyDisabled.AddListener(LevelHelper.Reset);

			EventManager.MainMenuLoaded.AddListener(StartCompats);
		}

		internal static void LogDebug(string log)
		{
			DebugLogger.LogDebug(log);
		}

		public void StartCompats()
		{
			Plugin.logger.LogDebug("Starting compatibility patches...");

			LLL = new("imabatby.lethallevelloader");

			WeatherTweaks = new("WeatherTweaks");
			MapperRestoreCompat = new("butterystancakes.lethalcompany.restoremapper");
			ShipInventoryCompat = new("ShipInventory");
			ItemWeightsCompat = new("DarthLilo.ItemWeights");
		}
	}

	internal class LocalConfigManager : MrovLib.ConfigManager
	{
		public static ConfigEntry<LoggingType> Debug { get; private set; }

		private LocalConfigManager(ConfigFile config)
			: base(config)
		{
			Debug = configFile.Bind("General", "Logging levels", LoggingType.Basic, "Enable debug logging");

			Plugin.DebugLogging = Debug;
		}

		public static new void Init(ConfigFile config)
		{
			Instance = new LocalConfigManager(config);
		}
	}
}
