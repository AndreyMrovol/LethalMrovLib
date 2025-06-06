using System;
using System.Linq;

namespace MrovLib.ContentType
{
	public class BuyableDecoration : BuyableUnlockable
	{
		public UnlockableItem Decoration => this.Unlockable;

		public override int Price
		{
			get => base.Price;
			set
			{
				base.Price = value;
				Decoration.shopSelectionNode.itemCost = value;
			}
		}

		public bool InRotation =>
			ContentManager.Terminal.ShipDecorSelection.Contains(Nodes.Node)
			|| ContentManager.Terminal.ShipDecorSelection.Any(node => node.name == Nodes.Node.name);

		public BuyableDecoration(Terminal terminal, RelatedNodes nodes)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Decoration;

			UnlockableItem decor = StartOfRound.Instance.unlockablesList.unlockables[Nodes.Node.shipUnlockableID];

			Unlockable = decor;
			Price = Decoration.shopSelectionNode.itemCost;
			Name = Decoration.unlockableName;
		}
	}
}
