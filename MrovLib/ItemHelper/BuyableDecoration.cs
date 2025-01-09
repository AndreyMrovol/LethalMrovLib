namespace MrovLib.ItemHelper
{
	public class BuyableDecoration : BuyableThing
	{
		public UnlockableItem Decoration;
		public bool InRotation => ContentManager.Terminal.ShipDecorSelection.Contains(Nodes.Node);

		public BuyableDecoration(Terminal terminal, RelatedNodes nodes)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Decoration;

			UnlockableItem decor = StartOfRound.Instance.unlockablesList.unlockables[Nodes.Node.shipUnlockableID];

			Decoration = decor;
			Price = Decoration.shopSelectionNode.itemCost;
			Name = Decoration.unlockableName;
		}
	}
}
