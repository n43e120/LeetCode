#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>

int searchInsert(int* nums, int numsSize, int target) {
	const int NONEXIST = -1;
	int a = 0;
	int b = numsSize - 1;
	int i;
	do
	{
		switch (b - a)
		{
		case 1: //two remaining
			if (nums[a] >= target) return a;
			if (nums[b] >= target) return b;
			return b + 1;
		case 0:
			if (nums[a] >= target) return a;
			return a + 1;
		case -1: //array len = 0
			return NONEXIST;
		}
		i = (b + a) >> 1;
		if (nums[i] >= target) {
			b = i - 1;
		}
		else
		{
			a = i;
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
void T(int target, int expect, int cnt, int* A) {
	print_array(cnt, A);
	int actual = searchInsert(A, cnt, target);
	printf(".FindInsertPos(%d)=%d ", target, expect);
	if (expect == actual)
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
	{
		int A[] = { 1,3,5,6 };
		int target = 5;
		int idx = 2;
		int cnt = MY_MACRO_ARRLEN(A);
		T(target, idx, cnt, A);
	}
	{
		int A[] = { 1,3,5,6 };
		int target = 2;
		int idx = 1;
		int cnt = MY_MACRO_ARRLEN(A);
		T(target, idx, cnt, A);
	}
	{
		int A[] = { 1,3,5,6 };
		int target = 7;
		int idx = 4;
		int cnt = MY_MACRO_ARRLEN(A);
		T(target, idx, cnt, A);
	}
}
