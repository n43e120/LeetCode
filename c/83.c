#define _CRT_SECURE_NO_WARNINGS
#include "main.h"

struct ListNode {
	int val;
	struct ListNode* next;
};
struct ListNode* deleteDuplicates(struct ListNode* head) {//leetcode 83
	if (head == NULL) return head;
	int lastval = head->val;
	struct ListNode* a, * b, * c;
	a = head;
AGAIN:
	b = a->next;
	if (b == NULL) goto END;
	if (b->val == a->val) {
		int v = a->val;
		do
		{
			b = b->next;
			if (b == NULL) {
				a->next = NULL;
				goto END;
			}
		} while (b->val == v);
		a->next = b;
	}
	a = b->next;
	if (a == NULL) goto END;
	if (b->val == a->val) {
		int v = b->val;
		do
		{
			a = a->next;
			if (a == NULL) {
				b->next = NULL;
				goto END;
			}
		} while (a->val == v);
		b->next = a;
	}
	goto AGAIN;
END:
	return head;
}
int main()
{
	//int A[] = { 1,1,2,3 };
	//int expect[] = { 1, 2, 3 };
	//int expect_size = MY_MACRO_ARRLEN(expect);
	//int actual_size;
	//int* actual = deleteDuplicates(A, MY_MACRO_ARRLEN(A), &actual_size);
	//print_array(actual_size, actual);
}
