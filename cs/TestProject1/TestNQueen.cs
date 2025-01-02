
using NQueen;
namespace TestProject1;
[TestClass]
public unsafe sealed class TestNativeOp
{
	[TestMethod]
	public void TestSetBitAt()
	{
		var f = (ulong x, int i) =>
		{
			NativeBitmap<ulong>.SetBitAt(&x, i);
			return x;
		};
		T(f(0, 0), 1ul);
		T(f(0, 1), 2ul);
		T(f(0, 3), 8ul);
	}
	[TestMethod]
	public void TestSetBitRange()
	{
		var f = (ulong x, int i, int cnt) =>
		{
			NativeBitmap<ulong>.SetBitRange(&x, i, cnt);
			return x;
		};
		T(f(0, 0, 2), 3ul);
		T(f(0, 1, 2), 6ul);
		T(f(0, 2, 2), 12ul);
	}
	[TestMethod]
	public void Test_NextOneBit()
	{
		var f = (ulong bitmap, int i) =>
		{
			return NativeBitmap<ulong>.NextOneBit(&bitmap, i);
		};
		T(f(1, -1), 0);
		T(f(0, -1), -1);
		T(f(0, 0), -1);
		T(f(1 << 1, 0), 1);
		T(f(0, 1), -1);
		T(f(0, 63), -1);
		T(f(1ul << 63, 62), 63);
		T(f(1ul << 63, 63), -1);
	}
	[TestMethod]
	public void Test_PrevOneBit()
	{
		var f = (ulong bitmap, int i) =>
		{
			return NativeBitmap<ulong>.PrevOneBit(&bitmap, i);
		};
		//T(f(0, 0), -1);
		//T(f(1, 1), 0);
		T(f(ulong.MaxValue, 63), 62);
		T(f(0, 63), -1);
	}
	[TestMethod]
	public void Test_NextZeroBit()
	{
		var f = (ulong bitmap, int i) =>
		{
			return NativeBitmap<ulong>.NextZeroBit(&bitmap, i);
		};
		T(f(0UL, 0), 1);
		T(f(0, 63), -1);
		T(f(0, 62), 63);
		T(f(ulong.MaxValue, 0), -1);
	}
	[TestMethod]
	public void TestRotate180()
	{
		var f = NativeBitmap<ulong>.Rotate180;
		T(f(1ul, 64), 1ul << 63);
		T(f(1ul << 63, 64), 1ul);
		T(f(1ul << 7, 64), 1ul << 63 - 7);
		T(f(1ul << 63 - 7, 64), 1ul << 7);
	}
	[TestMethod]
	public void TestFlipX()
	{
		var f = NativeBitmap<ulong>.FlipX;
		T(f(1ul, 8, 8), 1ul << 7);
		T(f(1ul << 7, 8, 8), 1ul);
		T(f(1ul << 63, 8, 8), 1ul << 63 - 7);
	}
	[TestMethod]
	public void TestRotate90()
	{
		var f = NativeBitmap<ulong>.Rotate90;
		T(f(1ul, 3, 2), 1ul << 4);
		T(f(2ul, 3, 2), 1ul << 2);

		T(f(1ul, 8, 8), 1ul << 63 - 7);
		T(f(1ul << 63 - 7, 8, 8), 1ul << 63);
		T(f(1ul << 63, 8, 8), 1ul << 7);
		T(f(1ul << 7, 8, 8), 1ul);
	}
	[TestMethod]
	public void TestRotate270()
	{
		var f = NativeBitmap<ulong>.Rotate270;
		T(f(1ul, 3, 2), 1ul << 1);
		T(f(1ul << 2, 3, 2), 1ul << 5);

		T(f(1ul, 8, 8), 1ul << 7);
		T(f(1ul << 7, 8, 8), 1ul << 63);
		T(f(1ul << 63, 8, 8), 1ul << 63 - 7);
		T(f(1ul << 63 - 7, 8, 8), 1ul);
	}
	[TestMethod]
	public void TestIndiceToBit()
	{
		var f = NativeBitmap<int>.ToIndexArray;
		T(f(0), []);
		T(f(1), [0]);
		T(f(3), [0, 1]);
	}
	[TestMethod]
	public void TestBitToIndice()
	{
		var f = NativeBitmap<int>.FromIndexArray;
		T(f([]), 0);
		T(f([0]), 1);
		T(f([0, 1]), 3);
	}
}


