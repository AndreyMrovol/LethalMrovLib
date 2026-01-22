using System;
using System.Collections.Generic;
using System.Linq;

namespace MrovLib
{
	public static class LevelHelper
	{
		public static List<SelectableLevel> Levels { get; private set; }
		public static List<SelectableLevel> SortedLevels { get; private set; }

		public static SelectableLevel CompanyMoon { get; private set; }
		public static List<SelectableLevel> CompanyMoons { get; private set; }

		public static string LongestPlanetName { get; private set; }

		public static void Populate()
		{
			Levels = StartOfRound.Instance.levels.ToList();

			CompanyMoons = Levels.Where(level => !level.planetHasTime && !level.spawnEnemiesAndScrap).ToList();
			CompanyMoon = CompanyMoons.FirstOrDefault();

			SortedLevels = Levels.ToList();
			SortedLevels.Sort((a, b) => StringResolver.GetNumberlessName(a).CompareTo(StringResolver.GetNumberlessName(b)));

			LongestPlanetName = Levels
				.Select(level => StringResolver.GetNumberlessName(level))
				.OrderByDescending(name => name.Length)
				.FirstOrDefault();
		}

		public static SelectableLevel GetRandomLevel()
		{
			if (Levels == null || Levels.Count == 0)
			{
				Plugin.logger.LogError("Levels list is null or empty, cannot get a random level.");
				return null;
			}

			int randomIndex = new Random().Next(Levels.Count);
			return Levels[randomIndex];
		}

		public static bool IsVanillaLevel(SelectableLevel level)
		{
			return Defaults.IsVanillaLevel(level);
		}

		public static void Reset(StartOfRound startOfRound)
		{
			Plugin.LogDebug("LevelHelper.Reset called");

			Levels = null;
			SortedLevels = null;
			CompanyMoon = null;
		}
	}
}
