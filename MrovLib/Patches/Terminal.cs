using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(Terminal))]
	internal class TerminalPatches
	{
		[HarmonyPrefix]
		[HarmonyPatch("Start")]
		private static void Prefix()
		{
			Plugin.DebugLogger.LogDebug("Terminal.Start prefix");
		}

		[HarmonyPostfix]
		[HarmonyPatch("Start")]
		private static void Postfix()
		{
			Plugin.DebugLogger.LogDebug("Terminal.Start postfix");
		}

		[HarmonyPrefix]
		[HarmonyPatch("Awake")]
		private static void AwakePrefix()
		{
			Plugin.DebugLogger.LogDebug("Terminal.Awake prefix");
		}

		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		private static void AwakePostfix()
		{
			Plugin.DebugLogger.LogDebug("Terminal.Awake postfix");
		}
	}
}
