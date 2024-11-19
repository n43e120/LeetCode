pub struct Solution;
impl Solution {
    pub fn two_sum(aa: Vec<i32>, target: i32) -> Vec<i32> {
        use std::collections::HashMap;
        let mut m: HashMap<i32, i32> = HashMap::new();
        let len = aa.len();
        let mut i = (len - 1) as i32;
        let mut a: i32 = aa[i as usize];
        if target & 1 == 0 { //target is even
            let half_target = target >> 1;
            if a == half_target { //for edge case when arr=[...,4,4] target=8
                return vec![i, i + 1];
            }
            loop {
                m.insert(a, i); //ignore duplicated value
                i -= 1;
                a = aa[i as usize];
                if a > half_target {
                    continue;
                }
                if a == half_target {
                    if aa[(i - 1) as usize] == a {
                        return vec![i, i + 1];
                    }
                    //discharge a
                    i -= 1;
                    a = aa[i as usize];
                } else {
                    //a < target
                }
                break;
            }
        } else {
            loop {
                m.insert(a, i);
                i -= 1;
                a = aa[i as usize];
                if a << 1 > target {
                    continue;
                }
                break;
            }
        }
        loop {
            let b = target - a;
            if m.contains_key(&b) {
                return vec![i + 1, m.get(&b).unwrap().clone() + 1];
            }
            i -= 1;
            if i < 0 {
                break;
            }
            a = aa[i as usize];
        }
        Vec::new()
    }
}

fn main() {
    let aa = vec![2, 7, 11, 15];
    let target = 9;

    // let aa = vec![2,3,4];
    // let target = 6;

    // let aa = vec![1, 2, 3, 4, 4, 9, 56, 90];
    // let target = 8;

    let aa = vec![1, 3, 4, 4];
    let target = 8;

    let b = Solution::two_sum(aa, target);
    print!("[");
    for i in b.iter() {
        print!("{},", i);
    }
    println!("]");
}
