namespace MrovLib.ContentType
{
	public class BuyableCar : BuyableThing
	{
		public BuyableVehicle Vehicle;

		public BuyableCar(Terminal terminal, RelatedNodes nodes)
			: base(terminal, nodes)
		{
			Type = PurchaseType.Vehicle;

			Vehicle = ContentManager.Terminal.buyableVehicles[nodes.Node.buyVehicleIndex];
			Price = Vehicle.creditsWorth;
			Name = Vehicle.vehicleDisplayName;

			ContentManager.Vehicles.Add(this);
		}
	}
}
