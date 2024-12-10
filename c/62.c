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
int my_pow(int base, int idx) {
	if (idx == 0)
	{
		return 1;
	}
	int n = base;
	for (size_t i = 0; i < idx - 1; i++)
	{
		n *= base;
	}
	return n;
}
uint64_t Factorial(int n) //int overflow at n=13
{
	uint64_t r = 1;
	for (int i = 1; i <= n; i++)
	{
		r *= i;
	}
	return r;
}
int Binominal(int n, int m)// choose m from n //C(m,n)= n!/m!(n-m)!
{
	uint64_t r = 1;
	for (int i = m + 1; i <= n; i++)
	{
		r *= i;
	}
	return r / Factorial(n - m);
}
int Binominal2(int n, int m)
{
	switch (m)
	{
	case 0: return 1;
	case 1: return n;
	default:
		break;
	}
	switch (n - m)
	{
	case 0: return 1;
	case 1: return n;
	default:
		break;
	}
	//Table[Prime[i], {i, 1, PrimePi[200]}]
	static uint8_t primes[] = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, \
67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, \
139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199 };
	const int PrimePi100 = sizeof(primes);
	uint8_t factors[sizeof(primes)];
	for (int i = 0; i < PrimePi100; i++)
	{
		factors[i] = 0;
	}
	for (int num = m + 1; num <= n; num++)
	{
		int x = num;
		for (int i = 0; i < PrimePi100; i++)
		{
			int p = primes[i];
			do
			{
				div_t result = div(x, p);
				if (result.rem != 0) break;
				factors[i] += 1;
				x = result.quot;
				if (x == 1)
				{
					goto NEXT_NUM;
				}
			} while (true);
		}
	NEXT_NUM:;
	}
	for (int num = 2; num <= (n - m); num++)
	{
		int x = num;
		for (int i = 0; i < PrimePi100; i++)
		{
			int p = primes[i];
			do
			{
				div_t result = div(x, p);
				if (result.rem != 0) break;
				factors[i] -= 1;
				x = result.quot;
				if (x == 1)
				{
					goto NEXT_NUM_2;
				}
			} while (true);
		}
	NEXT_NUM_2:;
	}
	int r = 1;
	for (int i = 0; i < PrimePi100; i++)
	{
		int f = factors[i];
		if (f > 0)
		{
			r *= my_pow(primes[i], factors[i]);
		}
	}
	return r;
}
int uniquePaths(int m, int n) {//leetcode 62
	return Binominal2((m - 1) + (n - 1), (m - 1));
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
static void T(int m, int n, int expect) {
	printf("m=%d,n=%d,output=%d ", m, n, expect);
	int actual = uniquePaths(m, n);
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
	//Map[Function[x, {PrimePi[x[[1]] - 1], x[[2]]} ], FactorInteger[Binomial[34, 12]]]
	T(3, 7, 28);
	T(3, 2, 3);
	T(13, 23, 548354040);
	T(100, 3, 5050);
}
