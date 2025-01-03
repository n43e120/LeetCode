# N-Queens Problem C# Solution Bitmap Approach
2025/1/3 
My (C#) solution uses Bitmap to simulate queens' threaten squares.
It is a general solution which can solve for any n and any piece.Rook and queen and more.
And it can return non-isomorphic group information.
Rotate and flip solutions are in one group.
Complete debug tools.
code size:~35KB.
I think those ~0m solutions on leetcode leaderboard are cheaters.

# Develop course
First, I use native ints and HashSet<uint64> to mimic bitmap to solve $n\le8$.
For $n=9$ case, I choose BitArray to extend bitmap size at first.
It needs unsafe reflection hack.
Then I turned to sparsed byte array to store queens xy position in one byte.
Debugging is painful, there are so many and so spooky.
It took me about a week day and night to finally finish.
Otherwise it is just about few days.
Bugs make me real pain and frustrated.
# Complexity
- Time complexity:
$$O(n!)$$, about 230ms.


- Space complexity:
$$O(n)$$

# C# Code
[https://github.com/n43e120/LeetCode/blob/main/cs/TestProject1/NQueen.cs](https://github.com/n43e120/LeetCode/blob/main/cs/TestProject1/NQueen.cs)