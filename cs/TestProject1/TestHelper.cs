#define MSTEST
global using static ConsoleApp1.TestHelper;
using System.Text;
namespace ConsoleApp1
{
	public static class TestHelper
	{
		public static void COLOR_PRINT(ConsoleColor color, Action f)
		{
			Console.ForegroundColor = color;
			f();
			Console.ResetColor();
		}
		public static void Print(object x)
		{
			switch (x)
			{
				case null:
					Console.Write("null");
					break;
				case int:
					Console.Write(x);
					break;
				case char:
					Console.Write($"\'{x}\'");
					break;
				case string:
					Console.Write($"\"{x}\"");
					break;
				default:
					Console.Write(x);
					break;
			}
		}
		public static string ConvertToString<T>(IList<T> list)
		{
			var sb = new StringBuilder();
			sb.Append('[');
			int i = 0;
			for (; i < list.Count - 1; i++)
			{
				sb.Append(list[i]);
				sb.Append(',');
			}
			if (i < list.Count)
			{
				sb.Append(list[i]);
			}
			sb.Append(']');
			return sb.ToString();
		}
		public static void Print<T>(IList<T> list)
		{
			Console.Write("[");
			int i = 0;
			for (; i < list.Count - 1; i++)
			{
				Print(list[i]);
				Console.Write(',');
			}
			if (i < list.Count)
			{
				Print(list[i]);
			}
			Console.Write("]");
		}
		public static void Print<T>(IList<IList<T>> ll)
		{
			Console.Write("[");
			int i = 0;
			for (; i < ll.Count - 1; i++)
			{
				Print(ll[i]);
				Console.Write(',');
			}
			if (i < ll.Count)
			{
				Print(ll[i]);
			}
			Console.Write("]");
		}
		public static bool IsEqual<T>(T A, T B) where T : IEquatable<T>
		{
			return A.Equals(B);
		}

		//public static bool IsMirror<T>(IList<T> A, IList<T> B) where T : IEquatable<T>
		//{
		//	if (A is null)
		//	{
		//		return (B is null);
		//	}
		//	if (B is null)
		//	{
		//		return false;
		//	}
		//	if (A.Count != B.Count)
		//	{
		//		return false;
		//	}
		//	var len = A.Count;
		//	var la = A.ToList();
		//	var lb = B.ToList();
		//	for (int j = 0; j < len; j++)
		//	{
		//		if (!la[j].Equals(lb[j]))
		//		{
		//			return false;
		//		}
		//	}
		//	return true;
		//}
		public static bool IsMirror2<T>(IList<IList<T>> A, IList<IList<T>> B)
		{
			if (A.Count != B.Count)
			{
				return false;
			}
			for (int i = 0; i < A.Count; i++)
			{
				var a = A[i];
				var b = B[i];
				if (!a.SequenceEqual(b))//IsMirror(a, b)
				{
					return false;
				}
			}
			return true;
		}
		public static bool AreSameSet<T>(IList<T> A, IList<T> B) where T : IEquatable<T>
		{
			if (A.Count != B.Count)
			{
				return false;
			}
			var len = A.Count;
			var la = A.ToList();
			la.Sort();
			var lb = B.ToList();
			lb.Sort();
			for (int j = 0; j < len; j++)
			{
				if (!la[j].Equals(lb[j]))
				{
					return false;
				}
			}
			return true;
		}
		static int Comparison_ILIST<T>(IList<T> A, IList<T> B) where T : IComparable<T>
		{
			if (A.Count < B.Count)
			{
				return -1;
			}
			if (A.Count > B.Count)
			{
				return 1;
			}
			for (int j = 0; j < A.Count; j++)
			{
				var c = B[j].CompareTo(A[j]);
				switch (c)
				{
					case 0:
						break;
					default:
						return c;
				}
			}
			return 0;
		}
		static List<T> SortList<T>(IList<T> A)
		{
			var list1 = A.ToList();
			list1.Sort();
			return list1;
		}
		static List<IList<T>> SortListList<T>(IList<IList<T>> A) where T : IComparable<T>
		{
			var list1 = A.ToList();
			foreach (List<int> item in list1)
			{
				item.Sort();
			}
			list1.Sort(Comparison_ILIST);
			return list1;
		}
		public static void S<T>(
		IList<T> expect,
		IList<T> actual
			) where T : IComparable<T>, IEquatable<T>
		{
			var sorted_expect = SortList(expect);
			var sorted_actual = SortList(actual);
			if (sorted_expect.SequenceEqual(sorted_actual))
			{
				//COLOR_PRINT(ConsoleColor.Green, () => { Print(" pass"); });
			}
			else
			{
				Print(sorted_expect);
				Console.Write(" != ");
				COLOR_PRINT(ConsoleColor.Red, () =>
				{
					Print(sorted_actual);
				});
				Console.WriteLine();
#if MSTEST
				Assert.Fail();
#endif
			}
		}
		public static void S<T>(
		IList<IList<T>> expect,
		IList<IList<T>> actual
			) where T : IComparable<T>, IEquatable<T>
		{
			var sorted_expect = SortListList(expect);
			var sorted_actual = SortListList(actual);
			if (IsMirror2(sorted_expect, sorted_actual))
			{
				//COLOR_PRINT(ConsoleColor.Green, () => { Print(" pass"); });
			}
			else
			{
				Print(sorted_expect);
				Console.Write(" != ");
				COLOR_PRINT(ConsoleColor.Red, () =>
				{
					Print(sorted_actual);
				});
				Console.WriteLine();
#if MSTEST
				Assert.Fail();
#endif
			}
		}
		public static void T<X>(
		X actual,
		X expect
		) where X : IEquatable<X>
		{
			if (expect.Equals(actual))
			{
				//COLOR_PRINT(ConsoleColor.Green, () => { Print(" pass"); });
			}
			else
			{
				Console.Write("expect:");
				Print(expect);
				Console.Write(" !=actural:");
				COLOR_PRINT(ConsoleColor.Red, () =>
				{
					Print(actual);
				});
				Console.WriteLine();
#if MSTEST
				Assert.Fail();
#endif
			}
		}
		public static void T<X>(
		IList<X> expect,
		IList<X> actual)
		{
			if (expect.SequenceEqual(actual))
			{
				//COLOR_PRINT(ConsoleColor.Green, () => { Print(" pass"); });
			}
			else
			{
				Print(expect);
				Console.Write(" != ");
				COLOR_PRINT(ConsoleColor.Red, () =>
				{
					Print(actual);
				});
				Console.WriteLine();
#if MSTEST
				Assert.Fail();
#endif
			}
		}
		public static void T<X>(
		IList<IList<X>> expect,
		IList<IList<X>> actual) where X : IEquatable<X>
		{
			if (IsMirror2(expect, actual))
			{
				//COLOR_PRINT(ConsoleColor.Green, () => { Print(" pass"); });
			}
			else
			{
				Print(expect);
				Console.Write(" != ");
				COLOR_PRINT(ConsoleColor.Red, () =>
				{
					Print(actual);
				});
				Console.WriteLine();
#if MSTEST
				Assert.Fail();
#endif
			}
		}
	}
}
