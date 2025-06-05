import java.util.HashSet;
import java.util.TreeSet;
import java.util.Set;

public class Lab_22 {
    public static void main(String[] args) {
        HashSet<Integer> hset = new HashSet<>();
        hset.add(36);
        hset.add(15);

        TreeSet<Integer> tset = new TreeSet<>();
        tset.add(52);
        tset.add(23);
        tset.add(32);
        tset.add(69);
        tset.add(1);
        tset.add(7);
        tset.add(36);
        tset.add(15);

        System.out.println("HashSet hset: " + hset);
        System.out.println("TreeSet tset: " + tset);

        if (tset.contains(32)) {
            tset.remove(32);
            System.out.println("刪除32後, TreeSet tset: " + tset);
        } else {
            System.out.println("tset中沒有元素32");
        }

        if (tset.containsAll(hset)) {
            System.out.println("tset包含hset的所有元素");
        } else {
            System.out.println("tset不包含hset的所有元素");
        }
    }
}
