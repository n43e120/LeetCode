using System.Collections;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
namespace NQueen;
/// <summary>
/// Operate on single variable of native integer type
/// </summary>
/// <typeparam name="T"></typeparam>
public unsafe static class NativeOp<T> where T : unmanaged, IBinaryInteger<T>//IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>, INumberBase<T>
{
	//public readonly static Func<T, byte[]> fConvert_T_To_Bytes;
	static NativeOp()
	{
		//fConvert_T_To_Bytes = typeof(BitConverter).GetMethod("GetBytes", [typeof(T)]).CreateDelegate<Func<T, byte[]>>();
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetBitAt(T* p, int iBit) => *p |= T.One << iBit;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetBitRange(T* p, int iBitStart, int cnt)
	{
		if (iBitStart + cnt >= cntBit_T)
		{
			*p |= T.Zero - (T.One << iBitStart);
		}
		else
		{
			*p |= (T.One << iBitStart + cnt) - (T.One << iBitStart);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ClearBitRange(T* p, int iBitStart, int cnt) => *p &= ~((T.One << iBitStart + cnt) - (T.One << iBitStart));
	public static int cntBit_T = sizeof(T) * 8;
	public static int PrevOneBit(void* ptr, int iBitCur)
	{
		if (iBitCur < 1) return -1;
		T MASK = iBitCur < (cntBit_T) ? (T.One << (iBitCur)) - T.One : T.Zero - T.One;
		var dst = (T*)ptr;
		var r = T.LeadingZeroCount(*dst & MASK);
		var i = (cntBit_T - 1) - *(int*)&r;
		//if (i >= iBitCur) return -1;
		return i;
	}
	public static int PrevZeroBit(void* ptr, int iBitCur)
	{
		var x = ~*(T*)ptr;
		return PrevOneBit(&x, iBitCur);
		//var dst = (T*)ptr;
		////if (iBitCur == 0) return -1;
		//byte xx = (byte)((1 << (iBitCur + 1)) - 1);
		//var r = T.LeadingZeroCount((byte)(~(*dst | ~xx)));
		//var i = Z - 1 - (int)*(byte*)&r;
		////if (i >= iBitCur) return -1;
		//return i;
	}
	public static int NextOneBit(void* ptr, int iBitCur)
	{
		if (iBitCur >= cntBit_T - 1) return -1;
		T MASK = (T.One << (iBitCur + 1)) - T.One;
		var dst = (T*)ptr;
		var r = T.TrailingZeroCount(*dst & ~MASK);
		var i = *(int*)&r;
		if (i == cntBit_T) return -1;
		return i;
	}
	public static int NextZeroBit(void* ptr, int iBitCur)
	{
		var x = ~*(T*)ptr;
		return NextOneBit(&x, iBitCur);
	}
	public static T Rotate180(T x, int len_bit)
	{
		//ReverseBits
		T y = default;
		int idxLast = len_bit - 1;
		for (int i = 0; i < len_bit; i++)
		{
			y |= (x & T.One) << (idxLast - i);
			x >>= 1;
		}
		return y;
	}
	public static T FlipX(T x, int w, int h)
	{
		T y = default;
		int lastCol = w - 1;
		for (int iRow = 0; iRow < h; iRow++)
		{
			for (int iCol = 0; iCol < w; iCol++)
			{
				y |= (x & T.One) << (iRow * w + lastCol - iCol);
				x >>= 1;
			}
		}
		return y;
	}
	public static T Rotate90(T x, int w, int h)
	{//anti-clockwise
		T y = default;
		int lastCol = w - 1;
		for (int iRow = 0; iRow < h; iRow++)
		{
			for (int iCol = 0; iCol < w; iCol++)
			{
				y |= (x & T.One) << ((lastCol - iCol) * h + iRow);
				x >>= 1;
			}
		}
		return y;
	}
	public static T Rotate270(T x, int w, int h)
	{
		T y = default;
		int lastRow = h - 1;
		for (int iRow = 0; iRow < h; iRow++)
		{
			for (int iCol = 0; iCol < w; iCol++)
			{
				y |= (x & T.One) << (iCol * h + (lastRow - iRow));
				x >>= 1;
			}
		}
		return y;
	}

	public static void PrintBackslash(void* bitmap, int r, int c, int iWidth, int iHeight)
	{
		var x = (T*)bitmap;
		do
		{
			*x |= T.One << (r * iWidth) + c;
			r++;
			if (r >= iHeight) break;
			c++;
			if (c >= iWidth) break;
		} while (true);
	}
	public static void PrintSlash(void* bitmap, int r, int c, int iWidth, int iHeight)
	{
		var x = (T*)bitmap;
		do
		{
			*x |= T.One << (r * iWidth) + c;
			r++;
			if (r >= iHeight) break;
			c--;
			if (c < 0) break;
		} while (true);
	}

	internal static void PrintQueenSight(NQueenData qs, void* pBitmap, int iBit)
	{
		var p = (T*)pBitmap;
		(var iRow, var iCol) = Math.DivRem(iBit, qs.iWidth);
		*p |= ((T*)qs.H_LINES)[iRow];
		*p |= ((T*)qs.V_LINES)[iCol];
		*p |= ((T*)qs.BACK_SLASHES)[iCol + qs.iHeight - 1 - iRow];
		*p |= ((T*)qs.SLASHES)[iCol + iRow];
	}

	public static T ConvertQueenPosArrayToNative(int[] indice)
	{
		T y = default;
		foreach (var x in indice)
		{
			y |= T.One << x;
		}
		return y;
	}
	public static int[] ConvertNativeToQueenPosArray(T x)
	{
		List<int> indice = [];
		//var sz = Marshal.SizeOf(typeof(T));
		int i = 0;
		while (x != T.Zero)
		{
			if ((x & T.One) == T.One)
			{
				indice.Add(i);
			}
			x >>= 1;
			i++;
		};
		return [.. indice];
	}
}
public unsafe static class MultiBlockOp<T> where T : unmanaged, IBinaryInteger<T> // IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>, INumberBase<T>
{
	/// <summary>
	/// size of block in byte
	/// </summary>
	//public readonly static int SZ = sizeof(T);
	public readonly static int cntBits_BLOCK = sizeof(T) * 8;
	public static void SetBitAt(T* p, uint iBit)
	{
		(var q, var r) = Math.DivRem((int)iBit, cntBits_BLOCK);
		p[q] |= T.One << r;
	}
	public static bool GetBitAt(T* p, uint iBit)
	{
		(var q, var r) = Math.DivRem((int)iBit, cntBits_BLOCK);
		return (p[q] & T.One << r) != T.Zero;
	}
	public static void SetBitRange(T* p, int iBitStart, int cnt)
	{
		(var q, var iBit) = Math.DivRem(iBitStart, cntBits_BLOCK);
		if (iBit + cnt <= cntBits_BLOCK)
		{
			NativeOp<T>.SetBitRange(p + q, iBit, cnt);
			if (q == 0) return;
		}
		else
		{
			*p |= T.Zero - (T.One << iBit);
		}

		cnt -= cntBits_BLOCK - iBit;
		iBit = 0;
		while (cnt >= cntBits_BLOCK)
		{
			p++;
			p[q] |= T.Zero - T.One;
			cnt -= cntBits_BLOCK;
		}
		if (cnt == 0) return;
		p++;
		p[q] |= (T.One << iBit + cnt + 1) - T.One;
	}


	internal static void Generate_HLINES(NQueenData qs, void* p)
	{
		for (int iRow = 0; iRow < qs.iHeight; iRow++)
		{
			SetBitRange((T*)p, iRow * qs.iWidth, qs.iWidth);
			p = qs.NextBitmap(p);
		}
	}
	static void Bitmap_CopyShiftLeftOne(T* dstBlock0, T* srcBlock0, ref readonly int iHeight)
	{
		ref readonly var d = ref dstBlock0;
		ref readonly var s = ref srcBlock0;
		var i = iHeight - 1;
		d[i] |= s[i] << 1;
		for (i--; i >= 0; i--)
		{
			d[i] |= s[i] << 1;
			if ((s[i] >> (cntBits_BLOCK - 1)) == T.One)
			{
				d[i + 1] |= T.One;
			}
		}
	}
	internal static void Generate_VLINES(NQueenData qs, void* p)
	{
		ref readonly var w = ref qs.iWidth;
		ref readonly var h = ref qs.iHeight;

		var q = 0;
		var rem = 0;
		for (int iRow = 0; iRow < h; iRow++)
		{
			((T*)p)[q] |= T.One << rem;
			rem += w;
			if (rem >= cntBits_BLOCK)
			{
				q++;
				rem = 0;
			}
		}
		int iCol = 1;
	AGAIN:
		var p2 = qs.NextBitmap(p);
		Bitmap_CopyShiftLeftOne((T*)p2, (T*)p, h);
		iCol++;
		if (iCol >= w) return;

		p = qs.NextBitmap(p2);
		Bitmap_CopyShiftLeftOne((T*)p, (T*)p2, h);
		iCol++;
		if (iCol >= w) return;
		goto AGAIN;
	}
	static void PrintBackslash_multipleBlock(void* pBitmap, int r, int c, int iWidth, int iHeight)
	{
		do
		{
			SetBitAt((T*)pBitmap, (uint)((r * iWidth) + c));
			r++;
			if (r >= iHeight) break;
			c++;
			if (c >= iWidth) break;
		} while (true);
	}
	internal static void Generate_BACKSLASHES(NQueenData qs, void* p)
	{
		var f = PrintBackslash_multipleBlock;
		if (qs.SZ_BITMAP <= sizeof(T))
		{
			f = NativeOp<T>.PrintBackslash;
		}
		ref readonly var w = ref qs.iWidth;
		ref readonly var h = ref qs.iHeight;
		//new ulong[(iWidth + iHeight - 1) * LEN_BITMAP_ULONG];
		var iRow = h - 1;
		var iCol = 0;
		int i = 0;
		do
		{
			f(p, iRow, iCol, w, h);
			i++;
			iRow--;
			p = qs.NextBitmap(p);
		} while (iRow >= 0);
		iRow = 0;
		iCol = 1;
		do
		{
			f(p, iRow, iCol, w, h);
			i++;
			iCol++;
			p = qs.NextBitmap(p);
		} while (iCol < w);
	}
	static void PrintSlash_multipleBlock(void* pBitmap, int r, int c, int iWidth, int iHeight)
	{
		do
		{
			SetBitAt((T*)pBitmap, (uint)((r * iWidth) + c));
			r++;
			if (r >= iHeight) break;
			c--;
			if (c < 0) break;
		} while (true);
	}
	internal static void Generate_SLASHES(NQueenData qs, void* p)
	{
		var f = PrintSlash_multipleBlock;
		if (qs.SZ_BITMAP <= sizeof(T))
		{
			f = NativeOp<T>.PrintSlash;
		}
		ref readonly var w = ref qs.iWidth;
		ref readonly var h = ref qs.iHeight;
		//TEMPLATE_SLASHES = new ulong[w + h - 1];
		int i = 0;
		int iRow = 0;
		int iCol = 0;
		do
		{
			f(p, iRow, iCol, w, h);
			i++;
			iCol++;
			p = qs.NextBitmap(p);
		} while (iCol < w);
		iCol = w - 1;
		iRow = 1;
		do
		{
			f(p, iRow, iCol, w, h);
			i++;
			iRow++;
			p = qs.NextBitmap(p);
		} while (iRow < h);
	}
	internal static void PrintQueenSight(NQueenData qs, void* pBitmap, int iBit)
	{
		//var pBitmap = qs.QueenCache_GetAt(iCache);
		void f(void* pBitmapSrc)
		{
			var d = (byte*)pBitmap;
			var s = (byte*)pBitmapSrc;
			for (int i = 0; i < qs.SZ_BITMAP; i++)
			{
				d[i] |= s[i];
			}
		}
		(var iRow, var iCol) = Math.DivRem(iBit, qs.iWidth);

		f(qs.GetHLineAt(iRow));
		f(qs.GetVLineAt(iRow));
		f(qs.GetBackSlashAt(iRow, iCol));
		f(qs.GetSlashAt(iRow, iCol));
	}
	internal static void GenerateTemplate(NQueenData qs)
	{
		Generate_HLINES(qs, qs.H_LINES);
		Generate_VLINES(qs, qs.V_LINES);
		Generate_BACKSLASHES(qs, qs.BACK_SLASHES);
		Generate_SLASHES(qs, qs.SLASHES);

	}
	internal static int[] ConvertByteArrayToQueenPosArray(byte[] bytes)
	{
		unsafe
		{
			fixed (byte* p = bytes)
			{
				List<int> indice = [];
				var len = bytes.Length;
				int i = 0;
				void f(ulong x)
				{
					while (x != 0)
					{
						var iBitOffset = (int)ulong.TrailingZeroCount(x);
						i += iBitOffset;
						indice.Add(i + iBitOffset);
						x >>= iBitOffset + 1;
					};
				}
				while (len >= sizeof(ulong))
				{
					f(*(ulong*)p);
					len -= sizeof(ulong);
				}
				if (len > 0)
				{
					ulong x = *(ulong*)p;
					NativeOp<ulong>.ClearBitRange(&x, len, sizeof(ulong) - len);
					f(*(ulong*)p);
				}
				return indice.ToArray();
			}
		}
	}
}
public unsafe class NQueenData : IDisposable
{

	internal readonly int iWidth;
	//public int Width => iWidth;
	internal readonly int iHeight;
	//public int Height => iHeight;

	void* buffer;
	int[] m_lenSEC;
	internal void* H_LINES;//bitmap templates
	internal void* V_LINES;
	internal void* BACK_SLASHES;
	internal void* SLASHES;
	internal void* QUEEN_CACHE;
	/// <summary>
	/// size of bitmap
	/// </summary>
	int SZ;
	public int SZ_BITMAP => SZ;
	/// <summary>
	/// size of block, 1 or multiple block consist a bitmap
	/// </summary>
	int m_sz_block;
	public int SZ_BLOCK => m_sz_block;
	internal void* NextBitmap(void* p0) => (byte*)p0 + SZ;
	internal void* QueenCache_GetAt(int iCache) => (byte*)QUEEN_CACHE + iCache * SZ;
	internal void QueenCache_ClearAt(int iCache) => NativeMemory.Clear(QueenCache_GetAt(iCache), (nuint)SZ);

	internal void QueenCache_CopyFromPrevTo(int iCache)
	{
		void* pBitmapDst = this.QueenCache_GetAt(iCache);
		void* pBitmapSrcPrevCache = (byte*)pBitmapDst - SZ;
		NativeMemory.Copy(pBitmapSrcPrevCache, pBitmapDst, (nuint)SZ);
	}
	internal void* GetHLineAt(int iRow) => (byte*)H_LINES + iRow * SZ;
	internal void* GetVLineAt(int iCol) => (byte*)V_LINES + iCol * SZ;
	internal void* GetBackSlashAt(int iRow, int iCol) => (byte*)BACK_SLASHES + (iCol + iHeight - 1 - iRow) * SZ;
	internal void* GetSlashAt(int iRow, int iCol) => (byte*)SLASHES + (iCol + iRow) * SZ;



	internal int cntChessboardTiles;
	internal int cntQueens;
	public NQueenData(uint iWidth, uint iHeight, uint cntQueen, uint szBlock = sizeof(ulong))
	{
		ArgumentOutOfRangeException.ThrowIfZero(cntQueen, nameof(cntQueen));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(cntQueen, 16u, nameof(cntQueen));
		cntQueens = (int)cntQueen;
		ArgumentOutOfRangeException.ThrowIfZero(szBlock, nameof(szBlock));
		ArgumentOutOfRangeException.ThrowIfGreaterThan((int)szBlock, 32, nameof(szBlock));
		this.m_sz_block = (int)szBlock;
		const int cntBit_BYTE = 8;
		this.iWidth = (int)iWidth;
		this.iHeight = (int)iHeight;
		ref var w = ref this.iWidth;
		ref var h = ref this.iHeight;
		cntChessboardTiles = w * h;
		ArgumentOutOfRangeException.ThrowIfZero(cntChessboardTiles, nameof(cntChessboardTiles));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(cntChessboardTiles, m_sz_block * cntBit_BYTE, nameof(cntChessboardTiles));
		(SZ, var rem) = Math.DivRem(cntChessboardTiles, cntBit_BYTE);
		if (rem > 0)
		{
			SZ++;
		}
		switch (SZ)
		{
			case <= 1: break;
			case <= sizeof(ushort): SZ = sizeof(ushort); break;
			case <= sizeof(uint): SZ = sizeof(uint); break;
			case <= sizeof(ulong): SZ = sizeof(ulong); break;
			default://multiple of 8 bytes
				(var q, var rem2) = Math.DivRem(cntChessboardTiles, m_sz_block * 8);
				if (rem2 > 0)
				{
					q++;
				}
				SZ = q * m_sz_block;
				break;
		}
		if (SZ < m_sz_block) SZ = m_sz_block;
		m_lenSEC = [
			h,
			w,
			w + h - 1,
			w + h - 1,
			(int)cntQueen - 1,
		];

		var m_len_buffer = SZ * m_lenSEC.Sum();
		buffer = NativeMemory.AllocZeroed((nuint)m_len_buffer);
		var m_pSEC = new byte*[m_lenSEC.Length];
		m_pSEC[0] = (byte*)buffer;
		for (int i = 1; i < m_pSEC.Length; i++)
		{
			m_pSEC[i] = m_pSEC[i - 1] + SZ * m_lenSEC[i - 1];
		}
		H_LINES = m_pSEC[0];
		V_LINES = m_pSEC[1];
		BACK_SLASHES = m_pSEC[2];
		SLASHES = m_pSEC[3];
		QUEEN_CACHE = m_pSEC[4];

		switch (SZ)
		{
			case <= 1:
				MultiBlockOp<byte>.GenerateTemplate(this);
				break;
			case <= sizeof(ushort):
				MultiBlockOp<ushort>.GenerateTemplate(this);
				break;
			case <= sizeof(uint):
				MultiBlockOp<uint>.GenerateTemplate(this);
				break;
			case <= sizeof(ulong):
				MultiBlockOp<ulong>.GenerateTemplate(this);
				break;
			default://multiple of 8 bytes
				MultiBlockOp<ulong>.GenerateTemplate(this);
				break;
		}

	}

	public Solver CreateSolver(bool preferMulti = false) => this.SZ switch
	{
		<= 0 => throw new ArgumentOutOfRangeException(),
		<= sizeof(ushort) => new SolverNative<ushort>(this),
		<= sizeof(uint) => new SolverNative<uint>(this),
		<= sizeof(ulong) => preferMulti switch
		{
			true => new SolverMultiBlock(this),
			_ => new SolverNative<ulong>(this)
		},
		_ => new SolverMultiBlock(this),
	};
	public static string BitMapToString(void* pBitmap, int w, int h)
	{
		var r = new StringBuilder();
		int iBit = 0;
		var data = (ulong*)pBitmap;
		int cnt_bits_total = w * h;
		int cnt_bits_typeBlock = Marshal.SizeOf(data[0]) * 8;
		for (int iRow = 0; iRow < h; iRow++)
		{
			for (int iCol = 0; iCol < w; iCol++)
			{
				if (((*data >> iBit) & 1) == 1)
				{
					r.Append('X');
				}
				else
				{
					r.Append('.');
				}
				iBit++;
				if (iBit >= cnt_bits_total)
				{
					goto END;
				}
				if (iBit >= cnt_bits_typeBlock)
				{
					iBit = 0;
					data++;
				}
			}
			r.AppendLine();
		}
	END:
		return r.ToString();
	}

	#region IDisposable
	private bool disposedValue;
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				// TODO: dispose managed state (managed objects)
			}

			// TODO: free unmanaged resources (unmanaged objects) and override finalizer
			NativeMemory.Free(buffer);
			buffer = null;
			// TODO: set large fields to null
			disposedValue = true;
		}
	}
	// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
	~NQueenData()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	#endregion

}

