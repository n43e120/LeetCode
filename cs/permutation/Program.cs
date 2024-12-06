using static ConsoleApp1.TestHelper;
public class Solution
{
	public void NextPermutation(int[] nums)//leetcode 31
	{
		var b = Permutation.Next(nums);
		if (!b)
		{
			nums.AsSpan().Sort();
		}
	}
	public IList<IList<int>> Permute(int[] nums)//leetcode 46
	{
		List<IList<int>> list = new();
		var x = nums.ToList();
		x.Sort();
		foreach (var A in Permutation.All(x.ToArray()))
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
internal static class Program
{
	static void T(int[] input, int[] expect)
	{
		PRINT_LIST(input);
		Print("->");
		PRINT_LIST(expect);
		Solution x = new();
		var f = x.NextPermutation;
		var actual = input.Clone() as int[];
		f(actual);
		if (IsEqualSeq(expect, actual))
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