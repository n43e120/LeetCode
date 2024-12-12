public class Solution
{
	public IList<IList<int>> CombinationSum(int[] candidates, int target)
	{
		//how many way to make change
		List<IList<int>> r = [];
		if (target == 0) goto END;
		var len_abacus = candidates.Length;
		if (len_abacus == 0) goto END;
		unsafe
		{
			var abacus = new int[len_abacus]; //counters
			var sum = 0;
			int i = 0;
			var firstchip = candidates[i];
			void Save()
			{
				var l = new List<int>();
				for (int i = 0; i < len_abacus; i++)
				{
					var chip = candidates[i];
					for (int j = 0; j < abacus[i]; j++)
					{
						l.Add(chip);
					}
				}
				r.Add(l);
			}
		AGAIN:
			var diff = target - sum;
			(var q, var rem) = Math.DivRem(diff, firstchip);
			if (rem == 0)
			{
				abacus[0] = q;
				Save();
			}
		CARRY:
			abacus[i] = 0;
			i++;
			if (i >= len_abacus)
			{
				goto END;
			}
			abacus[i]++;
			sum += candidates[i];
			switch (sum - target)
			{
				case < 0:
					i = 0;
					goto AGAIN;
				case > 0:
					break;
				default:
					Save();
					break;
			}
			sum -= candidates[i] * abacus[i];
			goto CARRY;
		}
	END:
		return r;
	}
}
internal static class Program
{
	static void Main(string[] args)
	{
		Solution x = new();
		var f = x.CombinationSum;
		T(f, [2, 3, 6, 7], 7, [[2, 2, 3], [7]]);
		T(f, [2, 3, 5], 8, [[2, 2, 2, 2], [2, 3, 3], [3, 5]]);
	}
}