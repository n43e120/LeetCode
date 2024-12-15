file class Solution
{
	public int Divide(int dividend, int divisor)
	{
		//if (dividend == int.MinValue && divisor == -1) { return int.MaxValue; }
		//return dividend / divisor;
		switch (divisor)
		{
			case 1: return dividend;
			case -1:
				if (dividend == int.MinValue) { return int.MaxValue; }
				return -dividend;
			default:
				break;
		}
		int sig = 1;
		if (dividend < 0)
		{
			dividend = -dividend;
			if (divisor < 0)
			{
				divisor = -divisor;
			}
			else
			{
				sig = -1;
			}
		}
		else if (divisor < 0)
		{
			divisor = -divisor;
			sig = -1;
		}
		unchecked
		{
			uint d = (uint)dividend;
			uint s = (uint)divisor;
			if (s > d)
			{
				return 0;
			}
			else if (s == d)
			{
				return sig;
			}
			while ((s & 1) == 0)
			{
				s >>= 1;
				d >>= 1;
			}

			int sum = 0;
			int q;
			while (d > 0)
			{
				q = 0;
				while (d >= s << q)
				{
					q++;
				}
				q--;
				if (q < 0)
				{
					goto END;
				}
				d -= s << q;
				sum += 1 << q;
			}
		END:
			if (sig < 0)
			{
				return -sum;
			}
			return sum;
		}
	}
}
[TestClass]
public sealed class Test29
{
	[TestMethod]
	public void TestMethod1()
	{
		var x = new Solution();
		var f = x.Divide;
		//T(f(10, 3), 3);
		T(f(-2147483648, 2), -2147483648 / 2);
		//T(f(-2147483648, -1), int.MaxValue);
	}
}