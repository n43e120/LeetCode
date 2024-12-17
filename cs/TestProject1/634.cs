file class Solution
{
	public int Derangement(int n)//leetcode 634
	{
		return Combinatorics.Derangement.Count(n);
	}
}
[TestClass]
public sealed class Test634
{
	[TestMethod]
	public void TestMethod1()
	{
		//for (int i = 0; i < 8; i++)
		//{
		//	Console.WriteLine(Combinatorics.Derangement.Count(i));
		//}
		//Assert.Fail();
		Solution x = new();
		var f = x.Derangement;
		T(f(1), 0);
		T(f(2), 1);
		T(f(3), 2);
		T(f(4), 9);
		T(f(5), 44);
		T(f(6), 265);
	}
}