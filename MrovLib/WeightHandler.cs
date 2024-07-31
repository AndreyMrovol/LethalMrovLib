using System;
using System.Collections.Generic;
using System.Linq;

namespace MrovLib
{
	// this was HEAVILY inspired by whitespike's suggestion for my shitty weathertweaks code
	// it's a complete overkill lmao

	public class WeightHandler<T>
	{
		private Dictionary<T, int> dictionary = [];

		public void Add(T key, int value)
		{
			// make sure the key is not null
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			// make sure the value is not negative
			if (Comparer<int>.Default.Compare(value, default(int)) < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be negative");
			}

			// if the key is already added, use the bigger value
			if (dictionary.TryGetValue(key, out int existingValue))
			{
				if (Comparer<int>.Default.Compare(value, existingValue) > 0)
				{
					dictionary[key] = value;
				}
			}
			// if the key is not added, add it
			else
			{
				dictionary.Add(key, value);
			}
		}

		public void Set(T key, int value)
		{
			// make sure the key is not null
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			// make sure the value is not negative
			if (Comparer<int>.Default.Compare(value, default(int)) < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be negative");
			}

			// if the key is already added, set the value
			if (dictionary.TryGetValue(key, out int existingValue))
			{
				dictionary[key] = value;
			}
			// if the key is not added, add it
			else
			{
				dictionary.Add(key, value);
			}
		}

		public void Remove(T key)
		{
			// make sure the key is not null
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			dictionary.Remove(key);
		}

		public int Get(T key)
		{
			// make sure the key is not null
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			// if the key is not added, return the default value
			if (!dictionary.TryGetValue(key, out int value))
			{
				return default;
			}

			return value;
		}

		public int Count
		{
			get
			{
				int count = dictionary.Count;

				if (count == 0)
				{
					throw new InvalidOperationException("Dictionary is empty - nothing to pick from!");
				}

				return count;
			}
		}

		public int Sum
		{
			get
			{
				int sum = default;
				foreach (int dictionaryValue in dictionary.Values)
				{
					sum += dictionaryValue;
				}

				// make sure the sum is not 0
				if (sum <= 0)
				{
					// if there's only one item, make sure it's picked; otherwise throw
					if (Count == 1)
					{
						return 1;
					}

					throw new InvalidOperationException("Sum cannot be 0 or negative");
				}

				return sum;
			}
		}

		public int RandomIndex()
		{
			Random random = new();
			int randomIndex = random.Next(0, Sum);
			return randomIndex;
		}

		public T Random()
		{
			int roll = RandomIndex();
			int sum = 0;
			foreach (KeyValuePair<T, int> pair in dictionary.OrderByDescending(v => v.Value))
			{
				sum += pair.Value;
				if (roll <= sum)
				{
					return pair.Key;
				}
			}
			return dictionary.Keys.FirstOrDefault();
		}
	}
}
