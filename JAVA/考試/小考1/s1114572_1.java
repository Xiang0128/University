import java.util.Scanner;

public class s1114572_1 {
    public static void main(String args[]) {
        Scanner scn = new Scanner(System.in);
        int size;
        System.out.print("輸入:");
        size = scn.nextInt();
        square(size);
    }

    public static void square(int a) {
        for (int i = 1; i <= a; i++) {
            if (i == 1 || i == (2 * a) - 1) {
                for (int l = 1; l <= (2 * a - 1); l++) {
                    if (a % 2 == 0)
                        System.out.print("*");
                    else
                        System.out.print(a);
                }
            } else {
                int b = a;
                int c = a - (i - 2);
                for (int k = 1; k <= i - 1; k++) {
                    if (b % 2 == 0)
                        System.out.print("*");
                    else
                        System.out.print(b);
                    b--;
                }
				if ((a - i + 1) % 2 == 0)
				System.out.print("0");
                for (int l = 1; l <= (2 * a - 1) - 2 * (i - 1); l++) {
					if ((a - i + 1) % 2 == 0)
					System.out.print("");
                    else
                        System.out.print(a - i + 1);
                }
				for (int l = 1; l <= ((2 * a - 1) - 2 * (i - 1)) - 2; l++) {
					if ((a - i + 1) % 2 == 0)
				System.out.print("*");
			}
				if ((a - i + 1) % 2 == 0)
				System.out.print("0");
                for (int k = i - 1; k >= 1; k--) {
                    if (c % 2 == 0)
                        System.out.print("*");
                    else
                        System.out.print(c);
                    c++;
                }
			}
            System.out.print("\n");
        }

        for (int i = a - 1; i >= 1; i--) {
            if (i == (2 * a) - 1) {
                for (int l = 1; l <= (2 * a - 1); l++) {
                    if (a % 2 == 0)
                        System.out.print("*");
                    else
                        System.out.print(a);
				}
            } else {
                int b = a;
                int c = a - (i - 2);
                for (int k = 1; k <= i - 1; k++) {
                    if (b % 2 == 0)
                        System.out.print("*");
                    else
                        System.out.print(b);
                    b--;
                }
				if ((a - i + 1) % 2 == 0)
				System.out.print("0");
                for (int l = 1; l <= (2 * a - 1) - 2 * (i - 1); l++) {
                    if ((a - i + 1) % 2 == 0)
                        System.out.print("");
                    else
                        System.out.print(a - i + 1);
                }
				for (int l = 1; l <= ((2 * a - 1) - 2 * (i - 1)) - 2; l++) {
					if ((a - i + 1) % 2 == 0)
                        System.out.print("*");
				}
				if ((a - i + 1) % 2 == 0)
				System.out.print("0");
				for (int k = i - 1; k >= 1; k--) {
                    if (c % 2 == 0)
                        System.out.print("*");
                    else
                        System.out.print(c);
                    c++;
                }
            }
            System.out.print("\n");
        }
    }
}