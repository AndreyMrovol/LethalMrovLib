using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrovLib
{
	public static class PlanetChecker
	{
		public static List<string> duplicates = [];
		public static bool ContainsRepeats => duplicates.Count > 0;

		public static void CheckRepeatingPlanetNames()
		{
			SelectableLevel[] levels = StartOfRound.Instance.levels;

			var planetNames = new ResolverCache<bool>();
			foreach (var level in levels)
			{
				if (planetNames.Contains(level.PlanetName))
				{
					Plugin.logger.LogWarning($"Duplicate planet name: {level.PlanetName}");
					duplicates.Add(level.PlanetName);
				}
				planetNames.Add(level.PlanetName, true);
			}
		}

		internal static IEnumerator WarnAboutRepeatingPlanetNames()
		{
			foreach (var duplicate in duplicates)
			{
				Plugin.logger.LogFatal($"Duplicate planet name: {duplicate}");
			}

			yield return new WaitForSeconds(10);

			HUDManager.Instance.DisplayTip(
				"<size=100>HALT!</size>",
				"You have multiple moons named the same in your modpack!\nNot changing this will cause issues!",
				true,
				false,
				"LC_Tip1"
			);

			yield return new WaitForSeconds(10);

			HUDManager.Instance.DisplayTip("Duplicate names:", $"{string.Join("; ", duplicates)}", false, false, "LC_Tip1");

			yield return null;
		}
	}
}
