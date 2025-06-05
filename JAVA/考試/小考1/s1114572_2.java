import java.util.Scanner;

public class s1114572_2 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.print("請輸入走訪順序(0代表左，1代表上，2代表右，3代表下): ");
        String sequence = scanner.next();

        System.out.print("請輸入二維陣列的行數: ");
        int rows = scanner.nextInt();
        System.out.print("請輸入二維陣列的列數: ");
        int cols = scanner.nextInt();
        
        int[][] array = new int[rows][cols];
        
        System.out.println("請輸入二維陣列的元素:");
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                array[i][j] = scanner.nextInt();
            }
        }
        
        scanner.close();
    }
    
    public static String traverseArray(int[][] array, String sequence) {
        int rows = array.length;
        int cols = array[0].length;
        StringBuilder sb = new StringBuilder();
        
        int row = rows / 2;
        int col = cols / 2;
        int direction = 0; 
        

        for (int i = 0; i < rows * cols; i++) {
            sb.append(array[row][col]).append(" ");
            switch (sequence.charAt(direction)) {
                case '0':
                    if (direction % 2 == 0) { 
                        row++;
                    } else { 
                        col--;
                    }
                    break;
                case '1':
                    if (direction % 2 == 0) {
                        col--;
                    } else { 
                        row--;
                    }
                    break;
                case '2':
                    if (direction % 2 == 0) {
                        row--;
                    } else {
                        col++;
                    }
                    break;
                case '3':
                    if (direction % 2 == 0) { 
                        col++;
                    } else { 
                    }
                    break;
            }
            direction = (direction + 1) % 4;
        }
        
        return sb.toString();
    }
}

