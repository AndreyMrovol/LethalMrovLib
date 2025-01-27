using System.Linq;
using UnityEngine;

namespace MrovLib.ContentType
{
	public class BuyableSuit : BuyableThing
	{
		public UnlockableItem Suit;
		public Material SuitMaterial;

		public bool IsUnlocked => Suit.hasBeenUnlockedByPlayer || Suit.alreadyUnlocked;
		public bool InRotation
		{
			get
			{
				// i have to do this shit because [orange suit] has no terminal nodes
				if (Nodes.Node == null)
				{
					return false;
				}

				return ContentManager.Terminal.ShipDecorSelection.Contains(Nodes.Node)
					|| ContentManager.Terminal.ShipDecorSelection.Any(node => node.name == Nodes.Node.name);
			}
		}

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
