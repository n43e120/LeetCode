#!/usr/bin/python3
import re
class Solution:
    def myAtoi(self, s: str) -> int:
        matchObj = re.match(r"^\s*?([+-]?\d+)", s, re.M|re.I)
        #matchObj = re.match(r"^\D*?([+-]?\d+)", s, re.M|re.I) #support "words and 987"=987
        #print ("case:'",s,"'")
        if matchObj:
           #print ("matchObj.group(1) : ", matchObj.group(1))
           s=matchObj.group(1)
           i=int(s)
           if i > (2**31-1):
               i=(2**31-1)
           elif i < (-2**31):
               i=(-2**31)
           return i
        else:
           #print ("No match!!")
            return 0


import unittest

x = Solution()

class TestStringMethods(unittest.TestCase):
    
    def test_normal(self):
        self.assertEqual(x.myAtoi("42"), 42)

    def test_space(self):
        self.assertEqual(x.myAtoi(" 42 "), 42)
        
    def test_neg(self):
        self.assertEqual(x.myAtoi("-1"), -1)

    def test_plus(self):
        self.assertEqual(x.myAtoi("+1"), 1)

    def test_noise1(self):
        self.assertEqual(x.myAtoi("1337c0d3"), 1337)

    def test_noise2(self):
        self.assertEqual(x.myAtoi("0-1"), 0)
        
    def test_noise3(self):
        self.assertEqual(x.myAtoi("words and 987"), 0)

    def test_rounding_1(self):
        self.assertEqual(x.myAtoi("-91283472332"), -2147483648)

    def test_rounding_2(self):
        self.assertEqual(x.myAtoi("+99999999999"), 2147483647)

if __name__ == "__main__":
    unittest.main()