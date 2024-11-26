#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>

char* convert(char* s, int numRows) {
	int lenTotal = strlen(s);
	if (lenTotal <= 2)	return s;
	if (numRows == 1)	return s;
	int idxLastLine = numRows - 1;
	int period = numRows * 2 - 1 - 1;
	char* r = malloc(lenTotal + 1);
	r[lenTotal] = 0;
	int ir = 0;
	for (size_t iLine = 0; iLine < numRows; iLine++)
	{
		int i = iLine;
		if (i == 0 || i == idxLastLine)
		{
			while (i < lenTotal) {
				r[ir++] = s[i];
				i += period;
			}
		}
		else {
			int offSetConj = 2 * (idxLastLine - i);  //conjugation
			while (i < lenTotal) {
				r[ir++] = s[i];
				int iconj = i + offSetConj;
				if (iconj >= lenTotal) break;
				r[ir++] = s[iconj];
				i += period;
			}
		}
	}
	return r;
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
void T(char* input, int numLine, char* expect) {
	printf("f(%s,%d)==%s ", input, numLine, expect);
	char* actual = convert(input, numLine);
	if (!strcmp(expect, actual))
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
	T("PAYPALISHIRING", 3, "PAHNAPLSIIGYIR");
	T("PAYPALISHIRING", 4, "PINALSIGYAHRPI");
	T("A", 1, "A");
	T("ABC", 1, "ABC");
}
