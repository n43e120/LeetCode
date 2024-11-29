#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>

/// 1 <= nums.length <= 3 * 10^4
/// - 100 <= nums[i] <= 100
/// nums is sorted in non-decreasing order.
int removeDuplicates(int* nums, int numsSize) {
	switch (numsSize)
	{
	case 1:
	case 0:
		return numsSize;
	}
	int i = 0;
	int j = 1;
	char cj, ci;
	ci = nums[i];
	do
	{
		cj = nums[j];
		if (ci == cj)
		{
			break;
		}
		i += 2;
		if (i >= numsSize)
		{
			return i;//=numsSize;
		}
		ci = nums[i];
		if (ci == cj)
		{
			goto J_STAND_STILL;
		}
		j += 2;
		if (j >= numsSize)
		{
			return j;//=numsSize;
		}
	} while (1);
	int tmp;
I_STAND_STILL:
	j++;
	if (j >= numsSize)
	{
		return i + 1;
	}
	cj = nums[j];
	if (ci == cj)
	{
		goto I_STAND_STILL;
	}
	tmp = j;
	j = i + 1;
	i = tmp;
	nums[j] = cj;
J_STAND_STILL:
	i++;
	if (i >= numsSize)
	{
		return j + 1;
	}
	ci = nums[i];
	if (ci == cj)
	{
		goto J_STAND_STILL;
	}
	tmp = i;
	i = j + 1;
	j = tmp;
	nums[i] = ci;
	goto I_STAND_STILL;
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
	printf("[", cnt);
	for (int i = 0; i < cnt; i++) {
		printf("%d,", A[i]);
	}
	printf("]");
}
void T(int input_cnt, int* input_arr, int expect) {
	print_array(input_cnt, input_arr);
	printf(".ToSet().len=%d", expect);
	auto actual = removeDuplicates(input_arr, input_cnt);
	if (expect == actual)
	{
		COLOR_PRINT("ok", COLOR_GREEN);
	}
	else
	{
		printf("!= ");
		COLOR_PRINT(actual, COLOR_RED);
	}
	printf("\n");
}
#define MY_MACRO_ARRLEN(arr) sizeof(arr) / sizeof(arr[0])
int main()
{
	{
		int A[] = { 1, 1, 2 };
		int cnt = MY_MACRO_ARRLEN(A);
		T(MY_MACRO_ARRLEN(A), A, 2);
	}
	{
		int A[] = { 0,0,1,1,1,2,2,3,3,4 };
		int cnt = MY_MACRO_ARRLEN(A);
		T(MY_MACRO_ARRLEN(A), A, 5);
	}
	{
		int A[] = {1,1,2,3};
		int cnt = MY_MACRO_ARRLEN(A);
		T(MY_MACRO_ARRLEN(A), A, 3);
	}
}
