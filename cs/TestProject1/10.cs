file class Solution
{
	int MinLen(ReadOnlySpan<char> A)
	{
		int i = A.Length - 1;
		int cnt = 0;
		while (i >= 0)
		{
			switch (A[i])
			{
				case '*':
					i -= 2;
					continue;
			}
			cnt++;
			i--;
		}
		return cnt;
	}
	bool IsMinLenZeroWidth(ReadOnlySpan<char> A)
	{
		int i = A.Length - 1;
		while (i >= 0)
		{
			switch (A[i])
			{
				case '*':
					i -= 2;
					continue;
			}
			return false;
		}
		return true;
	}
	bool IsMatch_inner(ReadOnlySpan<char> A, ReadOnlySpan<char> B)
	{
		var lenA = A.Length;
		var lenB = B.Length;

		int ib, ia;
		char a, b;

		ia = lenA - 1;
		ib = lenB - 1;

	MATCH_BACKWARD:
		if (ib < 0) return (ia < 0); //"",""
		b = B[ib];
		if (ia < 0) return IsMinLenZeroWidth(B.Slice(0, ib + 1));//string end, pattern not
		a = A[ia];
		if (a != b)
		{
			switch (b)
			{
				case '.':
					break;
				case '*':
					if (ib - 1 >= 0) //check if case "...a" "...b*"
					{
						b = B[ib - 1];
						if (a != b && b != '.') //"a" "b*" match zero-width
						{
							ib -= 2;
							goto MATCH_BACKWARD;
						}
					}
					if (ia + 1 < lenA)//if ia changed
					{
						A = A.Slice(0, ia + 1); lenA = A.Length;
						B = B.Slice(0, ib + 1); lenB = B.Length;
					}
					ia = 0;
					ib = 0;
					goto MATCH_FORWARD;
				default:
					return false;//simple not match
			}
		}
		ia--;
		ib--;
		goto MATCH_BACKWARD;
	MATCH_FORWARD:
		if (ib >= lenB)
		{
			if (ia >= lenA) return true;//both reach end

			//pattern ended, string not
			a = A[ia];

			ib--; if (ib < 0) return false; //lenB=0, string is longer
			b = B[ib];
			if (b != '*') return false; //not end with "...*"
			ib--; if (ib < 0) return false;//error format
			b = B[ib];
			if (b == '.') return true;//".*" match anything
			do //"a*" matches "aaaaa..."
			{
				if (a != b) return false;
				ia++; if (ia >= lenA) return true;
				a = A[ia];
			} while (true);
		}
		b = B[ib];
		if (ia >= lenA) return IsMinLenZeroWidth(B.Slice(ib));//string ended, pattern not
		a = A[ia];
		if (ib + 1 < lenB && B[ib + 1] == '*') //"a*"
		{
			if (a != b && b != '.') //"a..." "b*..." match zero-width
			{
				ib += 2;
				goto MATCH_FORWARD;
			}
			if (ia > 0)
			{
				A = A.Slice(ia); lenA = A.Length;
				B = B.Slice(ib); lenB = B.Length;
			}
			ia = 0;
			ib = 0;
			goto MATCH_Q;
		}
		if (a != b && b != '.') return false; //simple not match
		ib++;
		ia++;
		goto MATCH_FORWARD;

	MATCH_Q://match *
		var min = MinLen(B);
		if (min > lenA) return false; //min length is longer than input

		b = B[0];
		if (lenB == 2)
		{
			if (b == '.') return true;//.*
			return A.IndexOfAnyExcept(b) < 0; //a*
		}
		//?*???*
		var trailingB = B.Slice(2);
		int i = 0;
		int longestMatchLen = lenA - min;
		if (b == '.')
		{
			i = longestMatchLen;
		}
		else
		{
			i = A.IndexOfAnyExcept(b);
			if (i == 0)
			{ //b* match nothing
				B = trailingB;
				lenB -= 2;
				goto MATCH_FORWARD;
			}
			i = i < 0 ? longestMatchLen : Math.Min(i, longestMatchLen);
		}
		for (; i >= 0; i--)
		{
			if (IsMatch_inner(A.Slice(i), trailingB))
			{
				return true;
			}
		}
		return false;
	}
	public bool IsMatch(string s, string p)
	{
		var A = s.AsSpan();
		var B = p.AsSpan();
		return IsMatch_inner(A, B);
	}
}
[TestClass]
public class Test10
{
	[TestMethod]
	public void TestMethod1()
	{
		Solution x = new();
		var f = x.IsMatch;
		T(f("", ""), true);
		T(f("", ".*"), true);
		T(f("a", "."), true);
		T(f("ab", ".."), true);
		T(f("aa", "a"), false);
		T(f("aa", "a*"), true);
		T(f("ab", ".*"), true);
		T(f("aaaaaaaaaab", "a*"), false);
		T(f("a", "a*a"), true);
		T(f("bbbba", ".*a*a"), true);
		T(f("a", ".*."), true);
		T(f("baabbbaccbccacacc", "c*..b*a*a.*a..*c"), true);
		T(f("aaa", "ab*ac*a"), true);
	}
}