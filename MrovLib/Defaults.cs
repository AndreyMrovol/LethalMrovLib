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
		public static readonly string CompanyLevel = "Gordion";

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
