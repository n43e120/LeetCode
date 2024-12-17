namespace Combinatorics;
public static class Derangement
{
	public static int Factorial(int n)
	{
		var f = 1;
		for (int i = 2; i <= n; i++)
		{
			f *= i;
		}
		return f;
	}
	public static int Prod(int start, int end)
	{
		var f = 1;
		for (int i = start; i <= end; i++)
		{
			f *= i;
		}
		return f;
	}
	public static int Count(int n)
	{
		//https://oi-wiki.org/math/combinatorics/derangement/
		//n!\sum_{k=0}^{n} \frac{(-1)^k}{k!}
		switch (n)
		{
			case < 2: return 0;
			default:
				break;
		}
		var sum = 0;
		for (int k = 0; k <= n; k++)
		{
			if (int.IsEvenInteger(k))
			{
				sum += Prod(k + 1, n);
			}
			else
			{
				sum -= Prod(k + 1, n);
			}
		}
		return sum;
	}
	//[2,1]
	//[2,3,1], [3,1,2]
	//[2143]
	//public static int[] First(int n)
	//{
	//	switch (n)
	//	{
	//		case < 2: return null;
	//		case 2: return [2, 1];
	//		case 3: return [2, 3, 1];
	//		case 4: return [2, 1, 4, 3];
	//		default:
	//			break;
	//	}
	//	int[] A = new int[n];
	//	for (int i = 0; i < n; i++)
	//	{
	//		A[i] = i + 1;
	//	}
	//	if (int.IsEvenInteger(n))
	//	{
	//		for (int i = 0; i < n; i += 2)
	//		{
	//			Swap(ref A[i], ref A[i + 1]);
	//		}
	//	}
	//	else
	//	{
	//		int i = 0;
	//		for (; i < n - 1; i += 2)
	//		{
	//			Swap(ref A[i], ref A[i + 1]);
	//		}
	//		//now i=n-1 the last idx
	//		Swap(ref A[i], ref A[i - 1]);
	//	}
	//	return A;
	//}
	public static bool IsValid(int[] A)
	{
		if (A is null) return false;
		int n = A.Length;
		switch (n)
		{
			case < 2: return false;
		}
		for (int i = 0; i < n; i++)
		{
			if (A[i] == i + 1) return false;
		}
		return true;
	}
	static void Swap(ref int a, ref int b) => (b, a) = (a, b);
	public static IList<IList<int>> All(int n)
	{
		List<IList<int>> r = [];
		//if (A is null) return false;
		//int n = A.Length;
		switch (n)
		{
			case < 2: return r;
			case 2: return [[2, 1]];
			case 3: return [[2, 3, 1], [3, 1, 2]];
		}
		int[] seat = new int[n - 2]; //remaining available seat sapce
		var A = new int[n]; //current seat status
		int FirstSeat(int i) //pick lowest available seat
		{
			return int.TrailingZeroCount(seat[i] & ~(1 << i));
		}
		int NextSeat(int i)
		{
			return int.TrailingZeroCount(seat[i] & ~(1 << i) & ~((A[i] << 1) - 1));
		}
		void Save()
		{
			var B = new int[n];
			for (int i = 0; i < n; i++)
			{
				B[i] = int.TrailingZeroCount(A[i]) + 1;
			}
			r.Add(B);
		}
		int i = 0;
		seat[0] = (1 << n) - 1;
		A[0] = 1 << 1;
	INIT:
		i++;
		for (; i < n - 2; i++)
		{
			seat[i] = seat[i - 1] ^ A[i - 1];
			A[i] = 1 << FirstSeat(i);
		}
	SWAP_LAST_TWO:
		{
			var seatAB = seat[i - 1] ^ A[i - 1];
			A[i] = 1 << int.TrailingZeroCount(seatAB);
			A[i + 1] = seatAB ^ A[i];
			if (A[i] != 1 << i && A[i + 1] != 1 << i + 1)
			{
				Save();
			}
			Swap(ref A[i], ref A[i + 1]);
			if (A[i] != 1 << i && A[i + 1] != 1 << i + 1)
			{
				Save();
			}
		}
		{
			var nextSeat = NextSeat(i - 1);
			if (nextSeat < n)
			{
				A[i - 1] = 1 << nextSeat;//move to next seat
				goto SWAP_LAST_TWO;
			}
		}
		i -= 2;
		//CARRY:
		do
		{
			var nextSeat = NextSeat(i);
			if (nextSeat < n)
			{
				A[i] = 1 << nextSeat;
				goto INIT;
			}
			i--;
		} while (i >= 0);
		return r;
	}
}
[TestClass]
public sealed class TestDerangement
{
	[TestMethod]
	public void TestMethod1()
	{
		var n = 4;
		//var x = Derangement.All(n);
		//Print<int>(x);
		//Assert.Fail();
		T(Derangement.All(n).Count, Derangement.Count(n));
	}
}