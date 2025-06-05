class CShape {
    public double area() {
        return 0.0;
    }
}

class CCircle extends CShape {
    private double radius;

    public CCircle(double r) {
        radius = r;
    }

    public double area() {
        return Math.PI * radius * radius;
    }
}

class CSquare extends CShape {
    private double side;

    public CSquare(double s) {
        side = s;
    }

    public double area() {
        return side * side;
    }
}

class CTriangle extends CShape {
    private double base;
    private double height;

    public CTriangle(double b, double h) {
        base = b;
        height = h;
    }

    public double area() {
        return 0.5 * base * height;
    }
}

public class Lab_16 {
    public static void main(String[] args) {
        CShape[] shapes = new CShape[6];
        shapes[0] = new CCircle(5);
        shapes[1] = new CCircle(7);
        shapes[2] = new CSquare(4);
        shapes[3] = new CSquare(6);
        shapes[4] = new CTriangle(3, 4);
        shapes[5] = new CTriangle(5, 6);

        double maxArea = largest(shapes);
        System.out.println("Largest area: " + maxArea);
    }

    public static double largest(CShape[] shapes) {
        double maxArea = 0.0;
        for (CShape shape : shapes) {
            double area = shape.area();
            if (area > maxArea) {
                maxArea = area;
            }
        }
        return maxArea;
    }
}