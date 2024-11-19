pub fn is_palindrome(x: i32) -> bool {
    if x < 0 { return false; }
    if x < 10 { return true; }
    let str = x.to_string();
    let buffer = str.as_bytes();
    let len = buffer.len();
    let halflen = len / 2;
    let mut i = 0;
    let mut j = len - 1;
    while i < halflen {
        if buffer[i] != buffer[j] { return false; }
        i += 1;
        j -= 1;
    }
    true
}