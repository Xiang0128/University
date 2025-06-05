import java.math.BigInteger;
class CCalculator{
    private int a, b, c, d;
    
    public void set_value(int w, int x, int y,int z){
        a = w;
        b = x;
        c = y;
        d = z;
    }
    
    public void show(){
        System.out.println("a=" + a + ", b=" + b + ", c=" + c + ", d=" + d);
    }

    public int add() {
        int sum = 0;
        for (int i = 1; i <= a; i++) {
            sum += i;
        }
        return sum;
    }

    public int sub() {
        return b - add();
    }

    public long mul() {
        long result = 1;
        for (int i = 1; i <= c; i++) {
            result *= i;
        }
        return result;
    }

    public double avg() {
        return (double) mul() / d;
    }
}

public class Lab10{
    public static void main(String[] args) {
        CCalculator calc = new CCalculator();
        calc.set_value(5, 30, 17, 10);
        calc.show();
        System.out.println("1+2+...+a = " + calc.add());
        System.out.println("b-(1+2+...+a) = " + calc.sub());
        System.out.println("c! = " + calc.mul());
        System.out.println("c!/d = " + calc.avg());
    }
}
