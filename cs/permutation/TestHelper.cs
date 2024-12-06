namespace ConsoleApp1
{
	internal static class TestHelper
	{
		public static void COLOR_PRINT(ConsoleColor color, Action f)
		{
			Console.ForegroundColor = color;
			f();
			Console.ResetColor();
		}
		public static void PRINT_LIST(IList<int> list)
		{
			Console.Write("[");
			int i = 0;
			for (; i < list.Count - 1; i++)
			{
				Console.Write(list[i]);
				Console.Write(',');
			}
			if (i < list.Count)
			{
				Console.Write(list[i]);
			}
			Console.Write("]");
		}
		public static void PRINT_LISTLIST(IList<IList<int>> ll)
		{
			Console.Write("[");
			int i = 0;
			for (; i < ll.Count - 1; i++)
			{
				PRINT_LIST(ll[i]);
				Console.Write(',');
			}
			if (i < ll.Count)
			{
				PRINT_LIST(ll[i]);
			}
			Console.Write("]");
		}
		public static bool IsEqual<T>(T A, T B) where T : IEquatable<T>
		{
			return A.Equals(B);
		}

		public static bool IsEqualSeq(IList<int> A, IList<int> B)
		{
			if (A.Count != B.Count)
			{
				return false;
			}
			var len = A.Count;
			var la = A.ToList();
			var lb = B.ToList();
			for (int j = 0; j < len; j++)
			{
				if (la[j] != lb[j])
				{
					return false;
				}
			}
			return true;
		}
		public static bool IsEqualSet(IList<int> A, IList<int> B)
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
				if (la[j] != lb[j])
				{
					return false;
				}
			}
			return true;
		}
		public static bool IsEqual(IList<IList<int>> A, IList<IList<int>> B)
		{
			if (A.Count != B.Count)
			{
				return false;
			}
			for (int i = 0; i < A.Count; i++)
			{
				var a = A[i];
				var b = B[i];
				if (!IsEqualSet(a, b))
				{
					return false;
				}
			}
			return true;
		}

		public static void Print(object x)
		{
			Console.Write(x);
		}
		public static void Print(IList<int> x)
		{
			PRINT_LIST(x);
		}
		public static void Print(IList<IList<int>> x)
		{
			PRINT_LISTLIST(x);
		}
	}
}
