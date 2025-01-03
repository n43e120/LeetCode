using System.Collections;
using System.Diagnostics;
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
public unsafe static class NativeBitmap<T> where T : unmanaged, IBinaryInteger<T>//IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>, INumberBase<T>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetBitAt(T* p, int iBit) => *p |= T.One << iBit;
	public static bool GetBitAt(void* ptr, int iBit)
	{
		return ((*(T*)ptr) & (T.One << iBit)) == T.One;
	}
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
		T MASK = iBitCur < cntBit_T ? (T.One << (iBitCur)) - T.One : T.Zero - T.One;
		var dst = (T*)ptr;
		var r = T.LeadingZeroCount(*dst & MASK);
		var i = cntBit_T - 1 - *(int*)&r;
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
	public static T Rotate0(T x) => x;
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

	public static T FromIndexArray(int[] indice)
	{
		T y = default;
		foreach (var x in indice)
		{
			y |= T.One << x;
		}
		return y;
	}
	public static int[] ToIndexArray(T x)
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
	internal static string ToDebugString(T x, int w, int h)
	{
		var sb = new StringBuilder();
		var iBit = 0;
		for (nint iRow = 0; iRow < h; iRow++)
		{
			for (nint iCol = 0; iCol < w; iCol++)
			{
				if (((x >> iBit) & T.One) == T.One)
				{
					sb.Append('Q');
				}
				else
				{
					sb.Append('.');
				}
				iBit++;
			}
			sb.AppendLine();
		}
		return sb.ToString();
	}
}
public unsafe static class MultiBlockBitmap<T> where T : unmanaged, IBinaryInteger<T> // IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>, INumberBase<T>
{
	/// <summary>
	/// size of block in byte
	/// </summary>
	//public readonly static int SZ = sizeof(T);
	public readonly static int cntBits_BLOCK = sizeof(T) * 8;
	public static void SetBitAt(void* ptr, uint iBit)
	{
		T* p = (T*)ptr;
		(var q, var r) = Math.DivRem((int)iBit, cntBits_BLOCK);
		p[q] |= T.One << r;
	}
	public static bool GetBitAt(void* ptr, uint iBit)
	{
		T* p = (T*)ptr;
		(var q, var r) = Math.DivRem((int)iBit, cntBits_BLOCK);
		return (p[q] & T.One << r) != T.Zero;
	}
	public static void SetBitRange(void* ptr, int iBitStart, int cnt)
	{
		T* p = (T*)ptr;
		(var q, var iBit) = Math.DivRem(iBitStart, cntBits_BLOCK);
#if MYDEBUG_IS_WORDY
		var debugSpan = new Span<byte>(p, 8);
#endif
		if (iBit + cnt <= cntBits_BLOCK)
		{
			NativeBitmap<T>.SetBitRange(p + q, iBit, cnt);
			return;
		}

		p += q;
		*p |= T.Zero - (T.One << iBit);
		cnt -= cntBits_BLOCK - iBit;
		iBit = 0;
		while (cnt >= cntBits_BLOCK)
		{
			p++;
			*p |= T.Zero - T.One;
			cnt -= cntBits_BLOCK;
		}
		if (cnt == 0) return;
		p++;
		*p |= (T.One << (cnt)) - T.One;
	}
	public static int NextZeroBit(void* ptr, int iBitCur, int cntBlocksPerBitmap)
	{
		if (cntBlocksPerBitmap == 1)
		{
			return NativeBitmap<T>.NextZeroBit(ptr, iBitCur);
		}
		var p = (T*)ptr;
		(var q, var r) = Math.DivRem(iBitCur, cntBits_BLOCK);

		var idx = NativeBitmap<T>.NextZeroBit(p + q, r);
		r = -1;
		while (idx == -1)
		{
			q++;
			if (q >= cntBlocksPerBitmap)
			{
				return -1;
			}
			idx = NativeBitmap<T>.NextZeroBit(p + q, r);
		}
		return q * cntBits_BLOCK + idx;
	}

