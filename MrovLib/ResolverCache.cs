using System.Collections.Generic;

namespace MrovLib
{
	public class ResolverCache<T>
	{
		private Dictionary<string, T> _cache = new Dictionary<string, T>();

		public void Add(string key, T value)
		{
			if (key == null)
			{
				throw new System.ArgumentNullException(nameof(key));
			}
			if (_cache.ContainsKey(key))
			{
				_cache[key] = value;
			}
			else
			{
				_cache.Add(key, value);
			}
		}

		public T Get(string key)
		{
			if (_cache.TryGetValue(key, out T value))
			{
				return value;
			}
			return default(T);
		}

		public bool Contains(string key)
		{
			return _cache.ContainsKey(key);
		}

		public void Reset()
		{
			_cache.Clear();
		}
	}
}
