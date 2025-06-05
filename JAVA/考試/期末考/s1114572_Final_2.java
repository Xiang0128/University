import java.io.Console;
import java.util.Scanner;

class CPersonData
{
    public String name;     // 姓名
    public String degree;   // 職別
    private String p_id;    // 身分證號碼
    private int salary;     // 月薪資

    // (a) 無參數的建構子
    public CPersonData()
    {
        this.name = "~~~~~~~~";
        this.degree = "~~~~~~~~";
        this.p_id = "~~~~~~~~";
        this.salary = 0;
    }

    // (b) 包含4個參數的建構子
    public CPersonData(String Name, String Degree, String ID, int Salary)
    {
        this.name = Name;
        this.degree = Degree;
        this.p_id = ID;
        this.salary = Salary;
    }

    void setName(String n)
    {
        this.name = n;
    }

    void setDegree(String d)
    {
        this.degree = d;
    }

    void setP_id(String id)
    {
        this.p_id = id;
    }

    void setSalary(int s)
    {
        this.salary = s;
    }

    // (c) int getSalary()方法
    int getSalary()
    {
        return this.salary;
    }

    String getName()
    {
        return this.name;
    }

    String getDegree()
    {
        return this.degree;
    }
}

class CPersonAccount extends CPersonData
{
    private CPersonData personArr[];
    Scanner scn = new Scanner(System.in);

    // (d) 無參數的建構子
    public CPersonAccount()
    {
        this.personArr = new CPersonData[5];
        for(int i = 0; i < 5; i++)
        {
            personArr[i] = new CPersonData();
        }
    }

    // (e) 方法 void inputData()
    void inputData()
    {
        for (int i = 1; i < personArr.length; i++)
        {
            if (personArr[i].getName() == "~~~~~~~~")
            {
                System.out.print("請輸入姓名:");
                personArr[i].setName(scn.next());
                System.out.print("請輸入職別:");
                personArr[i].setDegree(scn.next());
                System.out.print("請輸入身分證字號:");
                personArr[i].setP_id(scn.next());
                System.out.print("請輸入月薪資:");
                personArr[i].setSalary(scn.nextInt());
                break;
            }
        }
    }

    // (f) 方法 void displayData()
    void displayData()
    {
        System.out.println("姓名        職別        月薪資        ");
        for (int i = 0; i < personArr.length; i++)
        {
            System.out.printf("%-12s%-12s%-12d\n", personArr[i].getName(),
                            personArr[i].getDegree(), personArr[i].getSalary());
        }
    }

    // (g) 方法 void sortBySalary()
    void sortBySalary()
    {
        for(int i = 0; i < personArr.length-1; i++)
        {
            for(int j = 0; j < personArr.length-1; j++){
                if(personArr[j+1].getSalary() < personArr[j].getSalary())
                {
                    CPersonData temp = personArr[j];
                    personArr[j] = personArr[j+1];
                    personArr[j+1] = temp;
                }
            }
        }
    }
}

public class s1114572_Final_2{
    public static void main(String args[])
    {
        CPersonAccount obj = new CPersonAccount();
        char runFunc = 'd';
        Console console = System.console();

        while (runFunc != 'q')
        {
            System.out.print("請選擇作業");
            System.out.print("(i = 輸入資料, s = 依月薪資排序, d = 顯示資料, q = 離開):");
            runFunc = console.readLine().charAt(0);
            switch (runFunc)
            {
                case'i':
                    obj.inputData();
                    break;
                case's':
                    obj.sortBySalary();
                    break;
                case'd':
                    obj.displayData();
                    break;
                default:
                    break;
            }
        }
    }
}
