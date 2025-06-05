import java.util.Scanner;

public class Lab7 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.print("請輸入一個正整數 n：");
        int n = scanner.nextInt();
        scanner.close();

        int result = addto(n);
        System.out.printf("%d", result);
    }

    public static int addto(int n) {
        int sum = 0;
        System.out.printf("Input=" + n + " , ");
        for (int i = 1; i <= n; i += 2) {
            if (i < n) {
                System.out.printf("%d+", i);
                sum += i;
            } else {
                System.out.printf("%d", i);
                sum += i;
            }
        }
        System.out.printf("=");
        return sum;
    }
}
