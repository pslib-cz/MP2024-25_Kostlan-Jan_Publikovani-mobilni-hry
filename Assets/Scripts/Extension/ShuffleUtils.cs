using System;
using System.Collections.Generic;
namespace Assets.Scripts.Extension
{
	public static class ShuffleUtils
	{
		private static Random rng = new Random();

		/// <summary>
		/// Zamíchá seznam pomocí Fisher-Yates algoritmu.
		/// </summary>
		public static void Shuffle<T>(this List<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);

				// Swap
				T temp = list[k];
				list[k] = list[n];
				list[n] = temp;
			}
		}
	}
}
