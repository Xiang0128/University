class Circle {
    private static double pi = 3.14;
    private double radius;

    public Circle(double r) {
        radius = r;
    }

    public double getArea() {
        return pi * radius * radius;
    }

    public static double totalArea(Circle[] c) {
        double total = 0.0;
        for (int i = 0; i < c.length; i++) {
            total += c[i].getArea();
        }
        return total;
    }
}

public class Lab13 {
    public static void main(String[] args) {
        Circle[] cir = new Circle[3];

        cir[0] = new Circle(1.0);
        cir[1] = new Circle(4.0);
        cir[2] = new Circle(2.0);

        double totalArea = Circle.totalArea(cir);
        System.out.println("三個圓物件的總面積 = " + totalArea);
    }
}