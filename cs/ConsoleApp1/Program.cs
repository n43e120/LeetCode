
using NQueen;
using System.Numerics;

namespace ConsoleApp1;
internal unsafe static class Program
{
	static void Main(string[] args)
	{
		//for (int i = 0; i < 8; i++)
		//{
		//	byte A = (byte)((1 << i) | 1);
		//	Console.WriteLine($"PrevOneBit({A},7)={NativeOp<byte>.PrevOneBit(&A, 7)}");
		//}
		//for (int i = 0; i < 8; i++)
		//{
		//	byte A = 1;
		//	Console.WriteLine($"PrevOneBit({A},{i})={NativeOp<byte>.PrevOneBit(&A, i)}");
		//}
		for (int i = 0; i < 8; i++)
		{
			byte A = (byte)((1 << i));
			Console.WriteLine($"NextOneBit({A},0)={NativeOp<byte>.NextOneBit(&A, 0)}");
		}
		for (int i = 0; i < 8; i++)
		{
			byte A = 128;
			Console.WriteLine($"NextOneBit({A},{i})={NativeOp<byte>.NextOneBit(&A, i)}");
		}
	}
}