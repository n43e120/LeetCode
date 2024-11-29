#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
//binary search
int search(int* nums, int numsSize, int target) {
	const int NONEXIST = -1;
	int a = 0;
	int b = numsSize - 1;
	int i;
	do
	{
		switch (b - a)
		{
		case 1: //two remaining
			if (nums[b] == target) return b;
		case 0:
			if (nums[a] == target) return a;
		case -1:
			return NONEXIST;
		}
		i = (b + a) >> 1;
		int e = nums[i];
		if (e < target) {
			a = i + 1;
		}
		else if (e > target) {
			b = i - 1;
		}
		else {
			break;
		}
	} while (1);
	return i;

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
void T(int target, int expect, int cnt, int* A) {
	int actual = search(A, cnt, target);
	printf("target %d is at A[%d]", target, expect);
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
int main()
{
	int A[] = { -1, 0, 3, 5, 9, 12 };
	int cnt = sizeof(A) / sizeof(int);
	printf("A[%d]=[", cnt);
	for (int i = 0; i < cnt; i++) {
		printf("%d,", A[i]);
	}
	printf("]\n");
	T(9, 4, cnt, A);
	T(2, -1, cnt, A);
}
