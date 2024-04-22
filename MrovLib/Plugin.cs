using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MrovLib.Compatibility;

namespace MrovLib
{
  [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
  public class Plugin : BaseUnityPlugin
  {
    internal static ManualLogSource logger;
    internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

    internal static LLL LLL;
    internal static LLLOldPlugin LLLOldPlugin;
    internal static WeatherTweaks WeatherTweaks;

    private void Awake()
    {
      logger = Logger;
      harmony.PatchAll();

      LLL = new("imabatby.lethallevelloader", "1.2.0.0");
      LLLOldPlugin = new("OldLLLLib");
      WeatherTweaks = new("WeatherTweaks");

      // Plugin startup logic
      Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
  }
}
