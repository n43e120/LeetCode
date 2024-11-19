int lengthOfLongestSubstring(char* s) {
	int len = strlen(s);
	if (len <= 1)
	{
		return len;
	}
	int h = 0; //head idx pointer
	int t = 1; //tail idx pointer
	int m = 1; //global max len record
	int i = 0;
	char c = s[t];
	do
	{
		if (s[i] != c)
		{
			i--;
			if (i < h) //reach start
			{
			MOVE_TAIL:
				i = t;
				t++;
				if (t < len) {
					c = s[t];
					continue;
				}
				else { //reach end
					int record = t - h;
					if (record > m)
					{
						m = record;
					}
					return m;
				}
			}
			continue;
		}
		else
		{ //found repeat
			int record = t - h;
			if (record > m)
			{
				m = record;
			}
			h = i + 1;
			goto MOVE_TAIL;
		}
	} while (1);

	return m;
}