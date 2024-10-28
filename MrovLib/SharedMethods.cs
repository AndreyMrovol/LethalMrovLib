using System;
using System.Collections.Generic;
using System.Linq;
using MrovLib.Compatibility;
using UnityEngine;

namespace MrovLib.API
{
	[Obsolete]
	public class SharedMethods
	{
		public static string GetWeather(SelectableLevel level)
		{
			return MrovLib.SharedMethods.GetWeather(level);
		}

		public static string GetNumberlessPlanetName(SelectableLevel level)
		{
			return MrovLib.SharedMethods.GetNumberlessPlanetName(level);
		}

		public static List<GrabbableObject> GetShipObjects()
		{
			return MrovLib.SharedMethods.GetShipObjects();
		}

		public static List<SelectableLevel> GetGameLevels()
		{
			return MrovLib.SharedMethods.GetGameLevels();
		}

		public static bool IsMoonHiddenLLL(SelectableLevel level)
		{
			return MrovLib.SharedMethods.IsMoonHiddenLLL(level);
		}

		public static bool IsMoonLockedLLL(SelectableLevel level)
		{
			return MrovLib.SharedMethods.IsMoonLockedLLL(level);
		}

		public static List<TerminalNode> GetLevelTerminalNodes(SelectableLevel level)
		{
			return MrovLib.SharedMethods.GetLevelTerminalNodes(level);
		}

		public static object GetLLLMoonsCataloguePage()
		{
			return MrovLib.SharedMethods.GetLLLMoonsCataloguePage();
		}
	}
}
