//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.

import java.util.regex.*;

class Solution {
    Pattern re;
    public Solution() {
        re = Pattern.compile("^\\s*?([+-]?0*\\d{1,11})");//, Pattern.MULTILINE| Pattern.CASE_INSENSITIVE
    }

    public int myAtoi(String s) {
        //var ss = """Hello\nWorld""";
        final int MIN_LEN = 0;
        final int MAX_LEN = 200;
        var i = 0;

        var matchObj = re.matcher(s);
        if (matchObj.find()) { //Matcher.matches is exact match
            s = matchObj.group(1);
            var l = Long.parseLong(s);
            if (l > Integer.MAX_VALUE) { //(2**31-1) 2147483647
                i = Integer.MAX_VALUE;
            } else if (l < Integer.MIN_VALUE) { //(-2**31) -2147483648
                i = Integer.MIN_VALUE;
            } else {
                i = (int) l;
            }
            return i;
        }
        return 0;
    }
}

public class Main {
    public static void main(String[] args) {
        //TIP Press <shortcut actionId="ShowIntentionActions"/> with your caret at the highlighted text
        // to see how IntelliJ IDEA suggests fixing it.
        System.out.printf("Hello and welcome!");

        for (int i = 1; i <= 5; i++) {
            //TIP Press <shortcut actionId="Debug"/> to start debugging your code. We have set one <icon src="AllIcons.Debugger.Db_set_breakpoint"/> breakpoint
            // for you, but you can always add more by pressing <shortcut actionId="ToggleLineBreakpoint"/>.
            System.out.println("i = " + i);
        }
    }
}