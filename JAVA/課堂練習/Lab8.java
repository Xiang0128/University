public class Lab8 {
    public static void main(String[] args) {
        star(5);
        star(5,7);
        star(1,3,5);
    }

    public static void star(int n) {
        int sum = 0;
        StringBuilder sumExpression = new StringBuilder();
        for(int i = 1; i <= n; i++){
            sum += i;
            sumExpression.append(i);
            if (i < n) {
                sumExpression.append("+");
            }
        }
        System.out.println(sumExpression + "=" + sum);
    }

    public static void star(int a, int b) {
        int max = (a > b) ? a : b;
        System.out.println(a + "," + b + " The larger value is: " + max);
    }

    public static void star(int a, int b, int c) {
        int result = a * b * c;
        System.out.println(a + "*" + b + "*" + c + "=" + result);
    }
}
