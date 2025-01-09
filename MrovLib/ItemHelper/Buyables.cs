using System.Collections.Generic;
using UnityEngine;

namespace MrovLib.ItemHelper
{
	public enum PurchaseType
	{
		Item,
		Unlockable,
		Decoration,
		Suit,
		Vehicle,
		Route,
		Bundle
	}

	public class RelatedNodes
	{
		public TerminalNode Node;
		public TerminalNode NodeConfirm;
	}

	public interface IBuyable
	{
		public string Name { get; }
		public int Price { get; }
	}

	public class BuyableThing : IBuyable
	{
		public string Name { get; set; }

		public int Price { get; set; }

		public PurchaseType Type;
		public RelatedNodes Nodes;

		public BuyableThing(Terminal terminal, RelatedNodes nodes)
		{
			Plugin.DebugLogger.LogWarning($"BuyableThing constructor: {terminal}, {nodes}");

			Nodes = nodes;
		}

		public override string ToString()
		{
			return $"{Name}";
		}
	}
}