	internal static void Generate_HLINES(NQueenData qs, void* p)
	{
		for (int iRow = 0; iRow < qs.iHeight; iRow++)
		{
			SetBitRange((T*)p, iRow * qs.iWidth, qs.iWidth);
			p = qs.NextBitmap(p);
		}
	}
	static void Bitmap_CopyShiftLeftOne(T* dstBlock0, T* srcBlock0, int iHeight)
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
				rem -= cntBits_BLOCK;
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
			f = NativeBitmap<T>.PrintBackslash;
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
			f = NativeBitmap<T>.PrintSlash;
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
		f(qs.GetVLineAt(iCol));
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
	internal static int[] ToIndexArray(byte[] bytes)
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
					NativeBitmap<ulong>.ClearBitRange(&x, len, sizeof(ulong) - len);
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
	public void QueenCache_ClearAt(int iCache) => NativeMemory.Clear(QueenCache_GetAt(iCache), (nuint)SZ);

	internal void QueenCache_CopyFromPrevTo(int iCache)
	{
		void* pBitmapDst = QueenCache_GetAt(iCache);
		void* pBitmapSrcPrevCache = (byte*)pBitmapDst - SZ;
		NativeMemory.Copy(pBitmapSrcPrevCache, pBitmapDst, (nuint)SZ);
	}
	internal void* GetHLineAt(int iRow) => (byte*)H_LINES + iRow * SZ;
	internal void* GetVLineAt(int iCol) => (byte*)V_LINES + iCol * SZ;
	internal void* GetBackSlashAt(int iRow, int iCol) => (byte*)BACK_SLASHES + (iCol + iHeight - 1 - iRow) * SZ;
	internal void* GetSlashAt(int iRow, int iCol) => (byte*)SLASHES + (iCol + iRow) * SZ;



