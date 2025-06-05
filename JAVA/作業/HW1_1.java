import java.util.Scanner;

public class HW1_1 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.print("請輸入一個正整數：");
        int n = scanner.nextInt();
        
        int[][] square = generateNumberSquare(n);
        printNumberSquare(square);
    }
    
    public static int[][] generateNumberSquare(int n) {
        int[][] square = new int[2 * n - 1][2 * n - 1];
        for (int i = 0; i < 2 * n - 1; i++) {
            for (int j = 0; j < 2 * n - 1; j++) {
                square[i][j] = n - Math.min(Math.min(i, j), Math.min(2 * n - 2 - i, 2 * n - 2 - j));
            }
        }
        return square;
    }
    
    public static void printNumberSquare(int[][] square) {
        for (int i = 0; i < square.length; i++) {
            for (int j = 0; j < square[i].length; j++) {
                System.out.print(square[i][j]);
            }
            System.out.println();
        }
    }
}
