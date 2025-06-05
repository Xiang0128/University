public class Lab5 {
    public static void main(String[] args) {
        int[] array = {3, 5, 0, 3, 2, 4, 1, 6, 8, 5, 4, 3, 2, 7, 2, 5, 11, 12, 13, 1, 7, 11};
        
        int count1 = 0;
        for (int num : array) {
            if (num >= 3 && num <= 7) {
                count1++;
            }
        }
        System.out.println("介於 3~7 之間的元素共有 " + count1 + " 個");
        
        int count2 = 0;
        for (int num : array) {
            if (num >= 11 && num <= 12) {
                count2++;
            }
        }
        System.out.println("介於 11~12 之間的元素共有 " + count2 + " 個");
    }
}
