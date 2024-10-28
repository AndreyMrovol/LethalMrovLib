using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(StartOfRound), "OnDisable")]
	internal class StartOfRoundPatch
	{
		private static void Postfix(StartOfRound __instance)
		{
			MrovLib.EventManager.LobbyDisabled.Invoke(__instance);
		}
	}
}
