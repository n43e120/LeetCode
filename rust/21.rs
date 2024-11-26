use std::cell::RefCell;
use std::mem::swap;
use std::rc::Rc;

#[derive(PartialEq, Eq, Clone, Debug)]
pub struct ListNode {
    pub val: i32,
    pub next: Option<Box<ListNode>>,
}
impl ListNode {
    #[inline]
    fn new(val: i32) -> Self {
        ListNode { next: None, val }
    }
}
pub struct Solution {}
impl Solution {
    pub fn merge_two_lists(
        list1: Option<Box<ListNode>>,
        list2: Option<Box<ListNode>>,
    ) -> Option<Box<ListNode>> {
        if list1 == None {
            return list2;
        }
        if list2 == None {
            return list1;
        }
        fn rc(x: Box<ListNode>) -> Rc<RefCell<Box<ListNode>>> {
            Rc::new(RefCell::new(x))
        }
        // fn rc(x: Option<Box<ListNode>>) -> Rc<RefCell<Option<Box<ListNode>>>> {
        //     Rc::new(RefCell::new(x))
        // }
        fn outbox(x: &Rc<RefCell<Box<ListNode>>>) -> Box<ListNode> {
            x.replace(Box::new(ListNode::new(100)))
        }
        fn inbox(x: &Rc<RefCell<Box<ListNode>>>, thing: Box<ListNode>) -> Box<ListNode> {
            x.replace(thing)
        }
        fn new_ele(val: i32) -> Option<Box<ListNode>> {
            Some(Box::new(ListNode::new(val)))
        }

        let mut rc_a = rc(list1.unwrap());
        let mut rc_b = rc(list2.unwrap());
        if rc_a.borrow().val <= rc_b.borrow().val {
        } else {
            swap(&mut rc_a, &mut rc_b);
        }
        let mut ret = new_ele(rc_a.borrow().val);
        {
            let mut current_ele = ret.as_mut().unwrap();
            loop {
                //a is the latest successor,C and B are candidates
                if rc_a.borrow().next == None {
                    //a.next = b;
                    let b = outbox(&rc_b);

                    current_ele.next = Some(b);
                    //print!("+tail");
                    break;
                    // print_list_list_node(&b);
                    // rc_a.borrow_mut().next.replace(b);
                    // break;
                };
                let mut a = outbox(&rc_a);
                let Some(c) = a.next.replace(Box::new(ListNode::new(99))) else {
                    todo!()
                };
                let clone_c = c.clone();
                if c.val <= rc_b.borrow_mut().val {
                    //a = a.next;
                    current_ele.next = new_ele(c.val);
                    current_ele = current_ele.next.as_mut().unwrap();
                    //println!("->{}", c.val);
                    a.next.replace(c);
                    //inbox(&rc_a, clone_c);
                    inbox(&rc_a, a);
                    rc_a = rc(clone_c);
                } else {
                    //b is new a
                    //a.next = b;
                    let b = outbox(&rc_b);

                    current_ele.next = new_ele(b.val);
                    current_ele = current_ele.next.as_mut().unwrap();
                    //println!("x{}", b.val);
                    let clone_b = b.clone();
                    a.next.replace(b.clone());

                    // inbox(&rc_a, clone_b);
                    // inbox(&rc_b, clone_c);
                    inbox(&rc_b, b);
                    inbox(&rc_a, a);
                    rc_a = rc(clone_b.clone());
                    rc_b = rc(clone_c);
                }
            }
        }
        ret
    }
}
pub struct SolutionUnsafe {}
impl SolutionUnsafe {
    unsafe fn ref2ptr<T>(x: &T) -> *mut T {
        *((&raw const x) as *mut *mut T)
    }
    pub fn merge_two_lists(
        list1: Option<Box<ListNode>>,
        list2: Option<Box<ListNode>>,
    ) -> Option<Box<ListNode>> {
        if list1 == None {
            return list2;
        }
        if list2 == None {
            return list1;
        }
        let ref2ptr_box = |x: &Box<ListNode>| unsafe {
            let w = x.as_ref();
            *((&raw const w) as *mut *mut ListNode)
        };
        let ref2ptr_option = |x: &Option<Box<ListNode>>| unsafe {
            Self::ref2ptr::<ListNode>(x.as_deref().unwrap_unchecked())
        };
        let mut a: *mut ListNode;
        let mut b: *mut ListNode;
        unsafe {
            a = ref2ptr_option(&list1);
            b = ref2ptr_option(&list2);
            let mut option_b = Self::ref2ptr(&list2);
            let r: Option<Box<ListNode>>;
            if (*a).val <= (*b).val {
                option_b = Self::ref2ptr(&list2);
                r = list1;
            } else {
                std::mem::swap(&mut a, &mut b);
                option_b = Self::ref2ptr(&list1);
                r = list2;
            }
            loop {
                //a is the latest successor,C and B are candidates
                let mut option_c = Self::ref2ptr(&(*a).next);
                println!("{:p}", option_c);
                let Some(next_a) = &(*a).next else {
                    (*a).next = (*option_b).clone();
                    break;
                };
                let c = ref2ptr_box(next_a);
                println!("{}<={}", (*c).val, (*b).val);
                if (*c).val <= (*b).val {
                    a = c;
                } else {
                    //b is new a
                    //a.next = b;
                    std::mem::swap(&mut (*a).next, &mut *option_b);
                    a = b;
                    b = c;
                }
            }
            r
        }
    }
}
fn print_list(l: &Option<Box<ListNode>>) {
    print!("[");
    let mut x = l;
    loop {
        match x {
            None => break,
            Some(n) => {
                print!("{:?},", n.val);
                x = &n.next;
            }
        }
    }
    println!("]");
}
fn print_list_list_node(n: &ListNode) {
    print!("[");
    print!("{:?},", n.val);
    let mut x = &n.next;
    loop {
        match x {
            None => break,
            Some(n) => {
                print!("{:?},", n.val);
                x = &n.next;
            }
        }
    }
    println!("]");
}
fn create_chain(v: &Vec<i32>) -> Option<Box<ListNode>> {
    let mut head: Option<Box<ListNode>> = None;
    let mut a = &mut head;
    for i in v.iter() {
        a.replace(Box::new(ListNode {
            val: i.clone(),
            next: None,
        }));
        a = &mut a.as_mut().unwrap().next;
    }
    head
}
fn get_all_addr(l: &Option<Box<ListNode>>) -> Vec<usize> {
    let mut a: Vec<usize> = Vec::new();
    let mut x = l;
    loop {
        match x {
            None => break,
            Some(n) => unsafe {
                let ptr = &raw const n;
                let o = *(ptr as *mut *mut Box<ListNode>);
                a.push(o as usize);
                x = &n.next;
            },
        }
    }
    a
}
fn main() {
    //Input: list1 = [1,2,4], list2 = [1,3,4]
    //Output: [1,1,2,3,4,4]
    let list1 = create_chain(&vec![1, 2, 5, 5]);
    let list2 = create_chain(&vec![1, 3, 4]);
    let x = Solution::merge_two_lists(list1, list2);
    print_list(&x);
}

// loop { //workable but bugged
//     //a is the latest successor,C and B are candidates
//     match &(*a).next {
//         None => {
//             (*a).next = Option::from(Box::from_raw(b));
//             break;
//         }
//         Some(next_a) => {
//             let c: *mut ListNode = Box::into_raw(next_a.to_owned());
//             if next_a.val <= (*b).val {
//                 a = c;
//             } else {
//                 (*a).next = Option::from(Box::from_raw(b));
//                 a = b;
//                 b = c;
//             }
//         }
//     }
// }
// return r;

// let list1 = Option::from(Box::new(ListNode {
//     val: 1,
//     next: Option::from(Box::new(ListNode {
//         val: 2,
//         next: Option::from(Box::new(ListNode { val: 5, next: None })),
//     })),
// }));
// let list2 = Option::from(Box::new(ListNode {
//     val: 1,
//     next: Option::from(Box::new(ListNode {
//         val: 3,
//         next: Option::from(Box::new(ListNode { val: 4, next: None })),
//     })),
// }));
