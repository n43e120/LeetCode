#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>

int strStr(char* haystack, char* needle) {
	const int NOT_FOUND = -1;
	int lenA = strlen(haystack);
	int lenB = strlen(needle);
	int idxNextLast = lenA - lenB;
	if (idxNextLast < 0) {
		return NOT_FOUND;
	}
	int iA = 0;
AGAIN:
	for (size_t iB = 0; iB < lenB; iB++)
	{
		if (needle[iB] != haystack[iA + iB])
		{
			iA++;
			if (iA <= idxNextLast) {
				goto AGAIN;
			}
			return NOT_FOUND;
		}
	}
	return iA;
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
void T(char* A, char* target, int expect) {
	printf("'%s'.Find(%s)=%d", A, target, expect);
	auto actual = strStr(A, target);
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
		char* A[] = { "sadbutsad", "sad" };
		char* ptr = strstr(A[0], A[1]);
		int pos = ptr ? ptr - A[0] : -1; //0
		T(A[0], A[1], pos);
	}
	{
		char* A[] = { "leetcode", "leeto" };
		char* ptr = strstr(A[0], A[1]);
		int pos = ptr ? ptr - A[0] : -1; //-1
		T(A[0], A[1], pos);
	}
}