	internal readonly int cntChessboardTiles;
	internal readonly int cntQueens;
	public NQueenData(uint iWidth, uint iHeight, uint cntQueen, uint szBlock = sizeof(ulong))
	{
		ArgumentOutOfRangeException.ThrowIfZero(cntQueen, nameof(cntQueen));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(cntQueen, 16u, nameof(cntQueen));
		cntQueens = (int)cntQueen;
		ArgumentOutOfRangeException.ThrowIfZero(szBlock, nameof(szBlock));
		ArgumentOutOfRangeException.ThrowIfGreaterThan((int)szBlock, 32, nameof(szBlock));
		m_sz_block = (int)szBlock;
		const int cntBit_BYTE = 8;
		this.iWidth = (int)iWidth;
		this.iHeight = (int)iHeight;
		ref var w = ref this.iWidth;
		ref var h = ref this.iHeight;
		cntChessboardTiles = w * h;
		ArgumentOutOfRangeException.ThrowIfZero(cntChessboardTiles, nameof(cntChessboardTiles));
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
				MultiBlockBitmap<byte>.GenerateTemplate(this);
				break;
			case <= sizeof(ushort):
				MultiBlockBitmap<ushort>.GenerateTemplate(this);
				break;
			case <= sizeof(uint):
				MultiBlockBitmap<uint>.GenerateTemplate(this);
				break;
			case <= sizeof(ulong):
				MultiBlockBitmap<ulong>.GenerateTemplate(this);
				break;
			default://multiple of 8 bytes
				MultiBlockBitmap<ulong>.GenerateTemplate(this);
				break;
		}

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
public unsafe static class NQueenDebugger
{
	public static string BitMapToString(void* pBitmap, int w, int h, Span<int> queens = default)
	{
		var r = new StringBuilder();
		int iBit = 0;
		var data = (ulong*)pBitmap;
		int cnt_bits_total = w * h;
		int cnt_bits_block = Marshal.SizeOf(data[0]) * 8;
		int iBlock = 0;
		for (int iRow = 0; iRow < h; iRow++)
		{
			for (int iCol = 0; iCol < w; iCol++)
			{
				char c;
				if (((data[iBlock] >> iBit) & 1) == 1)
				{
					if (queens.Contains(iBlock * cnt_bits_block + iBit))
					{
						c = 'Q';
					}
					else
					{
						c = '.';
					}
				}
				else
				{
					c = '_';
				}
				r.Append(c);
				iBit++;
				if (iBit >= cnt_bits_total)
				{
					goto END;
				}
				if (iBit >= cnt_bits_block)
				{
					iBit = 0;
					iBlock++;
				}
			}
			r.AppendLine();
		}
	END:
		return r.ToString();
	}
	public static void PrintBitmapAt(NQueenData data, int iQ)
	{
		unsafe
		{
			var bitmap = data.GetHLineAt(iQ);
			var a = BitMapToString(bitmap, data.iWidth, data.iHeight);
			Console.WriteLine(a);
			Console.WriteLine($"------{iQ}");
		}
	}
	public static void PrintAllTemplates(NQueenData d)
	{
		var cntBitmap = (d.iWidth + d.iHeight) * 3 - 2;
		for (int i = 0; i < cntBitmap; i++)
		{
			NQueenDebugger.PrintBitmapAt(d, i);
		}
	}
	public static void PrintCurrentGameStatus(NQueenData data, int iQ, int[] Queens)
	{
		unsafe
		{
			Span<int> subQs = Queens is null ? ([]) : (Span<int>)Queens[0..(iQ + 1)];
			var bitmap = data.QueenCache_GetAt(iQ);
			var a = BitMapToString(bitmap, data.iWidth, data.iHeight, subQs);
			Console.WriteLine(a);
			Console.Write("------");
			Console.Write('[');
			foreach (var q in subQs)
			{
				Console.Write(q);
				Console.Write(',');
			}
			Console.Write(']');
			Console.WriteLine();
		}
	}
#if DEBUG
	public static void PrintSolution(IList<int> solution, int n)
	{
		var lines = Solution.FromIndexArray(solution, n, n);
		foreach (var line in lines)
		{
			Console.WriteLine(line);
		}
		Console.Write("------");
		Print(solution);
		Console.WriteLine();
	}
#endif
	public static bool IsArrayStartWith(Span<int> array, Span<int> tester)
	{
		for (int i = 0; i < tester.Length; i++)
		{
			if (array[i] != tester[i]) { return false; }
		}
		return true;
	}
	public static int[] FlipY(NQueenData data, int[] Queens)
	{
		var w = data.iWidth;
		var h = data.iHeight;
		var list = new List<int>();
		foreach (int q in Queens)
		{
			(var iRow, var iCol) = Math.DivRem(q, w);
			list.Add((h - 1 - iRow) * w + iCol);
		}
		list.Sort();
		return list.ToArray();
	}
}
public abstract unsafe class Solver
{
	public delegate*<NQueenData, void*, int, void> fPrintQueenSight;
	public NQueenData m_data;
	public Solver(NQueenData data)
	{
		m_data = data;
	}
	public abstract int NextUnthreatenedSquare(void* ptr, int iBitCur);
	public abstract List<int[]> SaveSolutionGame(int[] Q);
	//public abstract List<int[]> GetSolutionsAsIndexArray();
	public abstract void PrintVisitedCornerToCache(int iBit);
}
public unsafe class SolverNative<T> : Solver where T : unmanaged, IBinaryInteger<T>
{
	HashSet<T> m_game;
	public SolverNative(NQueenData data1) : base(data1)
	{
		fPrintQueenSight = &NativeBitmap<T>.PrintQueenSight;
		m_game = [];
	}
	public override int NextUnthreatenedSquare(void* ptr, int iBitCur) => NativeBitmap<T>.NextZeroBit(ptr, iBitCur);
	public override List<int[]> SaveSolutionGame(int[] Q)
	{
		var w = m_data.iWidth;
		var h = m_data.iHeight;
		var cntQueens = m_data.cntQueens;
		var hs = m_game;
		var qs = m_data;
		var b = NativeBitmap<T>.FromIndexArray(Q);
		if (hs.Contains(b))
		{
			return null;
		}
		var isomorphicGroup = new List<int[]>();
		void fadd(T x)
		{
			var bSuccess = hs.Add(x);
			if (bSuccess)
			{
				isomorphicGroup.Add(NativeBitmap<T>.ToIndexArray(x));
#if MYDEBUG_IS_WORDY
				//Console.Write(NativeBitmap<T>.ToDebugString(x, w, h));
				Console.WriteLine($"---Add count:{hs.Count}");
#endif
			}
			else
			{
#if MYDEBUG_IS_WORDY
				Console.WriteLine($"Add fail");
#endif
			}
		}
		void f(T x)
		{
			fadd(NativeBitmap<T>.Rotate0(x));
			fadd(NativeBitmap<T>.Rotate90(x, qs.iWidth, qs.iHeight));
			fadd(NativeBitmap<T>.Rotate180(x, qs.cntChessboardTiles));
			fadd(NativeBitmap<T>.Rotate270(x, qs.iWidth, qs.iHeight));
		}
		f(b);
		var a = NativeBitmap<T>.FlipX(b, qs.iWidth, qs.iHeight);
		f(a);
		return isomorphicGroup;
	}
	//public override List<int[]> GetSolutionsAsIndexArray() => m_game.Select(NativeBitmap<T>.ToIndexArray).ToList();

