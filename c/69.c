#define _CRT_SECURE_NO_WARNINGS
#include "main.h"
//#include "intrin.h"

int mylzcnt(int n) {
	int i = 0;
	for (; i < 32; i++)
	{
		if (n < 0) break;
		n = ((uint32_t)n) << 1;
	}
	return i;
}
int mySqrt(int x) {//leetcode 69
	//DiscretePlot[{Floor@Sqrt[n], 2^Floor[IntegerLength[n, 2]/2]}, {n, 1, 128}]
	const int max = 46340;//Floor@Sqrt[2^31 - 1]
	if (x <= 1) return x;
	if (x >= max * max) return max;
	int a, b, d, m;
	//a = 32 - __lzcnt(x);
	a = 32 - mylzcnt(x);
	a = 1 << (a >> 1);
	d = x - a * a;
	if (d == 0)
	{
		return a;
	}
	if (d > 0) {
		b = a << 1;
		if (b > max) b = max;
	}
	else {
		b = a;
		a = a >> 1;
	}
	do
	{
		if (b - a <= 1) {
			return a;
		}
		m = (a + b) >> 1;
		d = x - m * m;
		if (d == 0)
		{
			return m;
		}
		else if (d > 0) {
			a = m;
		}
		else {
			b = m;
		}
	} while (true);
}
int main()
{
	int (*f)(int) = &mySqrt;
	T(f(4), 2, "sqrt(4)");
	T(f(9), 3, "sqrt(9)");
	T(f(16), 4, "sqrt(16)");
	T(f(25), 5, "sqrt(25)");
	T(f(2147395599), 46339, "sqrt(25)");
}
