class Caaa {
    private int num;
    public Caaa(int n) {
        num = n;
    }
    public Caaa(){

    }
    public int get() {
        return num;
    }
    public void display() {
        System.out.println("printed from Caaa class");
    }
}

class Cbbb extends Caaa {
    public Cbbb(int n) {
        super(n);
    }
    public Cbbb(){

    }
    public void show() {
        System.out.println("num=" + get());
    }
    public void display() {
        System.out.println("printed from Cbbb class");
    }
}

public class Lab_15 {
    public static void main(String args[]) {
        Cbbb aa = new Cbbb(5);
        aa.show();

        Caaa bb = new Cbbb();
        bb.display();
    }
}