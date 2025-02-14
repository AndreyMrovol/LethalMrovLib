using System.Collections.Generic;
using System.Linq;

namespace MrovLib.Definitions
{
	public class ContentTypeResolver<ResolveType>
	{
		internal static Dictionary<string, ResolveType> _dictionary = [];
		internal static ResolverCache<ResolveType[]> _cache = new();

		public Dictionary<string, ResolveType> StringToType
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary;
				}

				_dictionary = CreateTypeDictionary();
				return _dictionary;
			}
			set { _dictionary = value; }
		}

		public virtual Dictionary<string, ResolveType> CreateTypeDictionary()
		{
			return [];
		}

		public virtual ResolveType[] Resolve(string input)
		{
			if (_cache.Contains(input))
			{
				return _cache.Get(input);
			}

			List<ResolveType> output = [];
			List<ResolveType> remove = [];

			string[] names = MrovLib.StringResolver.ConvertStringToArray(input);

			foreach (string name in names)
			{
				if (name.StartsWith("!"))
				{
					Plugin.LogDebug($"String {name} will be removed from final consideration!");

					// recursive pass string without the !
					remove.AddRange(Resolve(name.Substring(1)));
				}

				ResolveType resolved = _dictionary.GetValueOrDefault(name.ToLowerInvariant());

				if (name == null)
				{
					continue;
				}

				Plugin.LogDebug($"String {name} resolved to: {resolved}");

				if (output.Contains(resolved))
				{
					continue;
				}

				output.Add(resolved);
			}

			return output.ToArray();
		}

		public virtual void Reset()
		{
			_dictionary.Clear();
			_cache.Reset();
		}
	}
}
