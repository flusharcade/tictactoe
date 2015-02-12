
namespace TicTacToeLab
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
		public static void Foreach<T>(this IEnumerable<T> list, Action<T> action)
		{
			foreach (var item in list)
			{
				action(item);
			}
		}
    }
}
