#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>

int removeElement(int* nums, int numsSize, int val) {
	switch (numsSize)
	{
	case 1:
		if (nums[0] != val) return numsSize;
	case 0:
		return 0;
	default:
		break;
	}
	int i = numsSize - 1;
	do
	{
		if (val != nums[i])
		{
			goto IJ_SEPRATION;
		}
		i--;
	} while (i > -1);
	return 0;
IJ_SEPRATION:
	int j = i;//idx of last
	do
	{
		i--;
		if (i < 0) {
			return j + 1;
		}
		if (val == nums[i])
		{
			nums[i] = nums[j];
			j--;
		}
	} while (1);
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
void T(int cnt, int* A, int val, int expect) {
	print_array(cnt, A);
	printf(".Remove(%d).len=%d", val, expect);
	auto actual = removeElement(A, cnt, val);
	if (expect == actual)
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
	{
		int A[] = { 1 };
		int val = 1;
		int expect = 0;
		int cnt = MY_MACRO_ARRLEN(A);
		T(cnt, A, val, expect);
	}
	{
		int A[] = { 1 };
		int val = 2;
		int expect = 1;
		int cnt = MY_MACRO_ARRLEN(A);
		T(cnt, A, val, expect);
	}
	{
		int A[] = { 3,2,2,3 };
		int val = 3;
		int expect = 2;
		int cnt = MY_MACRO_ARRLEN(A);
		T(cnt, A, val, expect);
	}
	{
		int A[] = { 0,1,2,2,3,0,4,2 };
		int val = 2;
		int expect = 5;
		int cnt = MY_MACRO_ARRLEN(A);
		T(cnt, A, val, expect);
	}
}
