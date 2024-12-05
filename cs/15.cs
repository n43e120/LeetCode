
public class Solution
{
	const int NONEXIST = -1;
	public int binary_search(ReadOnlySpan<int> A, int target)
	{
		int a = 0;
		int b = A.Length - 1;
		int i;
		do
		{
			switch (b - a)
			{
				case 1: //two remaining
					if (A[b] == target) return b;
					goto case 0;
				case 0:
					if (A[a] == target) return a;
					goto case -1;
				case -1:
					return NONEXIST;
			}
			i = (b + a) >> 1;
			int e = A[i];
			if (e < target)
			{
				a = i + 1;
			}
			else if (e > target)
			{
				b = i - 1;
			}
			else
			{
				break;
			}
		} while (true);
		return i;

	}
	/// <summary>
	/// find the smallest a in sorted A, such that A[a]>=target
	/// return i, otherwise -1 if not found
	/// </summary>
	public int find_a(ReadOnlySpan<int> A, int target)
	{
		int a = 0;
		int b = A.Length - 1;
		int i;

		switch (b - a)
		{
			case 1: //two remaining
				if (A[a] >= target) return a;
				goto case 0;
			case 0:
				if (A[b] >= target) return b;
				goto case -1;
			case -1:
				return NONEXIST;
		}

		if (A[a] >= target) return a;
		if (A[b] < target) return NONEXIST;
		//target in (a,b]
		a++;
		b--;

		do
		{
			switch (b - a)
			{
				case 1: //two remaining
					if (A[a] >= target) return a;
					goto case 0;
				case 0:
					if (A[b] >= target) return b;
					return b + 1;
			}
			i = (b + a) >> 1;
			int e = A[i];
			if (e < target)
			{
				a = i + 1;
			}
			else // if (e >= target)
			{
				b = i - 1;
			}
		} while (true);
	}
	/// <summary>
	/// find the largest b in sorted A, such that A[b]<= target
	/// return i, otherwise -1 if not found
	/// </summary>
	public int find_b(ReadOnlySpan<int> A, int target)
	{
		int a = 0;
		int b = A.Length - 1;
		int i;

		switch (b - a)
		{
			case 1: //two remaining
				if (A[b] <= target) return b;
				goto case 0;
			case 0:
				if (A[a] <= target) return a;
				goto case -1;
			case -1:
				return NONEXIST;
		}

		if (A[b] <= target) return b;
		if (A[a] > target) return NONEXIST;
		//target in [a,b)
		b--;
		a++;
		do
		{
			switch (b - a)
			{
				case 1: //two remaining
					if (A[b] <= target) return b;
					goto case 0;
				case 0:
					if (A[a] <= target) return a;
					return a - 1;
			}
			i = (b + a) >> 1;
			int e = A[i];
			if (e <= target)
			{
				a = i + 1;
			}
			else //if (e > target)
			{
				b = i - 1;
			}
		} while (true);
	}
	public HashSet<(int, int)> Sum2(ReadOnlySpan<int> A, int target)
	{
		HashSet<(int, int)> S = [];
		int a = 0;
		int b = A.Length - 1;
		do
		{
			if (b - a <= 0)
			{
				goto END;
			}
			var sum = A[a] + A[b];
			switch (sum - target)
			{
				case 0:
					S.Add((A[a], A[b]));
					a++;
					b--;
					break;
				case > 0:
					b--;
					break;
				case < 0:
					a++;
					break;
			}
		} while (true);
	END:
		return S;
	}
	public HashSet<(int, int, int)> Sum3(ReadOnlySpan<int> A, int target)
	{
		HashSet<(int, int, int)> S = [];
		int a = 0;
		int b = A.Length - 1;
		int i;
		int tmp;
		const int FLAG_A = 1;
		const int FLAG_B = 2;
		int f = FLAG_A | FLAG_B;
	AGAIN:
		{
			switch (b - a)
			{
				case < 2://A.Length <3
					goto END;
				case 2://A.Length ==3
					goto ONE_LEFT;
				default://A.Length >= 4
					break;
			}
			if ((f & FLAG_A) != 0)
			{
				i = b - 1;
				tmp = find_a(A.Slice(a, b - a + 1 - 2), target - (A[i] + A[b]));
				if (tmp == NONEXIST) goto END;
				if (tmp != 0)
				{
					a += tmp;
					f = FLAG_B;
					goto AGAIN;
				}
				f &= ~FLAG_A;
			}
			if ((f & FLAG_B) != 0)
			{
				i = a + 1;
				tmp = find_b(A.Slice(a + 2, b - a + 1 - 2), target - (A[i] + A[a]));
				if (tmp == NONEXIST) goto END;
				tmp = a + 2 + tmp;
				if (tmp != b)
				{
					b = tmp;
					f = FLAG_A;
					goto AGAIN;
				}
				f &= ~FLAG_B;
			}
		}
		var o = Sum2(A.Slice(a + 1, b - a + 1 - 1), target - A[a]);
		foreach (var x in o)
		{
			S.Add((A[a], x.Item1, x.Item2));
		}
		a++;
		goto AGAIN;
	ONE_LEFT:
		i = a + 1;
		if (A[a] + A[i] + A[b] == target)
		{
			S.Add((A[a], A[i], A[b]));
		}
	END:
		return S;
	}
	public IList<IList<int>> ThreeSum(int[] nums)
	{
		HashSet<(int, int, int)> S = new();
		int len = nums.Length;
		if (len < 3)
		{
			goto END;
		}
		var A = nums.ToList();
		A.Sort();
		var arr = A.ToArray();
		var span = new ReadOnlySpan<int>(arr);
		S = Sum3(span, 0);
	END:
		{
			var l = S.ToList();
			List<IList<int>> r = new();
			foreach (var x in l)
			{
				r.Add([x.Item1, x.Item2, x.Item3]);
			}
			return (IList<IList<int>>)r;
		}
	}
	public IList<IList<int>> ThreeSumBruteForce(int[] nums)
	{
		HashSet<(int, int, int)> S = new();
		int len = nums.Length;
		if (len < 3)
		{
			goto END;
		}
		var A = nums.ToList();
		A.Sort();
		int i = len - 1, j = i - 1, k = i - 2;
	AGAIN:
		if (A[i] + A[j] + A[k] == 0)
		{
			S.Add((A[i], A[j], A[k]));
		}
		if (k > 0)
		{
			k--;
			goto AGAIN;
		}
		if (j > 1)
		{
			j--;
			k = j - 1;
			goto AGAIN;
		}
		if (i > 2)
		{
			i--;
			j = i - 1;
			k = j - 1;
			goto AGAIN;
		}
	END:
		{
			var l = S.ToList();
			List<IList<int>> r = new();
			foreach (var x in l)
			{
				r.Add([x.Item1, x.Item2, x.Item3]);
			}
			return (IList<IList<int>>)r;
		}
	}
}

