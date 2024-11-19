package main

import "fmt"

func longestPalindrome(s string) string {
	var l = len(s)
	var idx_last = l - 1
	//index are doubled for gap, but len no
	//[h,e,l,l,o] =>[0,,2,,4,,6,,8], len=5,idx last element=4,doubled mid= 4,doubled last idx =8
	//[abba] =>[0,,2,,4,,6], len=4,last idx =3,doubled mid= 3
	//l is idx(doubled) of middle element (or gap)
	gEle := func(mm int) int { //return len of longest Palindrome at middel element middel m (not doubled)
		var m = mm >> 1
		var i = 1
		for true {
			var left = m - i
			if left < 0 {
				break
			}
			var right = m + i
			if right > idx_last {
				break
			}
			if s[left] != s[right] {
				break
			}
			i++
		}
		return (i-1)<<1 + 1
	}
	gGap := func(mm int) int { //len of longest Palindrome at gap after element m
		var m = mm >> 1
		var i = 0
		for true {
			var left = (m - i)
			if left < 0 {
				break
			}
			var right = (m + i + 1)
			if right > idx_last {
				break
			}
			if s[left] != s[right] {
				break
			}
			i++
		}
		return i << 1
	}
	var len_max_possible = l
	var idx_record int
	var len_record = 1
	checkM := func(mm int, g func(int) int) bool { //if m is gap, idx is doubled
		var len_localmax = g(mm)
		if len_localmax > len_record {
			len_record = len_localmax
			idx_record = mm
			if len_record == len_max_possible { //early quit
				return true
			}
		}
		return false
	}
	var mm = idx_last
	idx_record = mm
	len_record = 1
	var i int = 0
	for true {
		if l&1 == 1 { //len is odd, middle is element
			if checkM(mm, gEle) {
				break
			}
			goto WING_TIP_IS_GAP
		} else {
			if checkM(mm, gGap) {
				break
			}
			goto WING_TIP_IS_ELEMENT
		}

	WING_TIP_IS_GAP:
		i++
		if i > mm {
			break
		}
		len_max_possible--
		if len_record == len_max_possible { //early quit
			break
		}
		if checkM(mm-i, gGap) {
			break
		}
		if checkM(mm+i, gGap) {
			break
		}
	WING_TIP_IS_ELEMENT:
		i++
		if i > mm {
			break
		}
		len_max_possible--
		if len_record == len_max_possible { //early quit
			break
		}
		if checkM(mm-i, gEle) {
			break
		}
		if checkM(mm+i, gEle) {
			break
		}
		goto WING_TIP_IS_GAP

	}
	if len_record&1 == 1 {
		i = idx_record>>1 - (len_record >> 1)
	} else {
		i = idx_record>>1 - (len_record >> 1) + 1
	}
	return s[i : i+len_record]
}
func test(input string, expect string) {
	var output = longestPalindrome(input)
	if expect == output {
		fmt.Println(input, output)
	} else {
		fmt.Println(input, "expect=", expect, "output=", output)
	}
}
func main() {
	test("babad", "aba")

	test("a", "a")
	test("ab", "a")
	test("aba", "aba")
	test("abc", "b")
	test("abba", "abba")
	test("hello", "ll")
}

//baidu AI回复：golang国内镜像
//go env -w GOPROXY=https://mirrors.aliyun.com/goproxy/,direct

//https://blog.csdn.net/weixin_43064185/article/details/123797508
//go env -w GO111MODULE=on
//go env  -w GOPROXY=https://goproxy.cn,direct

/* https://goproxy.baidu.com/
go env -w GONOPROXY=\*\*.baidu.com\*\*              ## 配置GONOPROXY环境变量,所有百度内代码,不走代理
go env -w GONOSUMDB=\*                              ## 配置GONOSUMDB,暂不支持sumdb索引
go env -w GOPROXY=https://goproxy.bj.bcebos.com         ## 配置GOPROXY,可以下载墙外代码
*/

//https://goproxy.bj.bcebos.com
