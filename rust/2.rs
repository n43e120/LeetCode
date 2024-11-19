//Definition for singly-linked list.
#[derive(PartialEq, Eq, Clone, Debug)]
pub struct ListNode {
    pub val: i32,
    pub next: Option<Box<ListNode>>,
}

impl ListNode {
    #[inline]
    pub(crate) fn new(val: i32) -> Self {
        ListNode {
            next: None,
            val,
        }
    }
}
struct Solution {}
impl Solution {
    pub fn add_two_numbers(l1: Option<Box<ListNode>>, l2: Option<Box<ListNode>>) -> Option<Box<ListNode>> {
        let mut l3 = Some(Box::new(ListNode::new(0)));
        let mut c = l3.as_mut().unwrap();
        let mut a = l1;
        let mut b = l2;
        let mut x: i32;
        loop { //ab
            x = a.as_ref().unwrap().val + b.as_ref().unwrap().val + c.val;
            if x > 9 {//has carry
                x -= 10;
                c.val = x;
                x = 1;
            } else {
                c.val = x;
                x = 0;
            }
            a = a.unwrap().next;
            b = b.unwrap().next;
            if a.is_some() {
                c.next = Some(Box::new(ListNode::new(x))); //delay carry
                c = c.next.as_mut().unwrap();
                if b.is_some() {
                    continue;
                } else {
                    break;
                }
            } else if b.is_some() { //only b
                c.next = Some(Box::new(ListNode::new(x))); //delay carry
                c = c.next.as_mut().unwrap();
                a = b;
                break;
            } else {
                if x > 0 {
                    c.next = Some(Box::new(ListNode::new(x)));
                }
                return l3;
            }
        }
        loop { //only a
            x = a.as_ref().unwrap().val + c.val;
            if x > 9 {
                x -= 10;
                c.val = x;
                x = 1;
            } else {
                c.val = x;
                x = 0;
            }
            a = a.unwrap().next;
            if a.is_some() {
                c.next = Some(Box::new(ListNode::new(x))); //delay carry
                c = c.next.as_mut().unwrap();
                continue;
            } else {
                if x > 0 {
                    c.next = Some(Box::new(ListNode::new(x)));
                }
                return l3;
            }
        }
        return l3;
    }
}