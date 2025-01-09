using System.Collections.Generic;
using System.Linq;

namespace MrovLib.ItemHelper
{
	public class BuyableBundle : BuyableThing
	{
		public List<BuyableThing> Contents = [];

		public new int Price => Contents.Sum(c => c.Price);

		public BuyableBundle(
			Terminal terminal,
			RelatedNodes nodes,
			string name,
			List<Item> items = default,
			List<UnlockableItem> unlockables = default
		)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Bundle;

			foreach (Item item in items)
			{
				BuyableThing buyable = ContentManager.GetBuyable(item);
				Contents.Add(buyable);
			}

			foreach (UnlockableItem unlockable in unlockables)
			{
				BuyableThing buyable = ContentManager.GetBuyable(unlockable);
				Contents.Add(buyable);
			}

			Name = name;

			ContentManager.Buyables.Add(this);
		}

		public BuyableBundle(Terminal terminal, RelatedNodes nodes, string name, string[] items)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Bundle;
			foreach (string item in items)
			{
				BuyableThing buyable = ContentManager.GetBuyable(item);
				Contents.Add(buyable);
			}

			Name = name;

			ContentManager.Buyables.Add(this);
		}
	}
}
