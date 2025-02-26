using System;
using System.Collections.Generic;
using System.Linq;
using MrovLib;
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

		[Obsolete("Use CompanyMoons instead!!!")]
		public static readonly string CompanyLevel = "Gordion";

		public static string CompanyVanillaMoon = "Gordion";

		public static List<SelectableLevel> CompanyMoons => LevelHelper.CompanyMoons;
		public static List<string> CompanyMoonNames => CompanyMoons.Select(moon => StringResolver.GetAlphanumericName(moon)).ToList();

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
