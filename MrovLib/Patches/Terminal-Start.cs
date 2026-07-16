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

			ContentManager.Init(__instance);
		}

		private static void Postfix(Terminal __instance)
		{
			MrovLib.EventManager.TerminalStart.Invoke(__instance);

			if (PlanetChecker.ContainsRepeats)
			{
				Plugin.logger.LogFatal("Duplicate planet names detected - this will cause issues with the game!");
			}
		}
	}
}
