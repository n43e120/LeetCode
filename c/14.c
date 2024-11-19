#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
char* longestCommonPrefix(char** strs, int strsSize) {
	const int LEN_MIN_STR = 0;
	const int LEN_MAX_STR = 200;
	const int LEN_MIN_ARR = 1;
	const int LEN_MAX_ARR = 200;
	char* buffer = calloc(LEN_MAX_STR, sizeof(char));
	switch (strsSize)
	{
	case 1:
		strcpy(buffer, strs[0]);
	case 0:
		return buffer;
	}

	for (size_t iC = 0; iC < LEN_MAX_STR; iC++)
	{
		size_t iS = 0;
		char c = strs[iS][iC];
		if (c == 0) {
			break;
		}
		iS++;
		for (; iS < strsSize; iS++)
		{
			if (c == strs[iS][iC])
			{
				continue;
			}
			else {
				goto SALIDA;
			}
		}
		buffer[iC] = c;
	}
SALIDA:
	return buffer;
}
int main()
{
	//char* A[] = {"flower","flow","flight"};
	char* A[] = { "","" };
	int arraySize = sizeof(A) / sizeof(A[0]);
	char* r = longestCommonPrefix(A, arraySize);
	printf(r);
	free(r);
}
