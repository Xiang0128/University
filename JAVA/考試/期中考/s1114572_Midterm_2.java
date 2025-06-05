import java.util.Scanner;

public class s1114572_Midterm_2 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        System.out.println("輸入(輸入exit結束輸入）：");
        int sum = 0;
        boolean isFirstLine = true;
        while (true) {
            String line = scanner.nextLine();
            if (line.equals("exit")) {
                break;
            }

            String[] numbers = line.split(" ");
            if (isFirstLine) {
                isFirstLine = false;
                continue;
            }
            int max = Integer.MIN_VALUE;
            for (String numStr : numbers) {
                int num = Integer.parseInt(numStr);
                if (num > max) {
                    max = num;
                }
            }
            sum += max;
        }
        System.out.println(sum);
    }
}
