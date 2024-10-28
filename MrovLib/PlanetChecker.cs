using System.Collections.Generic;

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
					Plugin.logger.LogFatal($"Duplicate planet name: {level.PlanetName}");
					duplicates.Add(level.PlanetName);
				}
				planetNames.Add(level.PlanetName, true);
			}
		}
	}
}
