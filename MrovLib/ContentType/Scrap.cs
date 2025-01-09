namespace MrovLib.ContentType
{
	public class Scrap
	{
		public Item Item { get; set; }

		public string Name => Item.itemName;

		public float Weight => Item.weight;
		public bool Conductive => Item.isConductiveMetal;

		public int ValueMin => Item.minValue;
		public int ValueMax => Item.maxValue;

		public bool HasBattery => Item.requiresBattery;

		public Scrap(Item item)
		{
			Plugin.DebugLogger.LogWarning($"Scrap constructor: {item.itemName}");

			Item = item;

			ContentManager.Scraps.Add(this);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
