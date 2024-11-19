#include <stdio.h>
#include <stdlib.h>
double median(int* a, int len) {
	int i = len / 2;
	int isOdd = len & 1;
	if (isOdd)
	{
		return a[i];
	}
	else
	{
		i--;
		return (a[i] + a[i + 1]) / 2.0;
	}
}
int fIdxMaxB_lt_A(int a, int* B, int lb, int ub) {
	//a is an element of A
	//if a=b in value, then a<b in sort order
	//return idx of the biggeset b that < a
	//lb and ub set target range B[lb+1...ub-1]
	//by default, lb = -1, ub= lenB
	//return lb if a is lower than all
	//return ub-1 if a is higher than all
	int i;
	i = (lb + ub) / 2;
	if (a <= B[i])
	{
		if (a <= B[0]) return lb; //a is lower than all Bs
		ub = i;
	}
	else { //B[i]<a
		if (B[ub - 1] < a) return ub - 1; //a is higher than all Bs
		lb = i;
	}
AGAIN:
	if (lb == ub - 1)
	{
		return lb;
	}
	i = (lb + ub) / 2;
	if (a <= B[i])
	{
		ub = i;
	}
	else {
		lb = i;
	}
	goto AGAIN;
}
double findMedianSortedArrays(int* nums1, int nums1Size, int* nums2, int nums2Size) {
	if (nums1Size == 0)
	{
		return median(nums2, nums2Size);
	}
	else if (nums2Size == 0) {
		return median(nums1, nums1Size);
	}
	int cntTotal = nums1Size + nums2Size;
	int* A, * B;
	int lenA, lenB;
	if (nums1[nums1Size - 1] <= nums2[0]) //max1 <= min2
	{
		A = nums1;
		B = nums2;
		lenA = nums1Size;
		lenB = nums2Size;
	}
	else if (nums2[nums2Size - 1] <= nums1[0]) //max2 <= min1
	{
		A = nums2;
		B = nums1;
		lenA = nums2Size;
		lenB = nums1Size;
	}
	else
	{
		goto NORMAL_AB;
	}
NO_INTERSECT:
	if (lenA == lenB)
	{
		return (A[lenA - 1] + B[0]) / 2.0;
	}
	else if (lenA > lenB)
	{
		return median(A, cntTotal);
	}
	else {//if (lena < lenb) {
		return median(B, lenB - lenA);
	}
NORMAL_AB:
	if (nums1Size >= nums2Size)
	{
		A = nums1;
		B = nums2;
		lenA = nums1Size;
		lenB = nums2Size;
	}
	else
	{
		A = nums2;
		B = nums1;
		lenA = nums2Size;
		lenB = nums1Size;
	}
	int x, i;
	int lb = -1, ub = lenA;
	int lbB = -1, ubB = lenB;
	int isOdd = cntTotal & 1;
	int iB;
	if (isOdd)
	{
		int lenHalf = cntTotal / 2;
		int cntLowTotal;
		do //try A
		{
			i = (lb + ub) / 2;
			x = A[i];
			iB = fIdxMaxB_lt_A(x, B, lbB, ubB);
			cntLowTotal = iB + 1 + i;
			if (cntLowTotal == lenHalf)
			{
				return x;
			}
			else if (cntLowTotal > lenHalf) { //lower part is too much
				ub = i;
				ubB = iB + 1;
			}
			else {
				lb = i;
				lbB = iB;
			}
			if (lb == ub - 1)
			{
				break;
			}
		} while (1);
		iB = lenHalf - ub;
		return B[iB];
	}
	else {
		int lenHalf = cntTotal / 2 - 1;
		int cntLowTotal;
		do //try A
		{
			i = (lb + ub) / 2;
			x = A[i];
			iB = fIdxMaxB_lt_A(x, B, lbB, ubB);
			cntLowTotal = iB + 1 + i;
			if (cntLowTotal == lenHalf)
			{
				if (i + 1 >= lenA) {
					return (x + B[iB + 1]) / 2.0;
				}
				if (iB + 1 >= lenB) {
					return (x + A[i + 1]) / 2.0;
				}
				int a = A[i + 1];
				int b = B[iB + 1];
				if (a > b)
				{
					return (x + b) / 2.0;
				}
				else {
					return (x + a) / 2.0;
				}
				//return (x + min(a, b)) / 2.0;
			}
			if (cntLowTotal > lenHalf) { //lower part is too much
				ub = i;
				ubB = iB + 1;
			}
			else {
				lb = i;
				lbB = iB;
			}
			if (lb == ub - 1)
			{
				break;
			}
		} while (1);
	A_LOCKED:
		{
			iB = lenHalf - ub;
			x = B[iB];
			if (ub == lenA) {
				return (x + B[iB + 1]) / 2.0;
			}
			if (iB + 1 == lenB) {
				return (x + A[ub]) / 2.0;
			}
			int a = A[ub];
			int b = B[iB + 1];
			if (a > b)
			{
				return (x + b) / 2.0;
			}
			else {
				return (x + a) / 2.0;
			}
			//return (x + min(a, b)) / 2.0;
		}
	}
}

int main()
{
	double x;
	//int A[] = { 1, 3 };
	//int B[] = { 2 };

	//int A[] = { 1, 3 };
	//int B[] = { 2,4 };

	//int A[] = { 2 };
	//int B[] = { 1,3,4,5 };

	int A[] = { 2 };
	int B[] = { 1,3,4,5,6 };
	x = findMedianSortedArrays(A, sizeof(A) / sizeof(int), B, sizeof(B) / sizeof(int));


	//int A[] = { 1, 3 };
	//int B[] = { 2,4 };
	//x = findMedianSortedArrays(A, 0, B, sizeof(B) / sizeof(int));

	printf("%lf", x);



}
