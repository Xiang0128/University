// Lab_17,抽象類別的範例
abstract class Math {
    protected int ans;
    public void show() {
        System.out.println("ans=" + ans);
    }
    public abstract void add(int a, int b); // 計算a+b
    public abstract void sub(int a, int b); // 計算a-b
    public abstract void mul(int a, int b); // 計算a*b
    public abstract void div(int a, int b); // 計算a/b
}

class Compute extends Math {
    public Compute() {
        this.ans = 0;
    }
    public void add(int a, int b) {
        ans = a + b;
    }
    public void sub(int a, int b) {
        ans = a - b;
    }
    public void mul(int a, int b) {
        ans = a * b;
    }
    public void div(int a, int b) {
        if (b != 0) {
            ans = a / b;
        } else {
            System.out.println("Error: Division by zero");
            ans = 0;
        }
    }
}

public class Lab_17 {
    public static void main(String args[]) {
        Compute cmp = new Compute();
        cmp.mul(3, 5); // 計算3*5
        cmp.show(); // 此行會回應"ans=15"字串
    }
}
