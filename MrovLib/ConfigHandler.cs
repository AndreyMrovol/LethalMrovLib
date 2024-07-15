using BepInEx.Configuration;

namespace MrovLib
{
	public abstract class ConfigHandler<T, CT>
	{
		public virtual ConfigEntry<CT> ConfigEntry { get; set; }

		public virtual CT DefaultValue { get; set; }

		public abstract T Value { get; }
	}
}
