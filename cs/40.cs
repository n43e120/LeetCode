public class Solution
{
	bool IsSetEqual(List<int> list1, List<int> list2)
	{
		if (list1.Count != list2.Count) return false;
		for (int i = 0; i < list1.Count; i++)
		{
			if (list1[i] != list2[i])
			{
				return false;
			}
		}
		return true;
	}
	List<IList<int>> RemoveDuplicatElement(List<IList<int>> lists)
	{
		List<IList<int>> r = [];
		foreach (IList<int> list1 in lists)
		{
			for (int i = 0; i < r.Count; i++)
			{
				var list2 = r[i];
				if (IsSetEqual((List<int>)list1, (List<int>)list2))
				{
					goto NEXT;
				}
			}
			r.Add(list1);
		NEXT:
			continue;
		}
		return r;
	}
	Dictionary<int, int> CountBy(List<int> chips)
	{
		List<int> elements = [];
		List<int> counts = [];
		var dict = new Dictionary<int, int>();
		foreach (var item in chips)
		{
			if (dict.ContainsKey(item))
			{
				dict[item] += 1;
			}
			else
			{
				dict.Add(item, 1);
			}
		}
		return dict;
	}
	IList<IList<int>> CombinationSum2_small(List<int> chips, int target)
	{
		List<IList<int>> r = [];
		Dictionary<int, int> dict = CountBy(chips);
		var candidates = dict.Keys.ToArray();
		var len_abacus = candidates.Length;
		if (len_abacus == 0) return r;
		var abacusLimiter = dict.Values.ToArray(); //counters
		var abacus = new int[len_abacus]; //counters
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
		int i = 0;
		var sum = 0;
	AGAIN:
		abacus[i] += 1; sum += candidates[i];
		if (abacus[i] <= abacusLimiter[i])
		{
			if (sum == target) { Save(); }
			goto AGAIN;
		}
	CARRY:
		sum -= candidates[i] * abacus[i]; abacus[i] = 0;
		i++;
		if (i >= len_abacus)
		{
			goto END;
		}
		abacus[i] += 1; sum += candidates[i];
		if (abacus[i] > abacusLimiter[i])
		{
			goto CARRY;
		}
		switch (target - sum)
		{
			case 0:
				Save();
				break;
		}
		i = 0;
		goto AGAIN;
	END:
		return RemoveDuplicatElement(r);
	}
	IList<IList<int>> CombinationSum2_inner(List<int> chips, int target)
	{
		List<IList<int>> r = [];
		var cntSameValueChip = chips.RemoveAll(x => x == target);
		if (cntSameValueChip > 0)
		{
			r.Add([target]);
		}

		foreach (var subgroup in chips.GroupBy(c => c > target / 2))
		{
			if (subgroup.Key)
			{
				foreach (var item in subgroup.Distinct()) //larger chips
				{
					var subtarget = target - item;
					var complement = chips.FindAll(x => x <= subtarget);
					if (complement.Count > 0)
					{
						var subcom = CombinationSum2_inner(complement, subtarget);
						foreach (var subitem in subcom)
						{
							subitem.Add(item);
							r.Add(subitem.ToList());
						}
					}
				}
			}
			else
			{
				var smallchips = subgroup.ToList();
				if (smallchips.Count > 0)
				{
					r.AddRange(CombinationSum2_small(smallchips, target));
				}
			}
		}
		return r;
	}
	public IList<IList<int>> CombinationSum2(int[] candidates, int target)
	{
		//how many unique way to make change
		List<IList<int>> r = [];
		if (target <= 0) return r;
		var chips = candidates.ToList().FindAll(p => p <= target);
		chips.Sort();
		return CombinationSum2_inner(chips, target);
	}
}
internal static class Program
{
	static void Main(string[] args)
	{
		Solution x = new();
		var f = x.CombinationSum2;
		//T(f, [10, 1, 2, 7, 6, 1, 5], 8, [[1, 1, 6], [1, 2, 5], [1, 7], [2, 6]]);
		//T(f,
		//t1: [2, 5, 2, 1, 2],
		//t2: 5,
		//expect: [[1, 2, 2], [5]]);

		//T(f,
		//t1: [14, 6, 25, 9, 30, 20, 33, 34, 28, 30, 16, 12, 31, 9, 9, 12, 34, 16, 25, 32, 8, 7, 30, 12, 33, 20, 21, 29, 24, 17, 27, 34, 11, 17, 30, 6, 32, 21, 27, 17, 16, 8, 24, 12, 12, 28, 11, 33, 10, 32, 22, 13, 34, 18, 12],
		//t2: 27,
		//expect: [[27], [13, 14], [11, 16], [10, 17], [9, 18], [7, 20], [6, 21], [6, 7, 14], [6, 6, 7, 8]]
		//); //Time Limit Exceeded

		T(f,
		t1: [4, 4, 2, 1, 4, 2, 2, 1, 3],
		t2: 6,
		expect: [[1, 1, 2, 2], [1, 1, 4], [1, 2, 3], [2, 2, 2], [2, 4]]
		);

	}
}