// https://github.com/IAmBatby/LethalLevelLoader/blob/51f9af254c38f926f808f1714bb6dc52bb5f66dc/LethalLevelLoader/General/EventPatches.cs#L287-L307
namespace MrovLib.Events
{
	public class CustomEvent<T>
	{
		public delegate void ParameterEvent(T param);
		private event ParameterEvent onParameterEvent;
		public bool HasListeners => Listeners != 0;
		public int Listeners { get; internal set; }

		public void Invoke(T param)
		{
			onParameterEvent?.Invoke(param);
		}

		public void AddListener(ParameterEvent listener)
		{
			onParameterEvent += listener;
			Listeners++;
		}

		public void RemoveListener(ParameterEvent listener)
		{
			onParameterEvent -= listener;
			Listeners--;
		}
	}

	public class CustomEvent
	{
		public delegate void Event();
		private event Event onEvent;
		public bool HasListeners => Listeners != 0;
		public int Listeners { get; internal set; }

		public void Invoke()
		{
			onEvent?.Invoke();
		}

		public void AddListener(Event listener)
		{
			onEvent += listener;
			Listeners++;
		}

		public void RemoveListener(Event listener)
		{
			onEvent -= listener;
			Listeners--;
		}
	}
}
