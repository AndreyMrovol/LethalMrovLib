using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(Terminal), "Start")]
	internal class TerminalStart
	{
		[HarmonyPriority(Priority.First)]
		[HarmonyPostfix]
		private static void RunMeFirst(Terminal __instance)
		{
			LevelHelper.Populate();
		}

		private static void Postfix(Terminal __instance)
		{
			MrovLib.EventManager.TerminalStart.Invoke(__instance);
		}
	}
}
