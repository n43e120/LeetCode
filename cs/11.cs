
using static ConsoleApp1.TestHelper;
public class Solution
{
	public int MaxArea(int[] height)
	{
		int len = height.Length;
		int max = 0;
		int i = 0;

		var l = height[i];
		int j = len - 1;
		var r = height[j];
		var h = Math.Min(l, r); //highest water height
		var a = h * (j - i);
		if (a > max) { max = a; }
		if (r < l) //move right boundary
		{
			goto NEXT_RIGHT;
		}
	NEXT_LEFT:
		i++;
		if (j == i) goto END;

		l = height[i];
		if (l <= h) //the pole is submerged, useless
		{
			//height[i] = 0;
			goto NEXT_LEFT;
		}
		h = Math.Min(l, r); //new height
		a = h * (j - i);
		if (a > max) { max = a; }
		if (h >= r)
		{
			goto NEXT_RIGHT;
		}
		goto NEXT_LEFT;
	NEXT_RIGHT:
		j--;
		if (j == i) goto END;

		r = height[j];
		if (r <= h) //the pole is submerged, useless
		{
			//height[j] = 0;
			goto NEXT_RIGHT;
		}
		h = Math.Min(l, r); //new height
		a = h * (j - i);
		if (a > max) { max = a; }
		if (h >= l)
		{
			goto NEXT_LEFT;
		}
		goto NEXT_RIGHT;
	END:
		return max;
	}
}

internal static class Program
{
	static void T(int[] input, int expect)
	{
		Print(input);
		Print("->");
		Print(expect);

		Solution x = new();
		var f = x.MaxArea;
		var actual = f(input);
		if (expect == actual)
		{
			COLOR_PRINT(ConsoleColor.Green, () => { Print("ok"); });
		}
		else
		{
			Print(" != ");
			COLOR_PRINT(ConsoleColor.Red, () =>
			{
				Print(actual);
			});
		}
		Console.WriteLine();
	}
	static void Main(string[] args)
	{
		T([1, 1], 1);
		T([1, 8, 6, 2, 5, 4, 8, 3, 7], 49);
	}
}