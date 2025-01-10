using UnityEngine;

namespace MrovLib.ContentType
{
	public class BuyableSuit : BuyableThing
	{
		public UnlockableItem Suit;
		public Material SuitMaterial;
		public bool IsUnlocked => Suit.hasBeenUnlockedByPlayer || Suit.alreadyUnlocked;
		public bool InRotation => ContentManager.Terminal.ShipDecorSelection.Contains(Nodes.Node);

		public BuyableSuit(Terminal terminal, RelatedNodes nodes, UnlockableItem unlockable)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Suit;

			Suit = unlockable;
			SuitMaterial = Suit.suitMaterial;

			Price = Suit.shopSelectionNode == null ? 0 : Suit.shopSelectionNode.itemCost;
			Name = Suit.unlockableName ?? Suit.suitMaterial.name;
		}
	}
}
