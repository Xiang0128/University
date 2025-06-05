class Cwin{
    static int cnt = 0;
    String color;
    int width;
    int height;

    public Cwin(String str, int w, int h){
        color = str;
        width = w;
        height = h;
        cnt++;
    }

    public Cwin(){
        color = "red";
        width = 2;
        height = 2;
        cnt++;
    }

    public static void setZero(){
        cnt = 0;
    }

    public static void setValue(int n){
        cnt = n;
    }

    public static int getCount(){
        return cnt;
    }
}

public class Lab12 {
    public static void main(String[] args) {
        Cwin obj1 = new Cwin();
        Cwin obj2 = new Cwin();
        System.out.println("cnt=" + Cwin.getCount());
        Cwin.setZero();
        System.out.println("cnt=" + Cwin.getCount());
        Cwin.setValue(2);
        System.out.println("cnt=" + Cwin.getCount());
        Cwin obj3 = new Cwin();
        System.out.println("cnt=" + Cwin.getCount());
    }
}
