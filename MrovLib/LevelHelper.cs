using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MrovLib
{
	public static class LevelHelper
	{
		public static List<SelectableLevel> Levels { get; private set; }
		public static List<SelectableLevel> SortedLevels { get; private set; }

		public static SelectableLevel CompanyMoon { get; private set; }
		public static List<SelectableLevel> CompanyMoons { get; private set; }
		public static List<string> CompanyMoonNames => CompanyMoons.Select(moon => StringResolver.GetAlphanumericName(moon)).ToList();

		public static List<string> VanillaOrder = [];

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

		internal static void ParseVanillaMoonOrder()
		{
			var moonCatalogueText = ContentManager.MoonsKeyword.specialKeywordResult.displayText;
			if (string.IsNullOrWhiteSpace(moonCatalogueText))
			{
				Plugin.logger.LogWarning("Moon catalogue text is null or empty, cannot parse vanilla moon order.");
				return;
			}

			List<string> order = [];
			HashSet<string> seen = [];

			// Matches lines like:
			// * Experimentation [planetTime]
			Regex lineRegex = new(@"^\*\s+(?<name>[^\r\n\[]+)", RegexOptions.Multiline | RegexOptions.Compiled);

			foreach (Match match in lineRegex.Matches(moonCatalogueText))
			{
				string name = match.Groups["name"].Value.Trim();

				// Remove inline comments / extra text after spacing.
				int commentIndex = name.IndexOf("   //", StringComparison.Ordinal);
				if (commentIndex >= 0)
				{
					name = name[..commentIndex].Trim();
				}

				if (string.IsNullOrWhiteSpace(name))
				{
					continue;
				}

				// Keep company building out of the moon ordering.
				if (string.Equals(name, "The Company building", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				if (seen.Add(name))
				{
					order.Add(name);
				}
			}

			VanillaOrder = order;

			Levels = Levels
				.OrderBy(level =>
				{
					string numberlessName = StringResolver.GetNumberlessName(level);
					int index = VanillaOrder.FindIndex(name => string.Equals(name, numberlessName, StringComparison.OrdinalIgnoreCase));
					return index >= 0 ? index : int.MaxValue;
				})
				.ToList();
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

		public static bool IsHidden(SelectableLevel level)
		{
			if (Plugin.DawnLibCompat.IsModPresent)
			{
				bool hidden = Plugin.DawnLibCompat.GetLevelStatus(level).hidden;
				return hidden;
			}
			else if (Plugin.LLL.IsModPresent)
			{
				return SharedMethods.IsMoonHiddenLLL(level);
			}
			else if (Defaults.VanillaHiddenMoons.Contains(StringResolver.GetNumberlessName(level)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsLocked(SelectableLevel level)
		{
			if (Plugin.DawnLibCompat.IsModPresent)
			{
				bool locked = Plugin.DawnLibCompat.GetLevelStatus(level).locked;
				return locked;
			}
			else if (Plugin.LLL.IsModPresent)
			{
				return SharedMethods.IsMoonLockedLLL(level);
			}
			else
			{
				// vanilla doesn't have locked levels
				return false;
			}
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