internal static class Program
{
	static void COLOR_PRINT(ConsoleColor color, Action f)
	{
		Console.ForegroundColor = color;
		f();
		Console.ResetColor();
	}
	static void PRINT_LIST(IList<int> list)
	{
		Console.Write("[");
		int i = 0;
		for (; i < list.Count - 1; i++)
		{
			Console.Write(list[i]);
			Console.Write(',');
		}
		if (i < list.Count)
		{
			Console.Write(list[i]);
		}
		Console.Write("]");
	}
	static void PRINT_LISTLIST(IList<IList<int>> ll)
	{
		Console.Write("[");
		int i = 0;
		for (; i < ll.Count - 1; i++)
		{
			PRINT_LIST(ll[i]);
			Console.Write(',');
		}
		if (i < ll.Count)
		{
			PRINT_LIST(ll[i]);
		}
		Console.Write("]");
	}
	static bool IsEqual(IList<int> A, IList<int> B)
	{
		if (A.Count != B.Count)
		{
			return false;
		}
		var la = A.ToList();
		la.Sort();
		var lb = B.ToList();
		lb.Sort();
		for (int j = 0; j < 3; j++)
		{
			if (la[j] != lb[j])
			{
				return false;
			}
		}
		return true;
	}
	static bool IsEqual(IList<IList<int>> A, IList<IList<int>> B)
	{
		if (A.Count != B.Count)
		{
			return false;
		}
		for (int i = 0; i < A.Count; i++)
		{
			var a = A[i];
			var b = B[i];
			if (!IsEqual(a, b))
			{
				return false;
			}
		}
		return true;
	}
	static void T(int[] input, IList<IList<int>> expect)
	{
		PRINT_LIST(input);
		Console.Write("->");
		PRINT_LISTLIST(expect);

		Solution x = new();
		var f = x.ThreeSum;
		var actual = f(input);
		if (IsEqual(expect, actual))
		{
			COLOR_PRINT(ConsoleColor.Green, () => { Console.Write("ok"); });
		}
		else
		{
			Console.Write(" != ");
			COLOR_PRINT(ConsoleColor.Red, () =>
			{
				PRINT_LISTLIST(actual);
			});
		}
		Console.WriteLine();
	}
	static void Main(string[] args)
	{
		T([-1, 0, 1, 2, -1, -4], [[-1, -1, 2], [-1, 0, 1]]);
		T([0, 1, 1], []);
		T([0, 0, 0], [[0, 0, 0]]);
	}
}