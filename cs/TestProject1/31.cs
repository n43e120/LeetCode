using System.Text;

file class Solution
{
	public void NextPermutation(int[] nums)//leetcode 31
	{
		var max = nums.Max();
		var A = nums.AsSpan();
		if (!Combinatorics.Permutation.Next(A, max))
		{
			A.Sort();
		}
	}
	public IList<IList<int>> Permute(int[] nums)//leetcode 46
	{
		List<IList<int>> list = new();
		var x = nums.ToList();
		x.Sort();
		foreach (var A in Combinatorics.Permutation.All(x.ToArray()))
		{
			list.Add((IList<int>)A.Clone());
		}
		return list;
	}
	public IList<IList<int>> PermuteUnique(int[] nums)//leetcode 47
	{
		return Permute(nums);
	}
	public string GetPermutation(int n, int k)//leetcode 60
	{
		var A = Combinatorics.Permutation.FromNum((ulong)k - 1, n);
		var sb = new StringBuilder(n);
		foreach (var i in A)
		{
			sb.Append((char)('0' + i));
		}
		return sb.ToString();
	}
	public IList<IList<int>> Combine(int n, int k)//leetcode 77
	{
		List<IList<int>> list = new();
		foreach (var A in Combinatorics.Combination.All(k: k, n: n))
		{
			list.Add((IList<int>)A.Clone());
		}
		return list;
	}
}
[TestClass]
public sealed class Test31
{
	void T(int[] input, int[] expect)
	{
		Solution x = new();
		var f = x.NextPermutation;
		var a = (int[])input.Clone();
		f(a);
		ConsoleApp1.TestHelper.T(a, expect);
	}
	[TestMethod]
	public void TestMethod1()
	{
		T([1], [1]);

		T([0, 1], [1, 0]);
		T([1, 0], [0, 1]);

		T([1, 2, 3], [1, 3, 2]);
		T([1, 3, 2], [2, 1, 3]);
		T([2, 1, 3], [2, 3, 1]);
		T([2, 3, 1], [3, 1, 2]);
		T([3, 2, 1], [1, 2, 3]);

		T([0, 1, 1, 0, 4, 4], [0, 1, 1, 4, 0, 4]);
		T([2, 1, 2, 2, 2, 2, 2, 1], [2, 2, 1, 1, 2, 2, 2, 2]);

		T([1, 1, 2], [1, 2, 1]);
		T([1, 2, 1], [2, 1, 1]);
		T([2, 1, 1], [1, 1, 2]);

		//[[1, 1, 2, 2], [1, 2, 1, 2], [1, 2, 2, 1], [2, 1, 1, 2], [2, 1, 2, 1], [2, 2, 1, 1]]
		T([1, 1, 2, 2], [1, 2, 1, 2]);
		T([1, 2, 1, 2], [1, 2, 2, 1]);
		T([1, 2, 2, 1], [2, 1, 1, 2]);
		T([2, 1, 1, 2], [2, 1, 2, 1]);
		T([2, 1, 2, 1], [2, 2, 1, 1]);
		T([2, 2, 1, 1], [1, 1, 2, 2]);

		//int i = 0;
		//foreach (var A in Permutation.All([1, 1, 2, 2]))
		//{
		//	PRINT_LIST(A);
		//	Console.WriteLine();
		//	i++;
		//	if (i >= 6) break;
		//}
	}
}
[TestClass]
public sealed class Test60
{
	[TestMethod]
	public void TestMethod1()
	{
		Solution s = new Solution();
		var f = s.GetPermutation;
		T(f(3, 3), "213");
	}
}
[TestClass]
public sealed class Test77
{
	[TestMethod]
	public void TestMethod1()
	{
		Solution s = new Solution();
		var f = s.Combine;
		T(f(4, 2), [[1, 2], [1, 3], [1, 4], [2, 3], [2, 4], [3, 4]]);
	}
}