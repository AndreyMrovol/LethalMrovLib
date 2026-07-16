using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class StartOfRoundPatches
	{
		[HarmonyPrefix]
		[HarmonyPatch("Start")]
		private static void Prefix()
		{
			Plugin.DebugLogger.LogDebug("StartOfRound.Start prefix");

			Plugin.DebugLogger.LogDebug($"Is instance? {StartOfRound.Instance != null}");
		}

		[HarmonyPostfix]
		[HarmonyPatch("Start")]
		private static void Postfix()
		{
			Plugin.DebugLogger.LogDebug("StartOfRound.Start postfix");
			Plugin.DebugLogger.LogDebug($"Is instance? {StartOfRound.Instance != null}");
		}

		[HarmonyPrefix]
		[HarmonyPatch("Awake")]
		private static void AwakePrefix()
		{
			Plugin.DebugLogger.LogDebug("StartOfRound.Awake prefix");
			Plugin.DebugLogger.LogDebug($"Is instance? {StartOfRound.Instance != null}");
		}

		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		private static void AwakePostfix()
		{
			Plugin.DebugLogger.LogDebug("StartOfRound.Awake postfix");
			Plugin.DebugLogger.LogDebug($"Is instance? {StartOfRound.Instance != null}");
		}
	}
}
