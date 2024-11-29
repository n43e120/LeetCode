struct Solution {}
impl Solution {
    pub fn search(nums: Vec<i32>, target: i32) -> i32 {
        const NONEXIST: i32 = -1;
        let nums_size = nums.len();
        let mut a: i32 = 0;
        let mut b: i32 = (nums_size - 1) as i32;
        let mut i: i32;
        loop {
            match (b - a) as i32 {
                1 => {
                    if nums[b as usize] == target {
                        return b as i32;
                    }
                }
                0 => {
                    if nums[a as usize] == target {
                        return a as i32;
                    }
                }
                (-1i32) => {
                    return NONEXIST;
                }
                _ => println!("Rest of the number"),
            }
            i = (b + a) >> 1;
            let e = nums[i as usize];
            if e < target {
                a = i + 1;
            } else if e > target {
                b = i - 1;
            } else {
                break;
            }
        }
        return i as i32;
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn element_exist() {
        let v = vec![-1, 0, 3, 5, 9, 12];
        let result = Solution::search(v, 9);
        assert_eq!(result, 4);
    }
    #[test]
    fn element_nonexist() {
        let v = vec![-1, 0, 3, 5, 9, 12];
        let result = Solution::search(v, 2);
        assert_eq!(result, -1);
    }
}

fn main() {}
