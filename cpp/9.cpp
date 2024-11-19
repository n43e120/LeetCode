#include <sstream>
class Solution {
public:
	bool isPalindrome(int x) {
		if (x < 0) return false;
		if (x < 10) return true;
		std::ostringstream oss;
		oss << x;
		std::string result = oss.str();
		auto buffer = result.data();
		auto len = result.length(); //strlen(buffer);
		auto halflen = len / 2;
		char* head = buffer;
		char* tail = buffer + len - 1;
		for (size_t i = 0; i < halflen; i++)
		{
			if (*head != *tail) return false;
			head++;
			tail--;
		}
		return true;
	}
};

//#define _CRT_SECURE_NO_WARNINGS
//#include <cstdlib>
//#include <string.h>
//class Solution {
//public:
//	bool isPalindrome(int x) {
//		if (x < 0) return false;
//		if (x < 10) return true;
//		auto buffer = (char*)malloc(12);
//		auto s = _itoa(x, buffer, 10);
//		auto len = strlen(s);
//		auto halflen = len / 2;
//		char* head = buffer;
//		char* tail = buffer + len - 1;
//		for (size_t i = 0; i < halflen; i++)
//		{
//			if (*head != *tail) return false;
//			head++;
//			tail--;
//		}
//		return true;
//	}
//};
