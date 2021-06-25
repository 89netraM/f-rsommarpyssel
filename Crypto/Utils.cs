using System.Collections.Generic;
using System.Linq;

namespace Försommarpyssel.Crypto
{
	public static class Utils
	{
		// Adapted from https://stackoverflow.com/questions/11208446/generating-permutations-of-a-set-most-efficiently/36634935#36634935
		public static IEnumerable<List<T>> AllPermutation<T>(List<T> items)
		{
			int countOfItem = items.Count;

			if (countOfItem <= 1)
			{
				yield break;
			}

			yield return items.ToList();

			int[] indexes = new int[countOfItem];

			for (int i = 1; i < countOfItem;)
			{
				if (indexes[i] < i)
				{
					if ((i & 1) == 1)
					{
						(items[i], items[indexes[i]]) = (items[indexes[i]], items[i]);
					}
					else
					{
						(items[i], items[0]) = (items[0], items[i]);
					}
					yield return items.ToList();
					indexes[i]++;
					i = 1;
				}
				else
				{
					indexes[i++] = 0;
				}
			}
		}
	}
}
