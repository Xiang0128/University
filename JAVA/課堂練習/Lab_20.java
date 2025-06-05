public class Lab_20 {
    public static void main(String args[]) {
        int num = 12;
        int den[] = {12, 0, 3, 0, 0, 4};

        for (int i = 0; i < 10; i++) {
            try {
                System.out.println("ans=" + num / den[i]);
            } catch (ArithmeticException e) {
                System.out.println("Division by zero error occurred.");
            } catch (ArrayIndexOutOfBoundsException e) {
                System.out.println("Array index out of bounds error occurred.");
            }
        }
    }
}
