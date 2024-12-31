using System;

namespace Combinatorics;
public static partial class MyMath
{
	public static ulong Factorial(int n)
	{
		ulong f = 1;
		for (int i = 2; i <= n; i++)
		{
			f *= (ulong)i;
		}
		return f;
	}
	public static ulong Binominal(int k, int n)
	{
		ulong r = 1;
		k = System.Math.Max(k, n - k);
		for (int i = k + 1; i <= n; i++)
		{
			r *= (ulong)i;
		}
		return r / Factorial(n - k);
	}
}
public static class Combination
{
	public static IEnumerable<int[]> All(int k, int n)
	{
		int[] A = new int[k];
		int i = 0;
		for (; i < k; i++)
		{
			A[i] = i + 1;
		}
	AGAIN:
		yield return A;
		i--;
		while (A[i] < n)
		{
			A[i]++;
			yield return A;
		}
	CARRY:
		i--;
		if (i < 0)
		{
			goto END;
		}
		if (A[i] < (n - (k - 1 - i)))
		{
			A[i]++;
			for (i++; i < k; i++)
			{
				A[i] = A[i - 1] + 1;
			}
			goto AGAIN;
		}
		goto CARRY;
	END:;
	}
}