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

		public static void Populate()
		{
			Levels = StartOfRound.Instance.levels.ToList();

			CompanyMoons = Levels.Where(level => !level.planetHasTime && !level.spawnEnemiesAndScrap).ToList();
			CompanyMoon = CompanyMoons.Where(level => StringResolver.GetNumberlessName(level) == Defaults.CompanyLevel).FirstOrDefault();

			SortedLevels = Levels.ToList();
			SortedLevels.Sort((a, b) => StringResolver.GetNumberlessName(a).CompareTo(StringResolver.GetNumberlessName(b)));
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
