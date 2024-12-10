using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using static ConsoleApp1.TestHelper;
public class Solution
{
	const string P = "()";
	IEnumerable<string> ExpandNode(string s)
	{
		int i = s.Length;
		int len = i + 2;
		//if (!s.EndsWith(P))//only "..)))" +()
		//{
		//	yield return s + P;
		//}
		do
		{
			i--;
			if (i == 1) break;
			var spHead = s.AsSpan().Slice(0, i);
			if (!spHead.EndsWith(P))
			{
				yield return string.Concat(spHead, P, s.AsSpan().Slice(i));
			}
		} while (true);
		yield return string.Concat(s.AsSpan().Slice(0, i), P, s.AsSpan().Slice(i));
		if (s.StartsWith(P))//only ()+ if "()..."
		{
			yield return P + s;
		}
	}
	public IList<string> GenerateParenthesis(int n)
	{
		var A = new HashSet<string>();
		A.Add(P);
	AGAIN:
		n--;
		if (n == 0) return A.ToList();
		var B = new HashSet<string>();
		foreach (var a in A)
		{
			foreach (var b in ExpandNode(a))
			{
				B.Add(b);
			}
		}
		n--;
		if (n == 0) return B.ToList();
		A = new HashSet<string>();
		foreach (var b in B)
		{
			foreach (var a in ExpandNode(b))
			{
				A.Add(a);
			}
		}
		goto AGAIN;
	}
}
internal static class Program
{
	static void T(int input, IList<string> expect)
	{
		Print($"f@{input}=");
		PRINT_LIST(expect);
		Solution x = new();
		var f = x.GenerateParenthesis;
		var actual = f(input);
		if (IsEqualSet(expect, actual))
		{
			COLOR_PRINT(ConsoleColor.Green, () => { Print(" pass"); });
		}
		else
		{
			Print(" != ");
			COLOR_PRINT(ConsoleColor.Red, () =>
			{
				PRINT_LIST(actual);
			});
		}
		Console.WriteLine();
	}
	static void Main(string[] args)
	{
		T(1, ["()"]);
		T(2, ["()()", "(())"]);
		T(3, ["((()))", "(()())", "(())()", "()(())", "()()()"]);

		//Solution x = new();
		//var f = x.GenerateParenthesis(8);
		//PRINT_LIST(f);
	}
}