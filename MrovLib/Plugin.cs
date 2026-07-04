using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MrovLib.Compatibility;
using UnityEngine.SceneManagement;

namespace MrovLib
{
	[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	[BepInDependency("imabatby.lethallevelloader", BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency("TeamXiaolan.DawnLib", BepInDependency.DependencyFlags.SoftDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Logger DebugLogger;
		internal static Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

		internal static ConfigEntry<LoggingType> DebugLogging;

		public static LLL LLL;
		public static DawnLib DawnLibCompat;
		public static WeatherTweaksCompat WeatherTweaks;
		public static WeatherRegistryCompat WeatherRegistryCompat;
		public static MapperRestoreCompat MapperRestoreCompat;
		public static ShipInventoryCompat ShipInventoryCompat;
		public static ItemWeightsCompat ItemWeightsCompat;
		public static LethalConstellationsCompatibility ConstellationsCompat;

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			SceneManager.sceneLoaded += Patches.SceneManagerPatches.OnSceneLoaded;

			EventManager.ContentManagerReady.AddListener(LevelHelper.ParseVanillaMoonOrder);

			LocalConfigManager.Init(Config);

			DebugLogger = new("Debug", LoggingType.Debug);

			// Plugin startup logic
			Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

			DebugLogger.LogWarning($"Debug logs enabled!");

			EventManager.LobbyDisabled.AddListener(StringResolver.Reset);
			EventManager.LobbyDisabled.AddListener(LevelHelper.Reset);

			LLL = new("imabatby.lethallevelloader");
			DawnLibCompat = new("com.github.teamxiaolan.dawnlib");

			WeatherTweaks = new("WeatherTweaks");
			WeatherRegistryCompat = new("mrov.WeatherRegistry");
			MapperRestoreCompat = new("butterystancakes.lethalcompany.restoremapper");
			ShipInventoryCompat = new("ShipInventory");
			ItemWeightsCompat = new("DarthLilo.ItemWeights");
			ConstellationsCompat = new(LethalConstellations.Plugin.PluginInfo.PLUGIN_GUID);
		}

		internal static void LogDebug(string log)
		{
			DebugLogger.LogDebug(log);
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
