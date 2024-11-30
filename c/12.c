#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>
int my_pow(int base, int idx) {
	if (idx == 0)
	{
		return 1;
	}
	int n = base;
	for (size_t i = 0; i < idx-1; i++)
	{
		n *= base;
	}
	return n;
}
char* intToRoman(int num) {
	const char* R = "IVXLCDM"; //ROMAN NUMS
	char* B = malloc(100);//buffer
	int d = 3; //digit
	int i = 0;
	do
	{
		int q = my_pow(10, d);
		char* r = R + (d << 1);
		int k = num / q;
		switch (k)
		{
		case 9:
			B[i] = r[0]; //"I"
			i++;
			B[i] = r[2]; //"X"
			i++;
			break;
		case 4:
			B[i] = r[0]; //"I"
			i++;
			B[i] = r[1]; //"V"
			i++;
			break;
		case 5:
		case 8:
		case 7:
		case 6:
			B[i] = r[1]; //"V"
			i++;
			k -= 5;
		case 3:
		case 2:
		case 1:
		AGAIN:
			if (k > 0) {
				B[i] = r[0]; //"I"
				i++;
				k--;
				goto AGAIN;
			}
			break;
		case 0:
		default:
			break;
		}
		num %= q;
		d--;
	} while (d >= 0);
	B[i] = '\0';
	return B;
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
void print_array(int cnt, int* A) {
	printf("[");
	for (int i = 0; i < cnt; i++) {
		printf("%d,", A[i]);
	}
	printf("]");
}
void T(int n, char* romanNum) {
	printf("'%s'=%d ", romanNum, n);
	char* actual = intToRoman(n);
	if (!strcmp(romanNum, actual))
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
	T(3749, "MMMDCCXLIX");
	T(58, "LVIII");
	T(1994, "MCMXCIV");
}
