// 多重繼承的練習
interface Data {
    public void showData();
}

interface Test {
    public void showScore();
    public double calcu();
}

class CStu implements Data, Test {
    protected String id; // 學號
    protected String name; // 姓名
    protected int mid; // 期中考成績
    protected int finl; // 期末考成績
    protected int common; // 平時成績

    public CStu(String id, String name, int mid, int finl, int common) {
        this.id = id;
        this.name = name;
        this.mid = mid;
        this.finl = finl;
        this.common = common;
    }

    public void show() {
        showData();
        showScore();
        System.out.println("學期成績:" + (int)calcu());
    }

    public void showScore() {
        System.out.println("期中考成績:" + mid);
        System.out.println("期末考成績:" + finl);
        System.out.println("平時成績:" + common);
    }

    public void showData() {
        System.out.println("學號:" + id);
        System.out.println("姓名:" + name);
    }

    public double calcu() {
        return (mid + finl + common) / 3.0;
    }    
}

public class Lab_18 {
    public static void main(String args[]) {
        CStu stu = new CStu("940001", "Fiona", 90, 92, 85);
        stu.show();
    }
}

