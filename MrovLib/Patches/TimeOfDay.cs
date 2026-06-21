using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(TimeOfDay))]
	internal class TimeOfDayPatches
	{
		[HarmonyPrefix]
		[HarmonyPatch("Start")]
		private static void Prefix()
		{
			Plugin.DebugLogger.LogDebug("TimeOfDay.Start prefix");
		}

		[HarmonyPostfix]
		[HarmonyPatch("Start")]
		private static void Postfix()
		{
			Plugin.DebugLogger.LogDebug("TimeOfDay.Start postfix");
		}

		[HarmonyPrefix]
		[HarmonyPatch("Awake")]
		private static void AwakePrefix()
		{
			Plugin.DebugLogger.LogDebug("TimeOfDay.Awake prefix");
		}

		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		private static void AwakePostfix()
		{
			Plugin.DebugLogger.LogDebug("TimeOfDay.Awake postfix");
		}
	}
}
