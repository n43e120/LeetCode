class Solution:
	M: int = 2 ** 31 - 1
	N: int = -2 ** 31

	def reverse(self, x: int) -> int:
		if x == 0:
			return x
		s = str(x).rstrip("0")[::-1]
		n: int
		if x < 0:
			n = -int(s[:-1])
			if n < self.N:
				return 0
			return n
		n = int(s)
		if n > self.M:
			return 0
		return n


import unittest

x = Solution()


class TestStringMethods(unittest.TestCase):
	def test_normal(self):
		self.assertEqual(x.reverse(123), 321)

	def test_neg(self):
		self.assertEqual(x.reverse(-123), -321)

	def test_leading_zero(self):
		self.assertEqual(x.reverse(120), 21)

	def test_zero(self):
		self.assertEqual(x.reverse(0), 0)

	def test_outrange(self):
		self.assertEqual(x.reverse(1534236469), 0)


if __name__ == "__main__":
	unittest.main()
