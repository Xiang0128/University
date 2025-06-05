import java.util.LinkedList;
import java.util.ArrayList;

public class Lab_23 {
    public static void main(String[] args) {
        LinkedList<String> llist = new LinkedList<>();
        llist.add("apple");
        llist.add("guava");
        System.out.println("LinkedList llist: " + llist);

        ArrayList<String> alist = new ArrayList<>();
        alist.add("tomato");
        alist.add("apple");
        alist.add("papaya");
        alist.add("grape");
        System.out.println("ArrayList alist: " + alist);

        alist.addAll(llist);
        System.out.println("加入llist後, ArrayList alist: " + alist);

        int firstIndex = alist.indexOf("apple");
        int lastIndex = alist.lastIndexOf("apple");
        System.out.println("First Index of 'apple': " + firstIndex);
        System.out.println("Last Index of 'apple': " + lastIndex);
    }
}
