struct Solution;
impl Solution {
    pub fn roman_num_value(i: u32) -> i32 {
        10i32.pow(i >> 1) * (5i32.pow(i & 1))
    }
    fn pop_I (mut ss: &str, c: char) -> usize {
        let len_a = ss.len();
        ss = ss.trim_end_matches(c);
        let len_b = ss.len();
        (len_a - len_b)
    }
    fn pop_IX (mut ss: &str, ix: &str)  -> usize {
        let len_a = ss.len();
        ss = ss.trim_end_matches(ix);
        let len_b = ss.len();
        (len_a - len_b)
    }
    pub fn roman_to_int(s: String) -> i32 {
        let mut sum: i32 = 0;
        let M = "IVXLCDM";
        //let mut len = s.len();
        match s.len() {
            0 => return 0,
            1 => {
                return match M.find(s.as_bytes()[0] as char) {
                    None => 0,
                    Some(i) => {
                        Solution::roman_num_value(i as u32)
                    }
                }
            }
            _ => {}
        }
        let mut ss = &s[..];
        let mut i: i32 = 0;
        loop {
            let ten_pow = 10i32.pow((i >> 1) as u32);
            let I: char = M.as_bytes()[i as usize] as char;
            let V: char = M.as_bytes()[(i + 1) as usize] as char;
            let X: char = M.as_bytes()[(i + 2) as usize] as char;
            let IX = format!("{}{}", I, X);
            let mut cnt = Self::pop_I(ss, I); //'I'
            if cnt > 0 {
                ss = &ss[..ss.len() - cnt];
                sum += cnt as i32 * ten_pow;
                if ss.ends_with(V) { //'V'
                    ss = &ss[..ss.len() - 1];
                    sum += 5 * ten_pow;
                } else if ss.ends_with(X) { //'X'
                    ss = &ss[..ss.len() - 1];
                    sum += 10 * ten_pow;
                }
            } else {
                if ss.ends_with(V) {
                    ss = &ss[..ss.len() - 1];
                    cnt = Self::pop_I(ss, I);
                    if cnt > 0 {
                        ss = &ss[..ss.len() - cnt];
                    }
                    sum += (5 - cnt as i32) * ten_pow;
                } else {
                    cnt = Self::pop_IX(ss, IX.as_str());
                    if cnt > 0 {
                        ss = &ss[..ss.len() - 2];
                        sum += 9 * ten_pow;
                    }
                }
            }
            if ss.len() ==0 {
                break;
            }
            if i >= 4 {
                cnt = Self::pop_I(ss, 'M');
                if cnt > 0 {
                    sum += cnt as i32 * 1000;
                    break;
                }
                break;
            }
            i += 2;
        }
        sum
    }

    pub fn nonstandard_roman_to_int_recur(ss: &str, mut i_c: i32) -> i32 {
        let f = Self::nonstandard_roman_to_int_recur;
        let M = "IVXLCDM";
        let len = ss.len();
        if len == 0 {
            return 0;
        }
        loop {
            if i_c < 0 {
                return 0;
            }
            let c = M.as_bytes()[i_c as usize];
            match ss.find(c as char) {
                Some(idx_first) => {
                    let idx_last = ss.len() - 1;
                    let mut j = idx_first + 1;
                    loop {
                        if j > idx_last {
                            break;
                        }
                        if ss.as_bytes()[j] == c {
                            j += 1;
                            continue;
                        }
                        break;
                    }
                    let cnt = (j - idx_first) as i32;
                    let mut sum = cnt * Solution::roman_num_value(i_c as u32);
                    if j > idx_last { //no goto in Rust
                        if idx_first == 0 {
                            //in case "VVV"
                        } else {
                            // case "...VV"
                            sum -= f(&ss[..idx_first], i_c - 1);
                        }
                    } else {
                        if idx_first == 0 { //in case "VVV..."
                            sum += f(&ss[j..], i_c - 1);
                        } else {
                            //like case "IVI" invalid Roman number
                            sum -= f(&ss[..idx_first], i_c - 1);
                            sum += f(&ss[j..], i_c - 1);
                        }
                    }
                    return sum;
                }
                _ => {
                    i_c -= 1;
                }
            }
        }
    }
    pub fn nonstandard_roman_to_int(s: String) -> i32 {
        let M = "IVXLCDM";
        let len = s.len();
        match len {
            0 => return 0,
            1 => {
                return match M.find(s.as_bytes()[0] as char) {
                    None => 0,
                    Some(i) => {
                        Solution::roman_num_value(i as u32)
                    }
                }
            }
            _ => {}
        }
        return Self::nonstandard_roman_to_int_recur(&s[..], (M.len() as i32) - 1);
    }
}
#[cfg(test)]
mod tests {
    use crate::Solution;
    #[test]
    pub fn convension() {
        // I             1
        // V             5
        // X             10
        // L             50
        // C             100
        // D             500
        // M             1000
        let f = Solution::roman_to_int;
        let data = [
            ("I", 1),
            ("V", 5),
            ("X", 10),
            ("L", 50),
            ("C", 100),
            ("D", 500),
            ("M", 1000),
            ("III", 3),
            ("IV", 4),
            ("LVIII", 58),
            ("MCMXCIV", 1994) //1994= M +CM + XC +IV
        ];
        for datum in data {
            let x = f(datum.0.to_string());
            assert_eq!(x, datum.1);
        }
    }
    #[test]
    pub fn nonstandard() {
        let f = Solution::nonstandard_roman_to_int;
        let data = [
            ("IIIIIV", 0),
            ("IV", 4),
            ("VI", 6),
            ("VIMM", 1994), //not 1996, 1994=MM-VI, 1996=MM-V+I which is wrong
        ];
        for datum in data {
            let x = f(datum.0.to_string());
            assert_eq!(x, datum.1);
        }
    }
}

fn main() {
    //let f = Solution::nonstandard_roman_to_int;
    // let x = f("MCMXCIV".to_string());
    // println!("{}", x);

    let f = Solution::roman_to_int;
    let x = f("LVIII".to_string());
    println!("{}", x);

    // let mut s = String::from("VIII");
    // let b = s.trim_end_matches("I");
    // println!("{}", b);

    // use crate::tests as test1;
    // test1::nonstandard();
}
