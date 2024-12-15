file class Solution
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
[TestClass]
public sealed class Test7
{
	[TestMethod]
	public void TestMethod1()
	{
		var x = new Solution();
		var f = x.Reverse;
		T(f(123), 321);
		T(f(-123), -321);
		T(f(120), 21);
		T(f(0), 0);
		T(f(1534236469), 0);
	}
}