import java.util.Scanner;
public class s1114572_Midterm_1 {
    public static void main(String[] args) {
        Scanner scn = new Scanner(System.in);
        int size = 0;
        System.out.printf("請輸入矩陣尺寸(最大為5): ");
        while(size > 5 || size <= 0){
            size = scn.nextInt();
            if(size > 5 || size <= 0)
            System.out.printf("請輸入矩陣尺寸(最大為5): ");
        }
        String[][] matrix = new String[size][size];


        System.out.println("請逐一輸入矩陣內容(一次輸入一個字元): ");
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                matrix[i][j] = scn.next();
            }
        }
        String[][] transposeMatrix = transpose(matrix);
        printMatrix(transposeMatrix);
    }

    public static void printMatrix(String[][] matrix) {
        for (int i = 0; i < matrix.length; i++) {
            for (int j = 0; j < matrix[i].length; j++) {
                System.out.print(matrix[i][j]);
            }
            System.out.println();
        }
    }

        public static String[][] transpose(String[][] matrix) {
            int rows = matrix.length;
            int cols = matrix[0].length;
            String[][] transposeMatrix = new String[cols][rows];
    
            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    transposeMatrix[i][j] = matrix[j][i];
                }
            }
            return transposeMatrix;
        }
    }