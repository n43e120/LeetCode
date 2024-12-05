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

double myPow(double x, int n) {
	return pow(x, n); //standard lib is faster.
	double d = 1.0;
	//const double INF = 0x8000000000000000;//2 * DBL_MAX;
	if (n == 0) //x^0 = 1
	{
		return d;
	}
	switch (*((uint64_t*)&x))
	{
	case 0xBFF0000000000000: //-1
		if (n & 1) //-1^Odd == -1
		{
		}
		else {
			return 1.0; //-1^Even == 1
		}
	case 0://0^Inf == 0
	case 0x3FF0000000000000: //1 //1^Inf == 1
		return x;
	default:
		break;
	}
	int64_t i = n;
	if (i < 0) {
		x = 1 / x;
		i = -i;
	}
	uint64_t* pD = &d;
AGAIN:
	d *= x;
	switch (*pD)
	{
	case 0:
	case 0x8000000000000000: //INF
		goto END;
	}
	i--;
	if (i > 0) goto AGAIN;
END:
	return d;
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
void T(double base, int idx, double expect) {
	printf("%f^%d=%f ", base, idx, expect);
	double actual = myPow(base, idx);
	if (actual == expect)
	{
		COLOR_PRINT("ok", COLOR_GREEN);
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
	T(0, 100, 0);
	T(1.0, 2147483647, 1);
	T(-1.0, -3, -1.0);
	T(-1.0, -2147483648i32, 1.0);
	T(2.0, 10, 1024.0);
	T(2.1, 3, 9.26100);
	T(2.0, -2, 0.25000);
	T(2.0, -2147483648i32, 0);
}
