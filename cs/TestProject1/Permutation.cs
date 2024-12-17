namespace Combinatorics;
public static class Permutation
{
	// arr: [1,2,3], [1,3,2], [2,1,3], [2,3,1], [3,1,2], [3,2,1].
	static int NextHeadIndex(Span<int> A, int target)
	{
		//never return -1
		//target == new head >= old head+1
		int a = 0;
		int b = A.Length - 1;
		int i;
		do
		{
			switch (b - a)
			{
				case 1: //two remaining
					if (A[a] >= target) return a;
					goto case 0;
				case 0:
					if (A[b] >= target) return b;
					if (b + 1 < A.Length) return b + 1;
					throw new Exception("Should always exist but Not Found");
				case -1:
					throw new Exception("len = 0");
			}
			i = (b + a) >> 1;
			int e = A[i];
			if (e < target)
			{
				a = i + 1;
			}
			else //if (e >= target)
			{
				b = i - 1;
			}
		} while (true);
	}
	static void Swap(ref int a, ref int b) => (b, a) = (a, b);
	public static bool Next(Span<int> A, int max)
	{
		int cnt = A.Length;
		switch (cnt)
		{
			case < 2:
				return false;
		}
		int a, b, i;
		b = cnt - 1;
		a = b - 1;
		if (A[a] < A[b]) //...12->...21
		{
			Swap(ref A[a], ref A[b]);
			return true;
		}
	AGAIN:
		b = a - 1;
		if (b < 0) //a=0, reach lbound
		{

			if (A[a] == max) return false;//already last permu 321
			i = a;
			goto CARRY;
		}
		if (A[b] < A[a])//24
		{
			i = b;
			goto CARRY;
		}
		a = b - 1;
		if (a < 0) //reach lbound
		{
			if (A[b] == max) return false;//last permu
			i = b;
			goto CARRY;
		}
		if (A[a] < A[b])//24
		{
			i = a;
			goto CARRY;
		}
		goto AGAIN;
	CARRY:
		var tail = A.Slice(i + 1);
		{//reset A.Slice(i+1) in order

			//A.Slice(i+1).Sort();
			//var l = i + 1;
			//var r = cnt - 1;
			//while (l < r)
			//{
			//	Swap(ref A[l], ref A[r]);
			//	l++;
			//	r--;
			//}
			tail.Reverse();//shortcut, A already in reverse order
		}
		{//change to next head
			int iNewHead = NextHeadIndex(tail, A[i] + 1);
			Swap(ref tail[iNewHead], ref A[i]);
		}
		return true;
	}
	public static int[] First(int cnt)
	{
		int[] A = new int[cnt];
		for (int i = 0; i < cnt; i++)
		{
			A[i] = i + 1;
		}
		return A;
	}
	public static IEnumerable<int[]> All(int cnt)
	{
		int[] nums = First(cnt);
		var max = cnt;
		//var A = nums.AsSpan();
		yield return nums;
		while (Next(nums, max))
		{
			yield return nums;
		};
	}
	public static IEnumerable<int[]> All(int[] nums)
	{
		var max = nums.Max();
		//var A = nums.AsSpan();
		yield return nums;
		while (Next(nums, max))
		{
			yield return nums;
		};
	}
	public static int[] FromNum(UInt64 index, int cnt)
	{
		throw new NotImplementedException();
	}
	public static UInt64 ToNum(int[] p)
	{
		throw new NotImplementedException();
	}
}