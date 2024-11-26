class Solution:
	def convert(self, s: str, numRows: int) -> str:
		lenTotal = len(s)
		if lenTotal <= 2:
			return s
		if numRows == 1:
			return s
		idxLastLine = numRows - 1
		period: int = numRows * 2 - 1 - 1
		r: str = ""
		for iLine in range(0, numRows):
			i = iLine
			if i == 0 or i == idxLastLine:
				while i < lenTotal:
					r += s[i]
					i += period
			else:
				offSetConj: int = 2 * (idxLastLine - i)  # conjugation
				while i < lenTotal:
					r += s[i]
					iconj = i + offSetConj
					if iconj >= lenTotal:
						break
					r += s[iconj]
					i += period
		return r


import unittest

x = Solution()
f = x.convert


class TestStringMethods(unittest.TestCase):
	def test_normal(self):
		self.assertEqual("PAHNAPLSIIGYIR", f("PAYPALISHIRING", 3))

	def test_neg(self):
		self.assertEqual("PINALSIGYAHRPI", f("PAYPALISHIRING", 4))

	def test_oneChar(self):
		self.assertEqual("A", f("A", 1))

	def test_oneline(self):
		self.assertEqual("ABC", f("ABC", 1))


if __name__ == "__main__":
	unittest.main()
