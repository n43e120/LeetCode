#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>

int myAtoi(char* s) {
	int i = 0;
	int sig = 0;
	char c;
SPACE://[0-9 +-]
	c = s[i];
	if (c == 0) {//reach end
		goto BAD_INPUT;
	}
	if (c == ' ')
	{
		i += 1;
		goto SPACE;
	}
	//[0-9+-]
	switch (c)
	{
	case '-'://sign = -1;
	case '+'://sign = 1;
		sig = 44-c;
		i += 1;
		c = s[i];
		if (c == 0) {//reach end
			goto BAD_INPUT;
		}
	}
LEADING_ZERO://[0-9]
	if (c > '9' || c < '0')
	{
		goto BAD_INPUT;
	}
	if (c == '0')
	{
		i += 1;
		c = s[i];
		if (c == 0) {//reach end
			goto BAD_INPUT;
		}
		goto LEADING_ZERO;
	}
	int64_t n = c - '0';//c in [1,9]
	if (sig == -1)
	{
		n = -n;
	}
DIGIT:
	i += 1;
	c = s[i];
	if (c == 0) {//reach end
		return n;
	}
	if (c > '9' || c < '0')
	{
		return n;
	}
	n *= 10;
	if (n < 0) {
		n -= c - '0';
	}
	else
	{
		n += c - '0';
	}
	if (n >= INT_MAX)
	{
		return INT_MAX;
	}
	else if (n <= INT_MIN)
	{
		return INT_MIN;
	}
	goto DIGIT;
BAD_INPUT:
	return 0;
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


//https://www.cnblogs.com/lewki/p/14343894.html //C/C++¡ª¡ªprintf²ÊÉ«Êä³ö
void printf_red(const char* s)
{
	printf("\033[0m\033[1;31m%s\033[0m", s);
}
void T(char* s, int expect) {
	int (*f)(char*) = &myAtoi;
	int actual = f(s);
	if (expect == actual)
	{
		printf("f(%s)==%d ", s, expect);
		//printf_green("ok\n");

		COLOR_PRINT("ok\n", COLOR_GREEN);

		//system("Color 0A"); //green text //not working
		//printf("ok\n");
		//system("Color 07"); //white
	}
	else {
		printf("f(%s)==%d != ", s, expect);
		COLOR_PRINT(actual, COLOR_RED);
	}
}
int main()
{
	T("42", 42);
	T(" 42 ",42);
	T("-1",-1);
	T("+1",1);
	T("1337c0d3",1337);
	T("words and 987",0);
	T("-91283472332",-2147483647-1);
	T("+99999999999",2147483647);
	T("  0000000000012345678",12345678);
	T("21474836460",2147483647);
	T("+ 1",0);//for C
}
