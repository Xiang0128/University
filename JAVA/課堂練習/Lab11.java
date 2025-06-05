class CRectangle {
    int width;
    int height;

    public CRectangle(int w, int h) {
        System.out.println("constructor CRectangle(int w, int h) called");
        width = w;
        height = h;
    }

    public CRectangle() {
        this(0, 0);
        System.out.println("constructor CRectangle() called");
        this.width=10;
        this.height=8;
    }
}

public class Lab11 {
    public static void main(String[] args) {
        System.out.println("/* P.9-34 Q1 output-------------------------------------------");
        System.out.println();
        System.out.println("/* object rec1");
        CRectangle rec1 = new CRectangle(5, 2);
        System.out.println("width=" + rec1.width);
        System.out.println("height=" + rec1.height);
        System.out.println();

        System.out.println("/* object rect2");
        CRectangle rect2 = new CRectangle();
        System.out.println("width=" + rect2.width);
        System.out.println("height=" + rect2.height);
        System.out.println();
        System.out.println("---------------------------------------------------*/");
    }
}
