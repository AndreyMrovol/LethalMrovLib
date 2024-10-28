using HarmonyLib;

namespace MrovLib.Patches
{
	[HarmonyPatch(typeof(HangarShipDoor), "Start")]
	internal class HangarShipDoorPatch
	{
		private static void Postfix(HangarShipDoor __instance)
		{
			PlanetChecker.CheckRepeatingPlanetNames();

			if (PlanetChecker.ContainsRepeats)
			{
				Plugin.logger.LogError("Duplicate planet names detected - this will cause issues with the game!");
			}
		}
	}
}
