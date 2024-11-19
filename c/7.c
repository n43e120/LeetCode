#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>

int reverse(int x) {
	int64_t n;
	int sig = 0;
	if (x > 0)
	{
	}
	else if (x < 0)
	{
		sig = -1;
		//x = -x;
	}
	else
	{
		return 0;
	}
	int r;
	r = x % 10;
	while (r == 0) {
		x /= 10;
		r = x % 10;
	}
	n = r;
	x /= 10;
	while (x) {
		r = x % 10;
		n *= 10;
		n += r;
		x /= 10;
	}
	if (sig)
	{
		if (n < INT32_MIN)
		{
			return 0;
		}
		return n;
	}
	if (n > INT32_MAX)
	{
		return 0;
	}
	return n;
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
	float:printf("\033[0m\033[1;3%dm%f\033[0m", color, x)\
	))
void T(int input, int expect) {
	printf("f(%d)==%d ", input, expect);
	int actual = reverse(input);
	if (expect == actual)
	{
		COLOR_PRINT("ok", COLOR_GREEN);
	}
	else {
		printf(" != ");
		COLOR_PRINT(actual, COLOR_RED);
	}
	printf("\n");
}
int main()
{
	T(123, 321);
	T(-123, -321);
	T(120, 21);
	T(0, 0);
	T(1534236469, 0);
	T(-2147483647-1, 0);
}
