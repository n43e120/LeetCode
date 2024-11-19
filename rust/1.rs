pub fn two_sum(nums: Vec<i32>, target: i32) -> Vec<i32> {
    let len = nums.len();
    let mut i = (len - 1) as i32;
    while i > 0 {
        let mut j = i - 1;
        while j >= 0 {
            if nums[i as usize] + nums[j as usize] == target {
                return vec![i, j];
            }
            j -= 1;
        }
        i -= 1;
    }
    Vec::new()
}

//O(n)
pub fn two_sum(nums: Vec<i32>, target: i32) -> Vec<i32> {
    use std::collections::HashMap;
    let mut m: HashMap<i32, i32> = HashMap::new();
    let len = nums.len();
    let mut i = (len - 1) as i32;
    m.insert(nums[i as usize], i);
    i -= 1;
    while i >= 0 {
        let a = nums[i as usize];
        let b = target - a;
        if m.contains_key(&b) {
            return vec![i, m.get(&b).unwrap().clone()];
        }
        m.insert(a, i);
        i -= 1;
    }
    Vec::new()
}