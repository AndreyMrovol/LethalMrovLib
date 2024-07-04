using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(Terminal), "Start")]
	internal class TerminalStart
	{
		private static void Postfix()
		{
			MrovLib.EventManager.TerminalStart.Invoke();
		}
	}
}
