namespace MrovLib.ContentType
{
	public class BuyableCar : BuyableThing
	{
		public BuyableVehicle Vehicle;

		public override int Price
		{
			get => base.Price;
			set
			{
				base.Price = value;
				Nodes.Node.itemCost = value;
			}
		}

		public BuyableCar(Terminal terminal, RelatedNodes nodes)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Vehicle;

			Vehicle = ContentManager.Terminal.buyableVehicles[nodes.Node.buyVehicleIndex];
			Price = Nodes.Node.itemCost;
			Name = Vehicle.vehicleDisplayName;

			ContentManager.Vehicles.Add(this);
		}
	}
}
