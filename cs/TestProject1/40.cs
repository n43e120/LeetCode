file class Solution
{
	const int NONEXISTENT = -1;
	/// <summary>
	/// find the smallest a in sorted A, such that A[a]>=target
	/// return i, otherwise -1 if not found
	/// </summary>
	public int find_a(ReadOnlySpan<int> A, int target) //copied from 15.cs
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
				return NONEXISTENT;
		}

		if (A[a] >= target) return a;
		if (A[b] < target) return NONEXISTENT;
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

	class AbacusCounter
	{
		internal int[] candidates;
		internal int[] abacusLimiter;
		public AbacusCounter(IList<int> chips)
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
			candidates = dict.Keys.ToArray();
			abacusLimiter = dict.Values.ToArray(); //counters
		}
	}
	void Save(ReadOnlySpan<int> abacus, ReadOnlySpan<int> candidates, IList<IList<int>> r)
	{
		var len_abacus = abacus.Length;
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
	void CombinationSum2_small(AbacusCounter abacus1, int len_abacus, int target, IList<IList<int>> r)
	{
		if (len_abacus == 0) return;
		ReadOnlySpan<int> candidates = abacus1.candidates.AsSpan().Slice(0, len_abacus);
		ReadOnlySpan<int> abacusLimiter = abacus1.abacusLimiter.AsSpan().Slice(0, len_abacus);
		var abacus = new int[len_abacus]; //counters
		int i = 0;
		var sum = 0;
	AGAIN:
		abacus[i] += 1; sum += candidates[i];
		if (abacus[i] <= abacusLimiter[i])
		{
			if (sum == target) { Save(abacus, candidates, r); }
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
				Save(abacus, candidates, r);
				break;
		}
		i = 0;
		goto AGAIN;
	END:
		return;// RemoveDuplicatElement(r);
	}
	void CombinationSum2_inner(AbacusCounter abacus1, int len_abacus, int target, IList<IList<int>> r)
	{
		ReadOnlySpan<int> chips = abacus1.candidates.AsSpan().Slice(0, len_abacus);
		int i = len_abacus - 1;
		while (i >= 0) //chip > target is useless
		{
			if (chips[i] <= target)
			{
				goto NEXT_EQUAL;
			}
			i -= 1;
		};
		return;
	NEXT_EQUAL:
		if (chips[i] == target)
		{
			r.Add([target]);
			i--;
		}
		while (i >= 0)
		{
			if (chips[i] < target)
			{
				goto NEXT_STAGE_M;
			}
			i -= 1;
		};
		return;
	NEXT_STAGE_M:
		chips = chips.Slice(0, i + 1);
		var im = find_a(chips, target / 2 + 1);
		switch (im)
		{
			case 0: //all chips > target/2 can not make change
				return;
			case NONEXISTENT://all chips are small chip, no big chip
				CombinationSum2_small(abacus1, i + 1, target, r);
				return;
			default:
				break;
		}
		ReadOnlySpan<int> chips_small = chips.Slice(0, im);
		CombinationSum2_small(abacus1, im, target, r);

		var chips_big = chips.Slice(im);
		var hsUsed = new HashSet<int>();
		foreach (var item in chips_big) //larger chips
		{
			if (hsUsed.Contains(item))
			{
				continue;
			}
			hsUsed.Add(item);
			var subtarget = target - item;
			var icomplement = find_a(chips_small, subtarget + 1);
			switch (icomplement)
			{
				case 0: //all chips > subtarget too big to make change
					return;
				case NONEXISTENT://all chips are smaller than subtarget
					icomplement = chips_small.Length;
					break;
				default:
					break;
			}
			var complement = chips_small.Slice(0, icomplement);
			List<IList<int>> subr = [];
			CombinationSum2_inner(abacus1, icomplement, subtarget, subr);
			for (int j = 0; j < subr.Count; j++)
			{
				subr[j].Add(item);
				r.Add(subr[j]);
			}
		}
	}
	public IList<IList<int>> CombinationSum2(int[] candidates, int target)
	{
		//how many unique way to make change
		List<IList<int>> r = [];
		if (target <= 0) return r;
		var chips = candidates.ToList().FindAll(x => x <= target);
		chips.Sort();
		var a = new AbacusCounter(chips);
		var len_acabas = a.candidates.Length;
		CombinationSum2_inner(a, len_acabas, target, r);
		return r;
	}
}
file class Solution1
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
[TestClass]
public sealed class Test40
{
	[TestMethod]
	public void TestMethod1()
	{
		Solution x = new();
		var f = x.CombinationSum2;
		S(f([10, 1, 2, 7, 6, 1, 5], 8),
		[[1, 1, 6], [1, 2, 5], [1, 7], [2, 6]]);
		S(f([2, 5, 2, 1, 2], 5),
		[[1, 2, 2], [5]]);

		S(f([14, 6, 25, 9, 30, 20, 33, 34, 28, 30, 16, 12, 31, 9, 9, 12, 34, 16, 25, 32, 8, 7, 30, 12, 33, 20, 21, 29, 24, 17, 27, 34, 11, 17, 30, 6, 32, 21, 27, 17, 16, 8, 24, 12, 12, 28, 11, 33, 10, 32, 22, 13, 34, 18, 12],
		27),
		[[27], [13, 14], [11, 16], [10, 17], [9, 18], [7, 20], [6, 21], [9, 9, 9], [8, 9, 10], [8, 8, 11], [7, 9, 11], [7, 8, 12], [6, 10, 11], [6, 9, 12], [6, 8, 13], [6, 7, 14], [6, 6, 7, 8]]
		); //Time Limit Exceeded

		S(f([4, 4, 2, 1, 4, 2, 2, 1, 3], 6),
		[[1, 1, 2, 2], [1, 1, 4], [1, 2, 3], [2, 2, 2], [2, 4]]
		);

	}
}