namespace MrovLib.ContentType
{
	public class BuyableItem : BuyableThing
	{
		public Item Item;

		private int PercentOff
		{
			get { return ContentManager.Terminal.itemSalesPercentages[Nodes.Node.buyItemIndex]; }
			set { ContentManager.Terminal.itemSalesPercentages[Nodes.Node.buyItemIndex] = value; }
		}
		public int Discount => PercentOff != 100 ? (100 - PercentOff) : 0;
		public float DiscountPercentage => PercentOff / 100f;

		public BuyableItem(Terminal terminal, RelatedNodes nodes)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Item;

			Item = terminal.buyableItemsList[nodes.Node.buyItemIndex];
			Price = Item.creditsWorth;
			Name = Item.itemName;
		}
	}
}
