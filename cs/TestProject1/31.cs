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