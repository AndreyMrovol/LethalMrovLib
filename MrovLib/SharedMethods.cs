using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MrovLib.Compatibility;
using UnityEngine;

namespace MrovLib
{
	public class SharedMethods
	{
		public static string GetWeather(SelectableLevel level)
		{
			string weather;

			if (Plugin.WeatherTweaks.IsModPresent)
			{
				weather = WeatherTweaks.CurrentWeather(level);
			}
			else if (Plugin.LLL.IsModPresent)
			{
				weather = LLL.GetWeather(level);
			}
			else
			{
				weather = level.currentWeather.ToString();
			}

			Plugin.LogDebug($"Weather: {weather}");

			return weather == "None" ? "" : weather;
		}

		public static string GetNumberlessPlanetName(SelectableLevel level)
		{
			return new string(level.PlanetName.SkipWhile(c => !char.IsLetter(c)).ToArray());
		}

		public static string GetAlphanumericName(SelectableLevel level)
		{
			Regex regex = new(@"^[0-9]+|[-_/\\\ ]");
			return new string(regex.Replace(level.PlanetName, ""));
		}

		public static List<GrabbableObject> GetShipObjects()
		{
			GameObject ship = GameObject.Find("/Environment/HangarShip");
			return ship.GetComponentsInChildren<GrabbableObject>().ToList();
		}

		public static List<SelectableLevel> GetGameLevels()
		{
			Plugin.DebugLogger.LogDebug("GetGameLevels called");

			if (Plugin.LLL.IsModPresent)
			{
				Plugin.DebugLogger.LogDebug($"LLL present");
				return LLL.GetLevels();
			}
			else
			{
				Plugin.DebugLogger.LogDebug($"No LLL present");
				return LevelHelper.Levels;
			}
		}

		public static bool IsMoonHiddenLLL(SelectableLevel level)
		{
			if (Plugin.LLL.IsModPresent)
			{
				return LLL.IsMoonHidden(level);
			}

			return false;
		}

		public static bool IsMoonLockedLLL(SelectableLevel level)
		{
			if (Plugin.LLL.IsModPresent)
			{
				return LLL.IsMooonLocked(level);
			}

			return false;
		}

		public static List<TerminalNode> GetLevelTerminalNodes(SelectableLevel level)
		{
			if (Plugin.LLL.IsModPresent)
			{
				return LLL.GetLevelTerminalNodes(level);
			}
			else
			{
				return [];
			}
		}

		public static object GetLLLMoonsCataloguePage()
		{
			if (Plugin.LLL.IsModPresent)
			{
				return LLL.GetMoonsCataloguePage();
			}

			return null;
		}

		public static List<SelectableLevel> GetLevelsFromLLLTag(string tag)
		{
			if (Plugin.LLL.IsModPresent)
			{
				return LLL.GetLevelsWithTag(tag);
			}

			return [];
		}

		public static LevelWeatherType GetLevelWeather(SelectableLevel level)
		{
			return level.currentWeather;
		}
	}
}
