#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>
#include <float.h>
#include <math.h>

int divide(int dividend, int divisor) {
	switch (divisor)
	{
	case 1: return dividend;
	case -1:
		if (dividend == INT32_MIN) { return INT32_MAX; }
		return -dividend;
	case INT32_MIN:
		return (dividend == INT32_MIN);
	default:
		break;
	}
	int sig = 1;
	if (dividend < 0)
	{
		if (dividend != INT32_MIN) dividend = -dividend;
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
	//unchecked
	{
		uint32_t d = (uint32_t)dividend;
		uint32_t s = (uint32_t)divisor;
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

#define COLOR_RED 1
#define COLOR_GREEN 2
#define COLOR_YELLOW 3
#define COLOR_BLUE 4
#define COLOR_PINK 5
#define COLOR_CYAN 6
#define COLOR_PRINT(x, color) (_Generic((x),\
	char*:printf("\033[0m\033[1;3%dm%s\033[0m", color, x),\
	int:  printf("\033[0m\033[1;3%dm%d\033[0m", color, x),\
	float:printf("\033[0m\033[1;3%dm%f\033[0m", color, x),\
	double:printf("\033[0m\033[1;3%dm%f\033[0m", color, x)\
	))
void print_array(int cnt, int* A) {
	printf("[");
	for (int i = 0; i < cnt; i++) {
		printf("%d,", A[i]);
	}
	printf("]");
}
void T(int dividend, int divisor, int expect) {
	printf("%d/%d=%d ", dividend, divisor, expect);
	int actual = divide(dividend, divisor);
	if (actual == expect)
	{
		COLOR_PRINT(" ok", COLOR_GREEN);
	}
	else
	{
		printf(" != ");
		COLOR_PRINT(actual, COLOR_RED);
	}
	printf("\n");
}
#define MY_MACRO_ARRLEN(arr) sizeof(arr) / sizeof(arr[0])
int main()
{
	T(10, 3, 3);
	T(INT32_MIN, 2, INT32_MIN / 2);
	T(INT32_MIN, -1, INT32_MAX);
	T(INT32_MIN, INT32_MIN, 1);
}
