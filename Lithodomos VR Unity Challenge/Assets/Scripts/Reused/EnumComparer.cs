using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this IEqualityComparer implementation was not written by me.
//I got it from here:
//https://stackoverflow.com/questions/26280788/dictionary-enum-key-performance#
//See answer by Erti-Chris Eelmaa
//It is used to improve performance of dictionaries that use Enums as keys.

struct EnumComparer<T> : IEqualityComparer<T> 
	where T : struct
{
	static class BoxAvoidance
	{
		static readonly System.Func<T, int> _wrapper;

		public static int ToInt(T enu)
		{
			return _wrapper(enu);
		}

		static BoxAvoidance()
		{
			var p = System.Linq.Expressions.Expression.Parameter(typeof(T), null);
			var c = System.Linq.Expressions.Expression.ConvertChecked(p, typeof(int));

			_wrapper = System.Linq.Expressions.Expression.Lambda<System.Func<T, int>>(c, p).Compile();
		}
	}

	public bool Equals(T firstEnum, T secondEnum)
	{
		return BoxAvoidance.ToInt(firstEnum) == 
			BoxAvoidance.ToInt(secondEnum);
	}

	public int GetHashCode(T firstEnum)
	{
		return BoxAvoidance.ToInt(firstEnum);
	}
}