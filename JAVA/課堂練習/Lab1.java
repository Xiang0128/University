import java.util.Scanner;

public class Lab1 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.print("請輸入一個長度為3個字元的字串: ");
        String input = scanner.nextLine();

        if (input.length() == 3) {
            printAlphabetInfo(input.charAt(0), "第一個字母");
            printAlphabetInfo(input.charAt(1), "第二個字母");
            printAlphabetInfo(input.charAt(2), "第三個字母");
        } else {
            System.out.println("請正確輸入一個長度為3個字元的字串。");
        }
    }

    private static void printAlphabetInfo(char ch, String position) {
        int alphabetNumber = getAlphabetNumber(ch);

        if (alphabetNumber != -1) {
            System.out.println(position + "為" + ch + ", " + ch + "為英文中第" + alphabetNumber + "個字母");
        } else {
            System.out.println(position + "不是正確的英文字母。");
        }
    }

    private static int getAlphabetNumber(char ch) {
        if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')) {
            return Character.toLowerCase(ch) - 'a' + 1;
        } else {
            return -1;
        }
    }
}
