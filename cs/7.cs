public class Solution
{
	public int Reverse(int x)
	{
		if (x == 0) return 0;
		var s = string.Concat(x.ToString().TrimEnd('0').Reverse());
		long n;
		if (x < 0)
		{
			n = -long.Parse(s[..^1]);
			if (n < int.MinValue) return 0;
			return (int)n;
		}
		n = long.Parse(s);
		if (n > int.MaxValue) return 0;
		return (int)n;
	}
}

internal class Program
{
	static void COLOR_PRINT(object x, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.Write(x);
		Console.ResetColor();
	}
	static void Main(string[] args)
	{
		Solution x = new();
		var f = x.Reverse;
		void T(int input, int expect)
		{
			Console.Write("f({0})=={1}", input, expect);
			int actual = f(input);
			if (expect == actual)
			{
				COLOR_PRINT(" ok", ConsoleColor.Green);
				Console.WriteLine();
			}
			else
			{
				Console.Write(" != ");
				COLOR_PRINT(actual, ConsoleColor.Red);
				Console.WriteLine();
			}
		}
		T(123, 321);
		T(-123, -321);
		T(120, 21);
		T(0, 0);
		T(1534236469, 0);
	}
}