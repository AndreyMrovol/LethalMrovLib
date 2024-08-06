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
			Regex regex = new(@"^[0-9]+|[-_/\\\ ]");
			return new string(regex.Replace(level.PlanetName, ""));
		}

		public static SelectableLevel ResolveStringToLevel(string str)
		{
			return StringToLevel.GetValueOrDefault(str.ToLowerInvariant());
		}

		public static SelectableLevel[] ResolveStringToLevels(string str)
		{
			Plugin.LogDebug($"Resolving {str} into SelectableLevels");
			string[] levelNames = ConvertStringToArray(str);

			List<SelectableLevel> output = [];

			if (levelNames.Count() == 0)
			{
				return [];
			}

			foreach (string level in levelNames)
			{
				// if (level.ToLowerInvariant() == "all" || level.ToLowerInvariant() == "modded" || level.ToLowerInvariant() == "vanilla")
				// {
				//   SelectableLevel[] resolved = ResolveStringPlaceholderLevels(level);

				//   output.AddRange(resolved);
				//   continue;
				// }

				switch (level.ToLowerInvariant())
				{
					case "all":
					case "modded":
					case "vanilla":
						SelectableLevel[] resolved = ResolveStringPlaceholderLevels(level);

						Plugin.LogDebug($"String {level} resolved to selectable levels: {string.Join(',', resolved.Select(l => l.PlanetName))}");

						output.AddRange(resolved);
						continue;
					default:
						SelectableLevel selectableLevel = MrovLib.StringResolver.ResolveStringToLevel(level);

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

			return output.Where(listItem => listItem != null).ToArray();
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

			SelectableLevel companyLevel = StartOfRound.Instance.levels.FirstOrDefault(level =>
				GetNumberlessName(level).ToLowerInvariant() == Defaults.CompanyLevel.ToLowerInvariant()
			);

			SelectableLevel[] levels = placeholder switch
			{
				PlaceholderStringType.All => StartOfRound.Instance.levels.Where(level => level != companyLevel).ToArray(),
				PlaceholderStringType.Company => [companyLevel],
				PlaceholderStringType.Vanilla
					=> StartOfRound
						.Instance.levels.Where(level => level != companyLevel)
						.Where(level => Defaults.IsVanillaLevel(level))
						.ToArray(), // check intersection of all levels and levels names that are defined in Defaults.VanillaLevels
				PlaceholderStringType.Modded
					=> StartOfRound
						.Instance.levels.Where(level => level != companyLevel)
						.Where(level => !Defaults.IsVanillaLevel(level))
						.ToArray(),
				_ => [],
			};

			return levels.ToArray();
		}

		public static void Reset(Terminal terminal)
		{
			StringToLevel = null;
		}
	}
}
