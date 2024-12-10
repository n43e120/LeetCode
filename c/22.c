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

void First(char* A, int cnt) //output ((()))\0
{
	int i = 0;
	int m = cnt / 2;
	for (; i < m; i++)
	{
		A[i] = '(';
	}
	for (i = m; i < cnt; i++)
	{
		A[i] = ')';
	}
	A[cnt] = '\0';
}
inline void Swap(char* a, char* b) {
	char c = *a;
	*a = *b;
	*b = c;
}
bool Next(char* A, int cnt)
{
	switch (cnt)
	{
	case 0://""
	case 2://()
		return false;
	}
	int i = cnt - 2;
	int left = 0;
CHECK_RB:
	if (A[i] == '(')//()
	{
		left++;
		i -= 2;
		if (i < 2) return false; //last permutation ()()()()
		goto CHECK_RB;
	}
	do
	{
		i--;
	} while (A[i] == ')');

	A[i] = ')';
	i++;
	for (; left > -1; left--)
	{
		A[i] = '(';
		i++;
	}
	for (; i < cnt; i++)
	{
		A[i] = ')';
	}
	return true;
}
char** generateParenthesis(int n, int* returnSize) {//leetcode 22
	const char P[] = "()";
	int LEN[] = { 1, 2, 5, 14, 42, 132, 429, 1430 };
	int len = LEN[n - 1];
	*returnSize = len;
	int strlen_Zero = n * 2 + 1;
	int totalbytes = len * (strlen_Zero + sizeof(char*));
	char** buf = malloc(totalbytes);
	char* ptrS = (char*)(buf + len);
	for (size_t i = 0; i < len; i++)
	{
		buf[i] = ptrS + strlen_Zero * i;
	}
	First(ptrS, n * 2);
	for (size_t i = 1; i < len; i++)
	{
		strcpy(buf[i], buf[i - 1]);
		Next(buf[i], n * 2);
	}
	return buf;
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

#define PAIRCNT 3
#define CNT PAIRCNT * 2
int main()
{
	//int cnt = CNT;
	//char buf[CNT + 1];
	//First(buf, cnt);
	//printf("%s\n", buf);
	//bool b;
	//do
	//{
	//	b = Next(buf, cnt);
	//	if (b)
	//	{
	//		printf("%s\n", buf);
	//	}
	//	else
	//	{
	//		break;
	//	}
	//} while (true);

	int cnt;
	char** output = generateParenthesis(4, &cnt);
	char* s = output + cnt;
	for (size_t i = 0; i < cnt; i++)
	{
		printf("%s\n", output[i]);
	}
	free(output);
}
