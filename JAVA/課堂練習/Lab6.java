public class Lab6 {
    public static void main(String[] args) {
        int[][] scores = {
                { 83, 90, 76, 83, 89 },
                { 68, 88, 92, 60, 76 },
                { 79, 93, 67, 91, 78 }
        };

        int[] dailyAverage = new int[5];
        for (int i = 0; i < 5; i++) {
            int sum = 0;
            for (int[] student : scores) {
                sum += student[i];
            }
            dailyAverage[i] = sum / scores.length;
        }

        int[] studentAverage = new int[scores.length];
        for (int i = 0; i < scores.length; i++) {
            int sum = 0;
            for (int score : scores[i]) {
                sum += score;
            }
            studentAverage[i] = sum / scores[i].length;
        }

        int maxIndex = getMaxIndex(studentAverage);
        int minIndex = getMinIndex(studentAverage);

        int maxDayIndex = getMaxIndex(dailyAverage);
        int maxStudentIndex = getMaxIndex(scores[maxDayIndex]);

        int minDayIndex = getMinIndex(scores[minIndex]);
        int minStudentIndex = minIndex;

        System.out.println("       星期一 星期二 星期三 星期四 星期五   平均");
        for (int i = 0; i < scores.length; i++) {
            System.out.print("同學" + convertToChineseNumber(i + 1) + "     ");
            for (int j = 0; j < scores[i].length; j++) {
                System.out.print(scores[i][j] + "     ");
            }
            System.out.println(studentAverage[i]);
        }

        System.out.print("每日平均   ");
        for (int value : dailyAverage) {
            System.out.print(value + "     ");
        }
        System.out.println();

        System.out.println("第一名的同學為: 同學" + convertToChineseNumber(maxIndex + 1));
        System.out.println("最後一名同學為: 同學" + convertToChineseNumber(minIndex + 1));
        System.out.println("成績最高的日子與同學為: 星期" + convertToChineseNumber(maxDayIndex + 1) + "的同學"
                + convertToChineseNumber(maxStudentIndex + 1));
        System.out.println("成績最低的日子與同學為: 星期" + convertToChineseNumber(minDayIndex + 1) + "的同學"
                + convertToChineseNumber(minStudentIndex + 1));
    }

    public static int getMaxIndex(int[] array) {
        int maxIndex = 0;
        int max = array[0];
        for (int i = 1; i < array.length; i++) {
            if (array[i] > max) {
                max = array[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    public static int getMinIndex(int[] array) {
        int minIndex = 0;
        int min = array[0];
        for (int i = 1; i < array.length; i++) {
            if (array[i] < min) {
                min = array[i];
                minIndex = i;
            }
        }
        return minIndex;
    }

    public static String convertToChineseNumber(int number) {
        switch (number) {
            case 1:
                return "一";
            case 2:
                return "二";
            case 3:
                return "三";
            case 4:
                return "四";
            case 5:
                return "五";
            default:
                return String.valueOf(number);
        }
    }
}
