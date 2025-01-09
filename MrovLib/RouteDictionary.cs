using System.Collections.Generic;
using MrovLib.ItemHelper;

namespace MrovLib
{
	public class RouteDictionary
	{
		public Dictionary<SelectableLevel, Route> Routes = [];

		public void AddRoute(Route route)
		{
			Routes.Add(route.Level, route);
		}

		public void PopulateDictionary(List<Route> routes)
		{
			foreach (Route route in routes)
			{
				AddRoute(route);
			}
		}

		public Route GetRoute(SelectableLevel level)
		{
			return Routes[level];
		}

		public void Clear()
		{
			Routes.Clear();
		}
	}
}
