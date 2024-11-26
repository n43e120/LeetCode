/**
 * Definition for singly-linked list.
 * struct ListNode {
 *     int val;
 *     struct ListNode *next;
 * };
 */
//Definition for singly-linked list.
struct ListNode {
	int val;
	struct ListNode* next;
};
struct ListNode* mergeTwoLists(struct ListNode* list1, struct ListNode* list2) {
	if (list1 == NULL) {
		return list2;
	}
	if (list2 == NULL) {
		return list1;
	}
	struct ListNode* head, * A, * B, * C = NULL;
	if (list1->val <= list2->val) {
		head = list1;
		B = list2;
	}
	else {
		head = list2;
		B = list1;
	}
	A = head;

A:
	//A is latest successor,C and B are candidates
	C = A->next;
	if (C == NULL) {
		A->next = B;
		goto END;
	}
	if (C->val <= B->val) {
		A = C;
		goto A;
	}
	A->next = B; //redirect pointer

B:
	//B is latest successor, C and A are candidates
	A = B->next;
	if (A == NULL) {
		B->next = C;
		goto END;
	}
	if (A->val <= C->val) {
		B = A;
		goto B;
	}
	B->next = C; //redirect pointer

C:	//C is latest successor, B and A are candidates
	B = C->next;
	if (B == NULL) {
		C->next = A;
		goto END;
	}
	if (B->val <= A->val) {
		C = B;
		goto C;
	}
	C->next = A;
	goto A;

END:
	return head;
}