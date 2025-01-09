namespace MrovLib.ContentType
{
	public class BuyableUnlockable : BuyableThing
	{
		public UnlockableItem Unlockable;

		public BuyableUnlockable(Terminal terminal, RelatedNodes nodes)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Unlockable;

			Unlockable = StartOfRound.Instance.unlockablesList.unlockables[Nodes.Node.shipUnlockableID];

			int price = 0;

			if (Nodes.Node != null)
			{
				// price = Nodes.Node.itemCost;

				if (Nodes.Node.itemCost <= 0)
				{
					if (false)
					{
						// Plugin.debugLogger.LogWarning(
						// 	$"Unlockable {Unlockable.unlockableName} has an upgrade price of {Variables.upgrades[Unlockable.unlockableName]}"
						// );
						// price = Variables.upgrades[Unlockable.unlockableName];
					}
					else
					{
						Plugin.DebugLogger.LogDebug($"Unlockable {Unlockable.unlockableName} does not have an upgrade price");
						price = Nodes.Node.itemCost;
					}
				}
				else
				{
					price = Nodes.Node.itemCost;
				}
			}

			Price = price;
			Name = Unlockable.unlockableName;
		}
	}
}
