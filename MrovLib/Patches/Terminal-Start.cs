using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(Terminal), "Start")]
	internal class TerminalStart
	{
		private static void Postfix(Terminal __instance)
		{
			MrovLib.EventManager.TerminalStart.Invoke(__instance);
		}
	}
}