using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MrovLib
{
	public enum PlaceholderStringType
	{
		All,
		Company,
		Modded,
		Vanilla
	}

	public class StringResolver
	{
		private static Dictionary<string, SelectableLevel> _levelsDictionary = null;

		private static ResolverCache<SelectableLevel[]> stringToLevelsCache = new();

		public static Dictionary<string, SelectableLevel> StringToLevel
		{
			get
			{
				if (_levelsDictionary != null)
				{
					return _levelsDictionary;
				}

				Dictionary<string, SelectableLevel> Levels = [];

				StartOfRound
					.Instance.levels.ToList()
					.ForEach(level =>
					{
						Levels.TryAdd(GetNumberlessName(level).ToLowerInvariant(), level);
						Levels.TryAdd(GetAlphanumericName(level).ToLowerInvariant(), level);
						Levels.TryAdd(level.sceneName.ToLowerInvariant(), level);
						Levels.TryAdd(level.PlanetName.ToLowerInvariant(), level);
						Levels.TryAdd(level.name.ToLowerInvariant(), level);
					});

				_levelsDictionary = Levels;

				return Levels;
			}
			set { _levelsDictionary = value; }
		}

		// convert string to array of strings
		public static string[] ConvertStringToArray(string str)
		{
			string[] output = str.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();

			return output;
		}

		// straight-up copied from LLL (it's so fucking useful)
		public static string GetNumberlessName(SelectableLevel level)
		{
			return new string(level.PlanetName.SkipWhile(c => !char.IsLetter(c)).ToArray());
		}

		public static string GetAlphanumericName(SelectableLevel level)
		{
			Regex regex = new(@"^[0-9]+ |[^a-zA-Z0-9]");
			return new string(regex.Replace(level.PlanetName, ""));
		}

		[Obsolete("Use ResolveStringToLevels instead")]
		public static SelectableLevel ResolveStringToLevel(string str)
		{
			return StringToLevel.GetValueOrDefault(str.ToLowerInvariant());
		}

		//TODO: rework this shit a little
		public static SelectableLevel[] ResolveStringToLevels(string str)
		{
			if (stringToLevelsCache.Contains(str))
			{
				return stringToLevelsCache.Get(str);
			}

			Plugin.DebugLogger.LogInfo($"Resolving {str} into SelectableLevels");

			string[] levelNames = ConvertStringToArray(str);

			List<SelectableLevel> output = [];
			List<SelectableLevel> remove = [];

			if (levelNames.Count() == 0)
			{
				return [];
			}

			foreach (string level in levelNames)
			{
				if (level.StartsWith("!"))
				{
					Plugin.LogDebug($"String {level} will be removed from final consideration!");

					// recursive pass string without the !
					remove.AddRange(ResolveStringToLevels(level.Substring(1)));
				}

				if (level.StartsWith("$"))
				{
					Plugin.LogDebug($"String {level} is a LLL ContentTag");

					if (!Plugin.LLL.IsModPresent)
					{
						Plugin.LogDebug($"LLL is not present, skipping");
						continue;
					}

					List<SelectableLevel> resolved = Compatibility.LLL.GetLevelsWithTag(level.Substring(1));

					Plugin.LogDebug($"String {level} resolved to selectable levels: {string.Join(',', resolved.Select(l => l.PlanetName))}");
					output.AddRange(resolved);
					continue;
				}

				switch (level.ToLowerInvariant())
				{
					case "all":
					case "company":
					case "modded":
					case "custom":
					case "vanilla":
						SelectableLevel[] resolved = ResolveStringPlaceholderLevels(level);

						Plugin.LogDebug($"String {level} resolved to selectable levels: {string.Join(',', resolved.Select(l => l.PlanetName))}");

						output.AddRange(resolved);
						continue;
					default:
						SelectableLevel selectableLevel = StringToLevel.GetValueOrDefault(level.ToLowerInvariant());

						if (selectableLevel == null)
						{
							continue;
						}

						Plugin.LogDebug($"String {level} resolved to selectable level: {selectableLevel}");

						if (output.Contains(selectableLevel))
						{
							continue;
						}

						output.Add(selectableLevel);
						break;
				}
			}

			SelectableLevel[] outputLevels = output.Where(listItem => listItem != null).Where(listItem => !remove.Contains(listItem)).ToArray();
			stringToLevelsCache.Add(str, outputLevels);
			Plugin.DebugLogger.LogInfo($"Resolved {str} into {string.Join(',', outputLevels.Select(l => l.PlanetName))}");
			return outputLevels;
		}

		public static PlaceholderStringType GetPlaceholderType(string input)
		{
			return input.ToLowerInvariant() switch
			{
				"all" => PlaceholderStringType.All,
				"company" => PlaceholderStringType.Company,
				"modded" => PlaceholderStringType.Modded,
				"custom" => PlaceholderStringType.Modded,
				"vanilla" => PlaceholderStringType.Vanilla,
				_ => PlaceholderStringType.All,
			};
		}

		public static SelectableLevel[] ResolveStringPlaceholderLevels(string input)
		{
			PlaceholderStringType placeholder = GetPlaceholderType(input);

			List<SelectableLevel> companyLevels = LevelHelper.CompanyMoons;

			Plugin.logger.LogInfo($"Resolving placeholder {input} into SelectableLevels");
			Plugin.logger.LogWarning($"Company levels: {string.Join(',', companyLevels.Select(l => l.PlanetName))}");

			SelectableLevel[] levels = placeholder switch
			{
				PlaceholderStringType.All => StartOfRound.Instance.levels.Where(level => level != companyLevels.Contains(level)).ToArray(),
				PlaceholderStringType.Company => companyLevels.ToArray(),
				PlaceholderStringType.Vanilla
					=> StartOfRound
						.Instance.levels.Where(level => companyLevels.Contains(level))
						.Where(level => Defaults.IsVanillaLevel(level))
						.ToArray(), // check intersection of all levels and levels names that are defined in Defaults.VanillaLevels
				PlaceholderStringType.Modded
					=> StartOfRound
						.Instance.levels.Where(level => level != companyLevels.Contains(level))
						.Where(level => !Defaults.IsVanillaLevel(level))
						.ToArray(),
				_ => [],
			};

			return levels.ToArray();
		}

		public static void Reset(StartOfRound startOfRound)
		{
			Plugin.LogDebug("StringResolver.Reset called");

			StringToLevel = null;
			stringToLevelsCache.Reset();
		}
	}
}
