class Electricity {
    int Power;      // 瓦數
    int Hour;      // 用電時數
    double Degree;  // 使用度數

    // 印出
    void show() {
        System.out.print("瓦數: " + Power + "(w)，用電時數: " + Hour + "(H)，使用度數: " + Degree + "度，");
    }

    // 依照傳入的參數設定Power資料
    void setPower(int Power) {
        this.Power = Power;
    }

    // 依照傳入的參數設定Hour資料
    void setHour(int Hour) {
        this.Hour = Hour;
    }

    // 依照傳入的參數設定資料
    // 字元為'P', 設定瓦數 ; 字元為'H' , 設定用電時數
    void setData(char choose, int data) {
        if (choose == 'P') {
            setPower(data);
        } else if (choose == 'H') {
            setHour(data);
        }
    }

    // 將瓦數和用電時數經過公式計算後存回Degree
    // Degree = (瓦數 * 用電時數(時)) / 1000
    void calculate() {
        Degree = (Power * Hour) / 1000.0;
    }
}

public class s1114572_Final_1 {
    public static void main(String[] args) {
        Electricity Meter_1 = new Electricity();
        Electricity Meter_2 = new Electricity();
        
        // 使用setPower()與setHour()設定Meter1
        Meter_1.setPower(15609);
        Meter_1.setHour(325);
        Meter_1.calculate();
        System.out.print("Meter_1 ");
        Meter_1.show();
        System.out.println("電費: " + calPrice(Meter_1.Degree) + "元");

        // 使用setData()設定Meter2
        Meter_2.setData('P', 2367);
        Meter_2.setData('H', 128);
        Meter_2.calculate();
        System.out.print("Meter_2 ");
        Meter_2.show();
        System.out.println("電費: " + calPrice(Meter_2.Degree) + "元");
    }

    // 計算電費
    public static double calPrice(double Degree) {
        double price = 0;
        if (Degree <= 1000) {
            price = Degree * 3.5;
        } else if (Degree <= 3000) {
            price = 1000 * 3.5 + (Degree - 1000) * 4.2;
        } else {
            price = 1000 * 3.5 + 2000 * 4.2 + (Degree - 3000) * 5.4;
        }
        return price;
    }
}