[TestClass]
public unsafe sealed class TestMultiBlockOp
{
	[TestMethod]
	public void TestSetBitRange_8()
	{
		for (int i = 5; i < 8; i++)
		{
			var w = i;
			var h = i;
			var fExpect = (ulong x, int i, int cnt) =>
			{
				NativeBitmap<ulong>.SetBitRange(&x, i, cnt);
				return NQueenDebugger.BitMapToString(&x, w, h);
			};
			var fActual = (ulong x, int i, int cnt) =>
			{
				MultiBlockBitmap<byte>.SetBitRange((byte*)&x, i, cnt);
				return NQueenDebugger.BitMapToString(&x, w, h);
			};
			for (int iRow = 0; iRow < h; iRow++)
			{
				T(fActual(0, iRow * w, w), fExpect(0, iRow * w, w), $"n:{i},iRow:{iRow}");
			}
		}
	}
	[TestMethod]
	public void TestSetBitRange_9()
	{
		var i = 9;
		var w = i;
		var h = i;
		var fExpect = (ulong x, int i, int cnt) =>
		{
			NativeBitmap<ulong>.SetBitRange(&x, i, cnt);
			return NQueenDebugger.BitMapToString(&x, w, h);
		};
		var fActual = (ulong x, int i, int cnt) =>
		{
			MultiBlockBitmap<byte>.SetBitRange((byte*)&x, i, cnt);
			return NQueenDebugger.BitMapToString(&x, w, h);
		};
		for (int iRow = 0; iRow < h; iRow++)
		{
			T(fActual(0, iRow * w, w), fExpect(0, iRow * w, w), $"n:{i},iRow:{iRow}");
		}
	}
	[TestMethod]
	public void TestNextZeroBit()
	{
		var fExpect = (ulong x, int i) =>
		{
			NativeBitmap<ulong>.NextZeroBit(&x, i);
			return NQueenDebugger.BitMapToString(&x, 8, 8);
		};
		var fActual = (ulong x, int i, int cntBlocksPerBitmap) =>
		{
			MultiBlockBitmap<byte>.NextZeroBit((byte*)&x, i, cntBlocksPerBitmap);
			return NQueenDebugger.BitMapToString(&x, 8, 8);
		};
		void f(int iBit, int iBitAnchor)
		{
			ulong x = 1ul << iBitAnchor;
			var a = fActual(x, iBit, sizeof(ulong) / sizeof(byte));
			var e = fExpect(x, iBit);
			T(a, e, $"{iBitAnchor},{iBit}");
		}
		for (int iBitAnchor = 0; iBitAnchor < 64; iBitAnchor++)
		{
			for (int iBit = 0; iBit < 64; iBit++)
			{
				f(iBit, iBitAnchor);
			}
		}
	}
}
[TestClass]
public sealed unsafe class Test_SparseBitArrayQueen
{
	[TestMethod]
	public void Test_FromQueens()
	{
		int[] aa = [3, 14, 20, 35, 37, 52, 58, 69, 72];
		var f = (int[] x) =>
		{
			var cntQ = x.Length;
			var ba = XYTupleByteArray.FromIndexArray(x, cntQ);
			var y = XYTupleByteArray.ToIndexArray(ba, cntQ, cntQ);
			S(x, y);
		};
		f(aa);
	}
	[TestMethod]
	public void Test_Sort()
	{
		int[] aa = [3, 14, 20, 35, 37, 52, 58, 69, 72];
		//"[(3,0),(5,1),(2,2),(8,3),(1,4),(7,5),(4,6),(6,7),(0,8)]"
		var f = (int[] x) =>
		{
			var cntQ = x.Length;
			var ba = XYTupleByteArray.FromIndexArray(x, cntQ);
			var str1 = XYTupleByteArray.ToDebugString(ba, cntQ, cntQ);
			XYTupleByteArray.Sort(ba, cntQ, cntQ);
			var str2 = XYTupleByteArray.ToDebugString(ba, cntQ, cntQ);
			T(str1, str2);
		};
		f(aa);
	}
	[TestMethod]
	public void Test_R180()
	{
		var f = (int x) =>
		{
			int y;
			XYTupleByteArray.R180((nint)(&x), 8, 8, (nint)(&y), 1);
			return (byte)y;
		};
		T(f(0), 7 << 4 | 7);
		T(f(7 << 4 | 7), 0);
		T(f(7), 7 << 4);
		T(f(7 << 4), 7);
	}
}

[TestClass]
public sealed class TestNQueen
{
	[TestMethod]
	public void Test_5()
	{
		var x = new Solution();
		var f = x.SolveNQueens;
		var actual = f(5);
		IList<IList<string>> expect = [
		["Q....", "..Q..", "....Q", ".Q...", "...Q."],
		["Q....", "...Q.", ".Q...", "....Q", "..Q.."],
		[".Q...", "...Q.", "Q....", "..Q..", "....Q"],
		[".Q...", "....Q", "..Q..", "Q....", "...Q."],
		["..Q..", "Q....", "...Q.", ".Q...", "....Q"],
		["..Q..", "....Q", ".Q...", "...Q.", "Q...."],
		["...Q.", "Q....", "..Q..", "....Q", ".Q..."],
		["...Q.", ".Q...", "....Q", "..Q..", "Q...."],
		["....Q", ".Q...", "...Q.", "Q....", "..Q.."],
		["....Q", "..Q..", "Q....", "...Q.", ".Q..."]
		];
		//IList<IList<string>> actual = [
		//["Q....", "..Q..", "....Q", ".Q...", "...Q."],
		//["..Q..", "....Q", ".Q...", "...Q.", "Q...."],
		//[".Q...", "...Q.", "Q....", "..Q..", "....Q"],
		//["....Q", ".Q...", "...Q.", "Q....", "..Q.."],
		//["....Q", "..Q..", "Q....", "...Q.", ".Q..."],
		//["Q....", "...Q.", ".Q...", "....Q", "..Q.."],
		//["...Q.", ".Q...", "....Q", "..Q..", "Q...."],
		//["..Q..", "Q....", "...Q.", ".Q...", "....Q"]
		//];
		(var notInExpect, var notInActual) = SetListDiffer(expect, actual);
		if (notInActual.Count != 0 || notInExpect.Count != 0)
		{
			Print(notInActual);
			Print(notInExpect);
			Assert.Fail();
		}
	}

	[TestMethod]
	[DataRow(1, 1)]
	[DataRow(2, 0)]
	[DataRow(3, 0)]
	[DataRow(4, 2)]
	[DataRow(5, 10)]
	[DataRow(6, 4)]
	[DataRow(7, 40)]
	[DataRow(8, 92)]
	[DataRow(9, 352)]
	public void Test_n(int n, int cntSolutions){
		var x = new Solution();
		var f = x.SolveNQueens;
		var actual = f(8);
		Assert.AreEqual(92, actual.Count);
	}
}