	public override void PrintVisitedCornerToCache(int iBit)
	{
		ref readonly int w = ref m_data.iWidth;
		ref readonly int h = ref m_data.iHeight;
		if (iBit == 0) return;
		T bitmap = default(T);
		NativeBitmap<T>.SetBitRange(&bitmap, 0, iBit);
		NativeBitmap<T>.SetBitRange(&bitmap, w - iBit, iBit);
		bitmap |= bitmap << (NativeBitmap<T>.cntBit_T - w);
		bitmap |= NativeBitmap<T>.Rotate90(bitmap, w, h);
		*(T*)m_data.QUEEN_CACHE = bitmap;
	}
}
/// <summary>
/// using one byte to record (x,y) tuple, low 4 bit for x, high for y, 0<= x,y <= 15
/// </summary>
public unsafe static class XYTupleByteArray
{
	const byte MASK_HIGH = 0b1111_0000;//240;
	const byte MASK_LOW = 0b1111; //15;
	const int LEN_HALF = 4;
	internal static BitArray FromIndexArray(int[] Q, int w)
	{
		(var lenBuff, var r) = Math.DivRem(Q.Length, sizeof(ulong));
		if (r != 0)
		{
			lenBuff++;
		}
		var ba = new BitArray(lenBuff * sizeof(ulong) * 8);
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(ba) as int[];
		fixed (int* pDst = arrDst)
		{
			byte* p = (byte*)pDst;
			foreach (var q in Q)
			{
				(var iRow, var iCol) = Math.DivRem(q, w);
				*p = (byte)((iRow << LEN_HALF) | iCol);
				p++;
			}
		}
		return ba;
	}
	internal static int[] ToIndexArray(BitArray ba, int w, int cnt)
	{
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(ba) as int[];
		fixed (int* pDst = arrDst)
		{
			byte* p = (byte*)pDst;
			int[] r = new int[cnt];
			for (nint i = 0; i < cnt; i++)
			{
				r[i] = (*p & 15) + ((*p >> 4) * w);
				p++;
			}
			return r;
		}
	}

