
public static class Permutation
{
	// arr: [1,2,3], [1,3,2], [2,1,3], [2,3,1], [3,1,2], [3,2,1].
	static int BinarySearchForNextHead(Span<int> A, int target)
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
	public static bool Next(int[] A)
	{
		int cnt = A.Length;
		switch (cnt)
		{
			case 0:
			case 1:
				return false;
		}
		int a, b, i;
		b = cnt - 1;
		a = b - 1;
		if (A[a] < A[b]) //12->21
		{
			Swap(ref A[a], ref A[b]);
			return true;
		}
		var max = A.Max();
	AGAIN:
		b = a - 1;
		if (b < 0) //reach left end, 1 or 321 or 231 -> 312
		{

			if (A[a] == max) return false;//reach last permu 321 or 21
			i = a;
			goto ALTERNATE_HEAD;
		}
		if (A[b] < A[a])//24
		{
			i = b;
			goto ALTERNATE_HEAD;
		}
		a = b - 1;
		if (a < 0) //reach left end, 1 or 321 or 231 -> 312
		{
			if (A[b] == max) return false;//reach last permu 321 or 21
			i = b;
			goto ALTERNATE_HEAD;
		}
		if (A[a] < A[b])//24
		{
			i = a;
			goto ALTERNATE_HEAD;
		}
		goto AGAIN;
	ALTERNATE_HEAD:
		//A.Slice(i+1).Sort(); or A.Slice(i+1).Reverse();
		var l = i + 1;
		var r = cnt - 1;
		while (l < r)
		{
			Swap(ref A[l], ref A[r]);
			l++;
			r--;
		}
		int iNewHead = BinarySearchForNextHead(A.AsSpan().Slice(i + 1), A[i] + 1);
		Swap(ref A[i + 1 + iNewHead], ref A[i]);
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
		int[] A = First(cnt);
		yield return A;
		while (Next(A))
		{
			yield return A;
		};
	}
	public static IEnumerable<int[]> All(int[] num)
	{
		int[] A = num;
		yield return A;
		while (Next(A))
		{
			yield return A;
		};
	}
	public static int[] FromNum(UInt64 index, int cnt)
	{
		return new int[index];
	}
	public static UInt64 ToNum(int[] p)
	{
		return 0;
	}
}