public abstract unsafe class Solver
{
	public delegate*<NQueenData, void*, int, void> fPrintQueenSight;
	public Func<NQueenData, object, List<int[]>> fDumpBook;
	public NQueenData data;
	public Solver(NQueenData data)
	{
		this.data = data;
	}
	public abstract bool RegisterQueenConfig(int[] Q);
	public abstract List<int[]> DumpBook();
	public abstract int NextVacant(void* ptr, int iBitCur);


}
public unsafe class SolverNative<T> : Solver where T : unmanaged, IBinaryInteger<T>
{
	HashSet<T> book;
	public SolverNative(NQueenData data1) : base(data1)
	{
		fPrintQueenSight = &NativeOp<T>.PrintQueenSight;
		book = new HashSet<T>();
	}
	public override int NextVacant(void* ptr, int iBitCur) => NativeOp<T>.NextZeroBit(ptr, iBitCur);
	public override bool RegisterQueenConfig(int[] Q)
	{
		var hs = book;
		var qs = this.data;
		var b = NativeOp<T>.ConvertQueenPosArrayToNative(Q);
		if (hs.Contains(b))
		{
			return false;
		}
		void f(T x)
		{
			hs.Add(x);
			hs.Add(NativeOp<T>.Rotate90(x, qs.iWidth, qs.iHeight));
			hs.Add(NativeOp<T>.Rotate180(x, qs.cntChessboardTiles));
			hs.Add(NativeOp<T>.Rotate270(x, qs.iWidth, qs.iHeight));
		}
		f(b);
		var a = NativeOp<T>.FlipX(b, qs.iWidth, qs.iHeight);
		f(a);
		return true;
	}
	public override List<int[]> DumpBook() => book.Select(NativeOp<T>.ConvertNativeToQueenPosArray).ToList();
}
/// <summary>
/// using one byte to present queen position on chessboard, low 4 bit for x, high for y
/// </summary>
public unsafe static class SparseBitArrayQueen
{
	internal static BitArray FromQueens(int[] Q, int w)
	{
		var ba = new BitArray(Q.Length * 8);
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(ba) as int[];
		fixed (int* pDst = arrDst)
		{
			byte* p = (byte*)pDst;
			foreach (var q in Q)
			{
				(var iRow, var iCol) = Math.DivRem(q, w);
				*p = (byte)((iRow << 4) | iCol);
			}
		}
		return ba;
	}
	internal static int[] ToQueens(BitArray ba, int w)
	{
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(ba) as int[];
		fixed (int* pDst = arrDst)
		{
			byte* p = (byte*)pDst;
			var cnt = ba.Length / 8;
			int[] r = new int[cnt];
			for (nint i = 0; i < cnt; i++)
			{
				r[i] = (*p & 15) + ((*p >> 4) * w);
			}
			return r;
		}
	}
	const int MASK_HIGH = 0b1111_0000;//240;
	const int MASK_LOW = 0b1111; //15;
	const int LEN_HALF = 4;
	public static void R180(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//x=w-x
		//y=h-y
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = ((h - 1) << LEN_HALF) | (w - 1);
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)(magicnum - pS[i]);
		}
	}
	public static void FlipY(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//y=-y
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = ((h - 1) << LEN_HALF);
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((magicnum - pS[i] & MASK_HIGH) | (pS[i] & MASK_LOW));
		}
	}
	public static void FlipX(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//x=-x
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = w - 1;
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((pS[i] & MASK_HIGH) | (magicnum - pS[i] & MASK_LOW));
		}
	}
	public static void FlipXY(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//x=y
		//y=x
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = w - 1;
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((pS[i] >> LEN_HALF) | (pS[i] << MASK_HIGH));
		}
	}
	public static void R90(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//Rotate anti-clockwise
		//x=y
		//y=w-x	
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = w - 1;
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((pS[i] >> LEN_HALF) | ((w - (pS[i] & MASK_LOW)) << LEN_HALF));
		}
	}
	public static void R270(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//x=h-y
		//y=x
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = w - 1;
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((pS[i] << LEN_HALF) | (h - (pS[i] >> LEN_HALF)));
		}
	}
	internal static BitArray ConvertQueenPosArrayToBitArray(int[] Q, int cntBits_Bitmap)
	{
		var bb = new System.Collections.BitArray(cntBits_Bitmap);
		foreach (var q in Q)
		{
			bb.Set(q, true);
		}
		return bb;
		//return (Array)bb.GetType().GetField("m_array", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(bb);

		//var b1 = BigInteger.Zero;
		//foreach (int i in Q)
		//{
		//	b1 |= BigInteger.One << i;
		//}
		//return b1.ToByteArray();
	}
	static BitArray FuncMapper(BitArray baSrc, NQueenData data, Action<nint, int, int, nint, nint> fBitOp)
	{
		var arrSrc = BitArrayComparer.fi_bitarray.GetValue(baSrc) as int[];
		var baDst = new BitArray(baSrc.Length);
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(baDst) as int[];
		var cnt = data.cntQueens;
		var temp = stackalloc byte[cnt];
		fixed (int* pSrc = arrSrc, pDst = arrSrc)
		{
			fBitOp((nint)pSrc, data.iWidth, data.iHeight, (nint)temp, cnt);
			var pByteDst = (byte*)pDst;
			for (int i = 0; i < cnt; i++)
			{
				pDst[temp[i] >> LEN_HALF] = temp[i];
			}
		}
		return baDst;
	}
	public static BitArray Rotate90(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, SparseBitArrayQueen.R90);
	public static BitArray Rotate180(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, SparseBitArrayQueen.R180);
	public static BitArray Rotate270(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, SparseBitArrayQueen.R270);
	public static BitArray FlipX(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, SparseBitArrayQueen.FlipX);

}
public unsafe static class CQuickSort
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap(byte* a, byte* b)
	{
		var c = *a;
		*a = *b;
		*a = c;
	}
	// Partition function
	public static int partition(byte* arr, int low, int high)
	{
		int pivot = arr[low]; // Choosing the first element as pivot
		int i = low;
		int j = high;

		while (i < j)
		{
			while (arr[i] <= pivot && i <= high - 1)
			{
				i++;
			}
			while (arr[j] > pivot && j >= low + 1)
			{
				j--;
			}
			if (i < j)
			{
				Swap(&arr[i], &arr[j]);
			}
		}
		Swap(&arr[low], &arr[j]);
		return j;
	}

	// QuickSort function
	public static void quickSort(byte* arr, int low, int high)
	{
		if (low < high)
		{
			int pi = partition(arr, low, high); // Partition index
			quickSort(arr, low, pi - 1); // Recursively sort left sub-array
			quickSort(arr, pi + 1, high); // Recursively sort right sub-array
		}
	}
}
public unsafe class BitArrayComparer : IComparer<BitArray>
{
	const int cntBit_ULONG = sizeof(ulong) * 8;
	public static FieldInfo fi_bitarray = typeof(BitArray).GetField("m_array", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

	internal static int[] ConvertBitArrayToQueenPosArray(BitArray bb)
	{
		(var cntBlock, var rem) = Math.DivRem(bb.Length, cntBit_ULONG);
		//if (rem != 0)
		//{
		//	throw new Exception("bitarray is not a multiple of 8 bytes");
		//}
		unsafe
		{
			var ints = (int[])bb.GetType().GetField("m_array", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(bb);
			fixed (int* pINT = ints)
			{
				var p = (ulong*)pINT;
				List<int> indice = [];
				int i = 0;
				void f(ulong x)
				{
					while (x != 0)
					{
						var iBitOffset = (int)ulong.TrailingZeroCount(x);
						i += iBitOffset;
						indice.Add(i + iBitOffset);
						x >>= iBitOffset + 1;
					};
				}
				var iBlock = 0;
				while (iBlock < cntBlock)
				{
					f(p[iBlock]);
					iBlock++;
				}
				if (rem > 0)
				{
					var x = *p;
					x &= 1ul << (rem - 1);
					f(x);
				}
				return indice.ToArray();
			}
		}
	}
	public int Compare(BitArray? x, BitArray? y)
	{
		(var cntBlock, var rem) = Math.DivRem(x.Length, cntBit_ULONG);
		unsafe
		{
			var ints_a = (int[])fi_bitarray.GetValue(x);
			var ints_b = (int[])fi_bitarray.GetValue(y);
			fixed (int* pINT_A = ints_a, pINT_B = ints_b)
			{
				var pA = (ulong*)pINT_A;
				var pB = (ulong*)pINT_B;
				if (rem > 0)
				{
					long d = (long)((pA[cntBlock] & 1ul << (rem - 1)) - (pB[cntBlock] & 1ul << (rem - 1)));
					switch (d)
					{
						case 0: break;
						default: return Math.Sign(d);
					}
				}
				var iBlock = cntBlock - 1;
				while (iBlock >= 0)
				{
					long d = (long)(pA[cntBlock] - pB[cntBlock]);
					switch (d)
					{
						case 0: break;
						default: return Math.Sign(d);
					}
					iBlock--;
				}
			}
		}
		return 0;
	}
}
public unsafe class SolverMultiBlock : Solver
{
	SortedSet<BitArray> book;
	int cntBits_BITMAP;
	int cntBits_BLOCK;
	int cntBlocksPerBitmap;
	public SolverMultiBlock(NQueenData data1) : base(data1)
	{
		ArgumentOutOfRangeException.ThrowIfNotEqual(data.SZ_BLOCK, sizeof(ulong), "block must be ulong");
		ArgumentOutOfRangeException.ThrowIfNotEqual(data.SZ_BITMAP, sizeof(ulong), $"{nameof(data.SZ_BITMAP)} must be multiple of {nameof(data.SZ_BLOCK)}");
		cntBits_BLOCK = data.SZ_BLOCK * 8;
		cntBits_BITMAP = data.SZ_BITMAP * 8;
		fPrintQueenSight = &MultiBlockOp<ulong>.PrintQueenSight;
		book = new(new BitArrayComparer());
		cntBlocksPerBitmap = data.SZ_BITMAP / data.SZ_BLOCK;
	}
	public override int NextVacant(void* ptr, int iBitCur)
	{
		var p = (ulong*)ptr;
		(var q, var r) = Math.DivRem(iBitCur, cntBits_BLOCK);
	AGAIN:
		while (q < cntBlocksPerBitmap)
		{
			do
			{
				if ((p[q] & (1ul << r)) == 0ul)
				{
					return q * cntBits_BLOCK + r;
				}
				r++;
			} while (r < cntBits_BLOCK);
			r = 0;
			q++;
		}
		return -1;
	}
	public override bool RegisterQueenConfig(int[] Q)
	{
		var hs = book;
		var b = SparseBitArrayQueen.ConvertQueenPosArrayToBitArray(Q, cntBits_BITMAP);
		if (hs.Contains(b))
		{
			return false;
		}
		var cntQ = Q.Length;
		void f(BitArray x)
		{
			hs.Add(x);
			hs.Add(SparseBitArrayQueen.Rotate90(x, data));
			hs.Add(SparseBitArrayQueen.Rotate180(x, data));
			hs.Add(SparseBitArrayQueen.Rotate270(x, data));
		}
		f(b);
		var a = SparseBitArrayQueen.FlipX(b, data);
		f(a);
		return true;
	}
	public override List<int[]> DumpBook()
	{
		return book.Select(BitArrayComparer.ConvertBitArrayToQueenPosArray).ToList();
	}
}
public class NQueenSolver
{
	public static unsafe List<int[]> All(int n, bool preferMultiBlock = false)
	{
		using var qs = new NQueenData((uint)n, (uint)n, (uint)n);
		var s = qs.CreateSolver(preferMultiBlock);

		var idxLastTile = n * n - 1;
		var Queens = new int[n];//N queens' indice in bitmap
		Queens[0] = 0;
		int iQ = 0, iBit = 0;
		var fPrintCurrentQueenSight = () => s.fPrintQueenSight(qs, qs.QueenCache_GetAt(iQ), iBit);
		do
		{
			qs.QueenCache_ClearAt(iQ);
			iBit = Queens[iQ];
			fPrintCurrentQueenSight();
		NEXT_Q:
			iQ++;
			if (iQ < n - 1)
			{
				qs.QueenCache_CopyFromPrevTo(iQ);
				iBit = s.NextVacant(qs.QueenCache_GetAt(iQ), iBit);
				if (iBit == -1 || iBit >= idxLastTile)
				{
					iQ--;
					goto CARRY;
				}
				Queens[iQ] = iBit;
				fPrintCurrentQueenSight();
				goto NEXT_Q;
			}
		YIELD_AGAIN:
			iBit = s.NextVacant(qs.QueenCache_GetAt(iQ - 1), iBit);
			if (iBit == -1 || iBit >= idxLastTile)
			{
				iQ--;
				goto CARRY;
			}
			Queens[iQ] = iBit;
			s.RegisterQueenConfig(Queens);
			//yield return (int[])Q.Clone();
			goto YIELD_AGAIN;
		CARRY:
			if (iQ < 1) goto CARRY_FIRST;
			qs.QueenCache_CopyFromPrevTo(iQ);
			iBit = s.NextVacant(qs.QueenCache_GetAt(iQ), Queens[iQ]);
			if (iBit == -1 || iBit >= idxLastTile)
			{
				iQ--;
				goto CARRY;
			}
			Queens[iQ] = iBit;
			fPrintCurrentQueenSight();
			goto NEXT_Q;
		CARRY_FIRST:
			Queens[0]++;
		} while (Queens[0] < n - n / 2);//first queen only move few steps

		return s.DumpBook();
	}
}
public class Solution_leetCode
{
	public static IList<string> ConvertFromIndice(int[] indice, int w, int h)
	{
		int i = 0;
		List<string> r = new(h);
		var iData = 0;
		var idx_target = indice[iData];
		for (int iRow = 0; iRow < h; iRow++)
		{
			var line = new StringBuilder(w);
			for (int iCol = 0; iCol < w; iCol++)
			{
				if (i == idx_target)
				{
					line.Append('Q');
					iData++;
					if (iData < indice.Length)
					{
						idx_target = indice[iData];
					}
					else
					{
						iData = 0;
					}
				}
				else
				{
					line.Append('.');
				}
				i++;
			}
			r.Add(line.ToString());
		}
		return r;
	}
	public IList<IList<string>> SolveNQueens(int n, bool prefer = false)
	{
		var r = new List<IList<string>>();
		var list = NQueenSolver.All(n, prefer);
		foreach (var indice in list)
		{
			var x = ConvertFromIndice(indice, n, n);
			r.Add(x);
		}
		return r;
	}
}