	public static string PtrToString(void* pSrc, int w, int cnt)
	{
		var sb = new StringBuilder();
		byte* p = (byte*)pSrc;
		sb.Append('[');
		for (nint iQ = 0; iQ < cnt; iQ++)
		{
			var iRow = *p & MASK_LOW;
			var iCol = (*p & MASK_HIGH) >> LEN_HALF;
			sb.Append($"({iRow},{iCol}),");
			p++;
		}
		sb.Append(']');
		return sb.ToString();
	}
	public static string ToDebugString(BitArray ba, int w, int cnt)
	{
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(ba) as int[];
		fixed (int* pDst = arrDst)
		{
			return PtrToString(pDst, w, cnt);
		}
	}
	internal static void Sort(BitArray ba, int w, int cnt)
	{
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(ba) as int[];
		fixed (int* pDst = arrDst)
		{
			byte* p = (byte*)pDst;
			CQuickSort.quickSort(p, 0, cnt - 1);
		}
	}
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
		var magicnum = (h - 1) << LEN_HALF;
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
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((pS[i] >> LEN_HALF) | (pS[i] << MASK_HIGH));
		}
	}
	public static void R0(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//x=x
		//y=y
		NativeMemory.Copy((void*)pSrc, (void*)pDst, (nuint)len);
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
			pD[i] = (byte)((pS[i] >> LEN_HALF) | ((magicnum - (pS[i] & MASK_LOW)) << LEN_HALF));
		}
	}
	public static void R270(nint pSrc, int w, int h, nint pDst, nint len)
	{
		//x=h-y
		//y=x
		var pD = (byte*)pDst;
		var pS = (byte*)pSrc;
		var magicnum = h - 1;
		for (int i = 0; i < len; i++)
		{
			pD[i] = (byte)((pS[i] << LEN_HALF) | (magicnum - (pS[i] >> LEN_HALF)));
		}
	}
	static BitArray FuncMapper(BitArray baSrc, NQueenData data, Action<nint, int, int, nint, nint> fBitOp)
	{
		var arrSrc = BitArrayComparer.fi_bitarray.GetValue(baSrc) as int[];
		var baDst = new BitArray(baSrc.Length);
		var arrDst = BitArrayComparer.fi_bitarray.GetValue(baDst) as int[];
		var cnt = data.cntQueens;
		var temp = stackalloc byte[cnt];
		fixed (int* pSrc = arrSrc, pDst = arrDst)
		{
			//Console.Write("temp{");
			//Console.WriteLine(SparseBitArrayToString(temp, data.iWidth, cnt));
			fBitOp((nint)pSrc, data.iWidth, data.iHeight, (nint)temp, cnt);
#if MYDEBUG_IS_WORDY
			Console.Write($"{fBitOp.Method.Name}->");
			Console.WriteLine(PtrToString(temp, data.iWidth, cnt));
#endif
			byte* p = (byte*)pDst;
			for (int i = 0; i < cnt; i++)
			{
				p[temp[i] >> LEN_HALF] = temp[i];
			}
#if MYDEBUG_IS_WORDY
			Console.Write($"Sort->");
			Console.WriteLine(PtrToString(p, data.iWidth, cnt));
#endif
		}
		return baDst;
	}
	public static BitArray Rotate0(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, R0);
	public static BitArray Rotate90(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, R90);
	public static BitArray Rotate180(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, R180);
	public static BitArray Rotate270(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, R270);
	public static BitArray FlipX(BitArray baSrc, NQueenData data) => FuncMapper(baSrc, data, FlipX);

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
	public static FieldInfo fi_bitarray = typeof(BitArray).GetField("m_array", BindingFlags.Instance | BindingFlags.NonPublic);

	internal static BitArray ConvertIndexArrayToBitArray(int[] Q, int cntBits_Bitmap)
	{
		var bb = new System.Collections.BitArray(cntBits_Bitmap);
		foreach (var q in Q)
		{
			bb.Set(q, true);
		}
		return bb;
	}
	internal static int[] ConvertBitArrayToIndexArray(BitArray bb)
	{
		(var cntBlock, var rem) = Math.DivRem(bb.Length, cntBit_ULONG);
		//if (rem != 0)
		//{
		//	throw new Exception("bitarray is not a multiple of 8 bytes");
		//}
		unsafe
		{
			var ints = (int[])bb.GetType().GetField("m_array", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(bb);
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
#nullable disable
	public int Compare(BitArray x, BitArray y)
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
					long d = (long)(pA[iBlock] - pB[iBlock]);
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
	readonly SortedSet<BitArray> book;
	readonly int cntBlocksPerBitmap;
	public SolverMultiBlock(NQueenData data1) : base(data1)
	{
		ArgumentOutOfRangeException.ThrowIfNotEqual(m_data.SZ_BLOCK, sizeof(ulong), "block must be ulong");
		(cntBlocksPerBitmap, var r) = Math.DivRem(m_data.SZ_BITMAP, m_data.SZ_BLOCK);
		if (r != 0)
		{
			throw new ArgumentOutOfRangeException(nameof(data1), $"{nameof(m_data.SZ_BITMAP)} must be multiple of {nameof(m_data.SZ_BLOCK)}");
		}
		ArgumentOutOfRangeException.ThrowIfLessThan(m_data.SZ_BITMAP, m_data.SZ_BLOCK);
		fPrintQueenSight = &MultiBlockBitmap<ulong>.PrintQueenSight;
		book = new(new BitArrayComparer());
	}
	public override int NextUnthreatenedSquare(void* ptr, int iBitCur) => MultiBlockBitmap<ulong>.NextZeroBit(ptr, iBitCur, cntBlocksPerBitmap);
	public override List<int[]> SaveSolutionGame(int[] Q)
	{
		int w = m_data.iWidth;
		int cntQueens = m_data.cntQueens;
		var hs = book;
		var b = XYTupleByteArray.FromIndexArray(Q, m_data.iWidth);
		XYTupleByteArray.Sort(b, w, cntQueens);
		if (hs.Contains(b))
		{
			return null;
		}
		var isomorphicGroup = new List<int[]>();
		void fadd(BitArray x)
		{
			var bSuccess = hs.Add(x);
			if (bSuccess)
			{
				isomorphicGroup.Add(XYTupleByteArray.ToIndexArray(x, w, cntQueens));
#if MYDEBUG_IS_WORDY
				Console.WriteLine($"Add count:{hs.Count}");
				Console.WriteLine(XYTupleByteArray.ToDebugString(x, w, cntQueens));
#endif
			}
			else
			{
#if MYDEBUG_IS_WORDY
				Console.WriteLine($"Add fail");
#endif
			}
		}
		void fAddAllOperations(BitArray x)
		{
			fadd(XYTupleByteArray.Rotate0(x, m_data));
			fadd(XYTupleByteArray.Rotate90(x, m_data));
			fadd(XYTupleByteArray.Rotate180(x, m_data));
			fadd(XYTupleByteArray.Rotate270(x, m_data));
		}
		fAddAllOperations(b);
		var a = XYTupleByteArray.FlipX(b, m_data);
		fAddAllOperations(a);
		return isomorphicGroup;
	}
	//public override List<int[]> GetSolutionsAsIndexArray()
	//{
	//	var r = new List<int[]>();
	//	foreach (var ba in book)
	//	{
	//		r.Add(XYTupleByteArray.ToIndexArray(ba, m_data.iWidth, m_data.cntQueens));
	//	}
	//	return r;
	//}

	public override void PrintVisitedCornerToCache(int iBit)
	{
		if (iBit == 0) return;
		ref readonly int w = ref m_data.iWidth;
		ref readonly int h = ref m_data.iHeight;
		ref readonly int cntTiles = ref m_data.cntChessboardTiles;
		var pBitmap = m_data.QUEEN_CACHE;
		MultiBlockBitmap<ulong>.SetBitRange((ulong*)pBitmap, 0, iBit);
		MultiBlockBitmap<ulong>.SetBitRange((ulong*)pBitmap, w - iBit, iBit);
		MultiBlockBitmap<ulong>.SetBitRange((ulong*)pBitmap, cntTiles - w, iBit);
		MultiBlockBitmap<ulong>.SetBitRange((ulong*)pBitmap, cntTiles - iBit, iBit);

		for (var i = 1; i < iBit; i++)
		{
			MultiBlockBitmap<ulong>.SetBitAt((ulong*)pBitmap, (uint)(i * w));
			MultiBlockBitmap<ulong>.SetBitAt((ulong*)pBitmap, (uint)((i + 1) * w - 1));
			MultiBlockBitmap<ulong>.SetBitAt((ulong*)pBitmap, (uint)(cntTiles - (i + 1) * w));
			MultiBlockBitmap<ulong>.SetBitAt((ulong*)pBitmap, (uint)(cntTiles - (i) * w) - 1);
		}
	}
}
public static class NQueenSolver
{
	public static Solver CreateSolver(NQueenData data, bool preferMulti = false)
	{
		return data.SZ_BITMAP switch
		{
			<= sizeof(ushort) => new SolverNative<ushort>(data),
			<= sizeof(uint) => new SolverNative<uint>(data),
			<= sizeof(ulong) => preferMulti switch
			{
				true => new SolverMultiBlock(data),
				_ => new SolverNative<ulong>(data)
			},
			_ => new SolverMultiBlock(data),
		};
	}
	public class SolutionInfo
	{
		public int QueenCount { get; }
		public SolutionInfo(int cntQueens)
		{
			QueenCount = cntQueens;
		}
		public List<List<int[]>> IsomorphicGroups = [];
		public int Count
		{
			get
			{
				int cnt = 0;
				foreach (var item in IsomorphicGroups)
				{
					cnt += item.Count;
				}
				return cnt;
			}
		}

		public int NonIsomorphicSolutionCount => IsomorphicGroups.Count;

		public IEnumerable<IList<int>> Solutions()
		{
			foreach (var group in IsomorphicGroups)
			{
				foreach (var item in group)
				{
					yield return item;
				}
			}
		}
	}
	public static unsafe SolutionInfo All(int n, bool preferMultiBlock = false, bool preferTrimCorners = false)
	{
		var info = new SolutionInfo(n);
		switch (n)
		{
			case 1:
				info.IsomorphicGroups.Add([[0]]);
				return info;
			case 2:
			case 3:
				return info;
			default:
				break;
		}
		var cntQueens = n;
		using var data = new NQueenData((uint)n, (uint)n, (uint)cntQueens);
		var s = CreateSolver(data, preferMultiBlock);

		var idxLastTile = n * n - 1;
		var Queens = new int[cntQueens];//N queens' indice in bitmap
		Queens[0] = 0;
		int iQ = 0, iBit = 0;
		void fPrintCurrentQueenSightToCache()
		{
			s.fPrintQueenSight(data, data.QueenCache_GetAt(iQ), iBit);
#if MYDEBUG_IS_WORDY
			//NQueenDebugger.PrintCurrentGameStatus(data, iQ, Queens);
#endif
		}
		//#if DEBUG
		//		int[] breakarray = [2, 14, 25, 31, 36, 53, 60, 64, 75];
		//		var spanbreak = breakarray[..^2];
		//#endif
		do
		{
			data.QueenCache_ClearAt(iQ);
			iBit = Queens[iQ];
			if (preferTrimCorners)
			{
				s.PrintVisitedCornerToCache(iBit);
			}
			fPrintCurrentQueenSightToCache();
		NEXT_Q:
			iQ++;
			if (iQ < cntQueens - 1)
			{
				data.QueenCache_CopyFromPrevTo(iQ);
				iBit = s.NextUnthreatenedSquare(data.QueenCache_GetAt(iQ), iBit);
				if (iBit == -1 || iBit >= idxLastTile)
				{
					iQ--;
					goto CARRY;
				}
				Queens[iQ] = iBit;
				fPrintCurrentQueenSightToCache();
				//#if DEBUG
				//				if (NQueenDebugger.IsArrayStartWith(Queens, spanbreak))
				//				{
				//					NQueenDebugger.PrintCurrentGameStatus(data, 0, Queens);
				//					NQueenDebugger.PrintCurrentGameStatus(data, iQ, Queens);
				//					Debugger.Break();
				//				}
				//#endif
				goto NEXT_Q;
			}
		YIELD_AGAIN:
			iBit = s.NextUnthreatenedSquare(data.QueenCache_GetAt(iQ - 1), iBit);
			if (iBit == -1 || iBit > idxLastTile)
			{
				iQ--;
				goto CARRY;
			}
			Queens[iQ] = iBit;
			var outList = s.SaveSolutionGame(Queens);
			if (outList != null)
			{
				info.IsomorphicGroups.Add(outList);
			}
			//yield return (int[])Q.Clone();
			goto YIELD_AGAIN;
		CARRY:
			if (iQ < 1) goto CARRY_FIRST;
			data.QueenCache_CopyFromPrevTo(iQ);
			//Queens[iQ]++;
			iBit = s.NextUnthreatenedSquare(data.QueenCache_GetAt(iQ), Queens[iQ]);
			if (iBit == -1 || iBit >= idxLastTile)
			{
				iQ--;
				goto CARRY;
			}
			Queens[iQ] = iBit;
			fPrintCurrentQueenSightToCache();
			goto NEXT_Q;
		CARRY_FIRST:
			Queens[0]++;
		} while (Queens[0] < n - n / 2);//first queen only move few steps
		return info;
	}
}
public class Solution
{
	public static IList<string> FromIndexArray(IList<int> indice, int w, int h)
	{
		var cntQueens = indice.Count;
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
					if (iData < cntQueens)
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
	/// <summary>
	/// _leetCode_51 N-Queens
	/// </summary>
	public IList<IList<string>> SolveNQueens(int n, bool prefer = false)
	{
		var r = new List<IList<string>>();
		var info = NQueenSolver.All(n, prefer);
		foreach (var indice in info.Solutions())
		{
			var x = FromIndexArray(indice, n, n);
			r.Add(x);
		}
		return r;
	}
	/// <summary>
	/// _leetCode_52 N-Queens II
	/// </summary>
	public int TotalNQueens(int n)
	{
		var r = new List<IList<string>>();
		var info = NQueenSolver.All(n, false, true);
		return info.Count;
	}
}