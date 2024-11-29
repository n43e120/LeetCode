#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
//#include <regex.h> //unsupport
#include <assert.h>
#include <stdint.h>
#include <stdbool.h>

/// valid parenthese pair check
/// s consists of parentheses only '()[]{}'.
/// 1 <= s.length <= 10^4
bool isValid(char* s) {
	//parentheses
	//square bracket
	//curly braces
	int j = -1;//idx Stack
	int i = -1;
	char c;
	do
	{
		i++;
		c = s[i];
		switch (c)
		{
		case 0:goto END;
		case '(':
		case '[':
		case '{':
			j++;
			continue;
		case ')':
		case ']':
		case '}':
			goto NESTED;
		default: //strange input
			return false;
		}
	} while (true);
NESTED:
	if (j < 0)
	{
		return false;//too many end brackets, stack underflow
	}
	switch (c - s[j])
	{
	case 1: //()
	case 2: //[] or {}
		j--; //open and end cancel out
		break;
	default: //end bracket mismatch open bracket type
		return false;
	}

	do
	{
		i++;
		c = s[i];
		switch (c)
		{
		case 0:goto END;
		case '(':
		case '[':
		case '{':
			j++;
			s[j] = c;
			continue;
		case ')':
		case ']':
		case '}':
			goto NESTED;
		default: //strange input
			return false;
		}
	} while (true);
END:
	if (j > -1)
	{
		return false; //uneven pair
	}
	return true; //all opens and ends has balanced out
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
char M_BUFF[20];
void T(char* input, bool expect) {
	strcpy(M_BUFF, input);
	printf("f(\"%s\")=%s ", input, expect ? "true" : "false");
	auto actual = isValid(M_BUFF);
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
//char M_BUFF[20];
//char* M_BUFF;
int main()
{
	//M_BUFF = malloc(100);
	T("", true);
	T(")", false);
	T("(", false);
	T("()", true);
	T("()[]{}", true);
	T("([])", true);
	T("(]", false);
	//free(M_BUFF);
}
