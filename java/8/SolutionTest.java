import static org.junit.jupiter.api.Assertions.*;

//following 视频教程 https://www.bilibili.com/video/BV1Wn4y197d4/
//To run this test, find Packages Tab among Project and Tests ..
//Right click, select Run Test
class SolutionTest {
    @org.junit.jupiter.api.Test
    void myAtoi() {
        var x = new Solution();
        assertAll("Test for LeetCode Problem 8. String to Integer (atoi)",
                ()->assertEquals(42, x.myAtoi("42")),
                ()->assertEquals(42, x.myAtoi(" 42 ")),
                ()->assertEquals(-1, x.myAtoi("-1")),
                ()->assertEquals(1, x.myAtoi("+1")),
                ()->assertEquals(1337, x.myAtoi("1337c0d3")),
                ()->assertEquals(0, x.myAtoi("0-1")),
                ()->assertEquals(0, x.myAtoi("words and 987"),"start with non-digit should be 0"),
                ()->assertEquals(-2147483648, x.myAtoi("-91283472332")),
                ()->assertEquals(2147483647, x.myAtoi("+99999999999")),
                ()->assertEquals(12345678, x.myAtoi("  0000000000012345678")), //java error, python ok
                ()->assertEquals(2147483647, x.myAtoi("21474836460")) //java error, python ok

        );
    }
}