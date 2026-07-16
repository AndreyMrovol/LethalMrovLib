using System;
using System.Collections.Generic;
using System.Linq;
using MrovLib;
using MrovLib.ContentType;
using UnityEngine;

namespace MrovLib
{
	public class Defaults
	{
		public static readonly List<string> VanillaLevels =
		[
			"Gordion",
			"Experimentation",
			"Assurance",
			"Vow",
			"March",
			"Offense",
			"Adamance",
			"Rend",
			"Dine",
			"Titan",
			"Liquidation",
			"Embrion",
			"Artifice"
		];

		public static readonly List<string> IgnoreLevels = ["Gordion"];
		public static readonly List<string> VanillaHiddenMoons = ["Liquidation", "Gordion", "Artifice", "Embrion"];
		public static readonly List<int> InaccessibleVanillaMoons = [11];

		[Obsolete("Use LevelHelper.CompanyMoons instead.")]
		public static readonly string CompanyLevel = "Gordion";

		public static string CompanyVanillaMoon = "Gordion";

		[Obsolete("Use LevelHelper.CompanyMoons instead.")]
		public static List<SelectableLevel> CompanyMoons => LevelHelper.CompanyMoons;

		[Obsolete("Use LevelHelper.CompanyMoonNames instead.")]
		public static List<string> CompanyMoonNames => LevelHelper.CompanyMoonNames;

		public static bool IsVanillaLevel(SelectableLevel level)
		{
			return VanillaLevels.Select(l => l.ToLowerInvariant()).Contains(StringResolver.GetNumberlessName(level).ToLowerInvariant());
		}

		public static readonly List<LevelWeatherType> VanillaWeathers =
		[
			LevelWeatherType.None,
			LevelWeatherType.DustClouds,
			LevelWeatherType.Foggy,
			LevelWeatherType.Rainy,
			LevelWeatherType.Stormy,
			LevelWeatherType.Flooded,
			LevelWeatherType.Eclipsed
		];
	}
}
