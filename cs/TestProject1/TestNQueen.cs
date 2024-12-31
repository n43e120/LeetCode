
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
			NativeOp<ulong>.SetBitAt(&x, i);
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
			NativeOp<ulong>.SetBitRange(&x, i, cnt);
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
			return NativeOp<ulong>.NextOneBit(&bitmap, i);
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
			return NativeOp<ulong>.PrevOneBit(&bitmap, i);
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
			return NativeOp<ulong>.NextZeroBit(&bitmap, i);
		};
		T(f(0UL, 0), 1);
		T(f(0, 63), -1);
		T(f(0, 62), 63);
		T(f(ulong.MaxValue, 0), -1);
	}
	[TestMethod]
	public void TestRotate180()
	{
		var f = NativeOp<ulong>.Rotate180;
		T(f(1ul, 64), 1ul << 63);
		T(f(1ul << 63, 64), 1ul);
		T(f(1ul << 7, 64), 1ul << 63 - 7);
		T(f(1ul << 63 - 7, 64), 1ul << 7);
	}
	[TestMethod]
	public void TestFlipX()
	{
		var f = NativeOp<ulong>.FlipX;
		T(f(1ul, 8, 8), 1ul << 7);
		T(f(1ul << 7, 8, 8), 1ul);
		T(f(1ul << 63, 8, 8), 1ul << 63 - 7);
	}
	[TestMethod]
	public void TestRotate90()
	{
		var f = NativeOp<ulong>.Rotate90;
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
		var f = NativeOp<ulong>.Rotate270;
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
		var f = NativeOp<int>.ConvertNativeToQueenPosArray;
		T(f(0), []);
		T(f(1), [0]);
		T(f(3), [0, 1]);
	}
	[TestMethod]
	public void TestBitToIndice()
	{
		var f = NativeOp<int>.ConvertQueenPosArrayToNative;
		T(f([]), 0);
		T(f([0]), 1);
		T(f([0, 1]), 3);
	}
}


[TestClass]
public unsafe sealed class TestMultiBlockOp
{
	[TestMethod]
	public void TestSetBitRange()
	{
		var fExpect = (ulong x, int i, int cnt) =>
		{
			NativeOp<ulong>.SetBitRange(&x, i, cnt);
			return NQueenData.BitMapToString(&x, 8, 8);
		};
		var fActual = (ulong x, int i, int cnt) =>
		{
			MultiBlockOp<byte>.SetBitRange((byte*)&x, i, cnt);
			return NQueenData.BitMapToString(&x, 8, 8);
		};
		for (int iRow = 7; iRow < 8; iRow++)
		{
			T(fActual(0, iRow * 8, 8), fExpect(0, iRow * 8, 8));
		}
	}
}
[TestClass]
public sealed unsafe class Test_SparseBitArrayQueen
{
	[TestMethod]
	public void Test_R180()
	{
		var f = (int x) =>
		{
			int y;
			SparseBitArrayQueen.R180((nint)(&x), 8, 8, (nint)(&y), 1);
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
	public void TestMethod1()
	{
		var x = new Solution_leetCode();
		var f = x.SolveNQueens;
		var a = f(4);
	}
	[TestMethod]
	public void Test1()
	{
		var n = 4;
		var qs = new NQueenData((uint)n, (uint)n, 4);
		unsafe
		{
			var f = (ulong bitmap, int i) =>
			{
				NativeOp<ulong>.PrintQueenSight(qs, &bitmap, i);
				return NQueenData.BitMapToString((byte*)&bitmap, qs.iWidth, qs.iHeight);
			};
			ulong data = 0;
			var a = f(data, 0);
		}

	}
}