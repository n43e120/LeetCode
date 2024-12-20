#define _CRT_SECURE_NO_WARNINGS
#include "main.h"

int* plusOne(int* digits, int digitsSize, int* returnSize) {//leetcode 66
	int newlen = digitsSize;
	int bytelen = digitsSize * sizeof(digits[0]);
	int i = digitsSize - 1;

	int* r = malloc(bytelen);
	memcpy(r, digits, bytelen);
	do
	{
		if (digits[i] < 9) {
			r[i]++;
			break;
		}
		r[i] = 0;
		i--;
		if (i < 0) {
			int* r2 = malloc(bytelen + sizeof(digits[0]));
			memcpy(r2 + 1, r, bytelen);
			free(r);
			r2[0] = 1;
			digitsSize++;
			r = r2;
			break;
		}
	} while (true);
	*returnSize = digitsSize;
	return r;
}
int main()
{
	int A[] = { 9 };
	int expect[] = { 1, 2, 4 };
	int expect_size = MY_MACRO_ARRLEN(expect);
	int actual_size = 0;
	int* actual = plusOne(A, MY_MACRO_ARRLEN(A), &actual_size);
	print_array(actual_size, actual);
}
