file class Solution
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
[TestClass]
public sealed class Test11
{
	[TestMethod]
	public void TestMethod1()
	{
		Solution x = new();
		var f = x.MaxArea;
		T(f([1, 1]), 1);
		T(f([1, 8, 6, 2, 5, 4, 8, 3, 7]), 49);
	}
}