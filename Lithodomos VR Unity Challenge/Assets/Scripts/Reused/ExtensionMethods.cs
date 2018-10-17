using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{

	public static int Loop(this int value, int increment, int max)
	{
		value += increment;
		if (value >= max)
			return 0;
		if (value < 0)
			return max - 1;
		return value;
	}
}
