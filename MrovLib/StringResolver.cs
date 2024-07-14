using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MrovLib
{
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
						Levels.TryAdd(GetNumberlessName(level).ToLower(), level);
						Levels.TryAdd(GetAlphanumericName(level).ToLower(), level);
						Levels.TryAdd(level.PlanetName.ToLower(), level);
						Levels.TryAdd(level.name.ToLower(), level);
					});

				_levelsDictionary = Levels;

				return Levels;
			}
			set { _levelsDictionary = value; }
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
			return StringToLevel.GetValueOrDefault(str.ToLower());
		}

		public static void Reset(Terminal terminal)
		{
			StringToLevel = null;
		}
	}
}
