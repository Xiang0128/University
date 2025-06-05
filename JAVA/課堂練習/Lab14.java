class Rectangle {
    private int length;
    private int width;

    public Rectangle(int len, int wid) {
        length = len;
        width = wid;
    }

    public Rectangle() {
        this(2, 2);
    }

    private void show(){
        System.out.print("length="+length) ;
        System.out.print(", width="+width);
    }

    public void area() {
        int area = length * width;
        show();
        System.out.println(", area=" + area);
    }
}

class Data extends Rectangle {
    public Data(int len, int wid) {
        super(len, wid);
    }
    public Data() {
        super();
    }
}

public class Lab14 {
    public static void main(String args[]) {
        Data obj1 = new Data(3, 8);
        Data obj2 = new Data();
        obj1.area();
        obj2.area();
    }
}