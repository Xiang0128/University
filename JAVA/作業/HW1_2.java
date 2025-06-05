import java.util.Scanner;

public class HW1_2 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.print("請輸入原始矩陣的行數：");
        int cols = scanner.nextInt();
        System.out.print("請輸入原始矩陣的列數：");
        int rows = scanner.nextInt();

        int[][] matrix = new int[rows][cols];

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                System.out.print("A[" + i + "][" + j + "]元素值=>");
                matrix[i][j] = scanner.nextInt();
            }
        }

        System.out.println("原始矩陣：");
        printMatrix(matrix);

        int[][] transposeMatrix = transpose(matrix);

        System.out.println("轉置矩陣：");
        printMatrix(transposeMatrix);
    }

    public static int[][] transpose(int[][] matrix) {
        int rows = matrix.length;
        int cols = matrix[0].length;
        int[][] transposeMatrix = new int[cols][rows];

        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                transposeMatrix[i][j] = matrix[j][i];
            }
        }

        return transposeMatrix;
    }

    public static void printMatrix(int[][] matrix) {
        for (int i = 0; i < matrix.length; i++) {
            for (int j = 0; j < matrix[i].length; j++) {
                System.out.print(matrix[i][j] + " ");
            }
            System.out.println();
        }
    }
}
