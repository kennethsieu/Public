package bearmaps.proj2ab;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;
import java.util.Collections;
import java.util.NoSuchElementException;

/**
 * @author Kenneth Sieu
 * @param <T> Takes in generic Type T
 * Array Heap Min Priority Queue.
 */
public class ArrayHeapMinPQ<T> implements ExtrinsicMinPQ<T> {

    /**
     * A Node that stores an item and its priority.
     */
    private class PQNode {
        /**
         * Stores priority.
         */
        private double priority;
        /**
         * Stores the generic item.
         */
        private T item;

        /**
         *
         * @param i A given item.
         * @param p A given priority value.
         * Creates a node with a given item and priority.
         */
        private PQNode(T i, double p) {
            priority = p;
            item = i;
        }
    }

    /**
     * The ArrayList holding all PQNodes.
     */
    private List<PQNode> queue;
    /**
     * A list that stores item's by its natural ordering. Also stores an
     * item position in the queue.
     */
    private Map<T, Integer> existance;
    /**
     * A PQ node that is never called and sit at the beginning of the
     * list for indexing.
     */
    private PQNode sentinel = new PQNode(null, 0);

    /**
     * Constructor for the ArrayHeapPQ.
     */
    public ArrayHeapMinPQ() {
        queue = new ArrayList<>();
        queue.add(sentinel);
        existance = new TreeMap<>();
    }

    /**
     *
     * @param index The index of the child.
     * @return the index of the parent.
     */
    private int parent(int index) {
        return index / 2;
    }

    /**
     *
     * @param child Takes a child node.
     * Pushes the node upward towards the root until its correct position.
     */
    private void swim(int child) {
        int parent = parent(child);
        if (parent != 0) {
            if (queue.get(child).priority < queue.get(parent).priority) {
                Collections.swap(queue, child, parent);
                existance.replace(queue.get(parent).item, parent);
                existance.replace(queue.get(child).item, child);
                swim(parent);
            }
        }
    }

    /**
     *
     * @param index The index for a Node.
     * @return If this node has any children.
     */
    private boolean hasChildren(int index) {
        int left = index * 2;
        return queue.size() > left;
    }

    /**
     *
     * @param index The index for a Node.
     * @return If the node has a right child.
     */
    private boolean hasRight(int index) {
        int right = index * 2 + 1;
        return queue.size() > right;
    }

    /**
     *
     * @param parent Takes a parent node.
     * pulls the Node toward the leaves until it correct position.
     */
    private void sink(int parent) {
        if (hasChildren(parent)) {
            int left = parent * 2;
            int right = parent * 2  + 1;
            if (hasRight(parent)) {
                if (queue.get(left).priority < queue.get(right).priority) {
                    if (queue.get(left).priority < queue.get(parent).priority) {
                        Collections.swap(queue, left, parent);
                        existance.replace(queue.get(parent).item,
                                parent);
                        existance.replace(queue.get(left).item,
                                left);
                        sink(left);
                    }
                } else {
                    if (queue.get(right).priority
                            < queue.get(parent).priority) {
                        Collections.swap(queue, right, parent);
                        existance.replace(queue.get(parent).item,
                                parent);
                        existance.replace(queue.get(right).item,
                                right);
                        sink(right);
                    }
                }
            }
            if (queue.get(left).priority < queue.get(parent).priority) {
                Collections.swap(queue, left, parent);
                existance.replace(queue.get(parent).item, parent);
                existance.replace(queue.get(left).item, left);
                sink(left);
            }
        }
    }

    /**
     *
     * @param item An item.
     * @param priority A priority status.
     * Adds the item with its given priority to the queue.
     */
    @Override
    public void add(T item, double priority) {
        if (contains(item)) {
            throw new IllegalArgumentException();
        }
        PQNode insert = new PQNode((T) item, priority);
        queue.add(insert);
        existance.put(item, queue.size() - 1);
        swim(queue.size() - 1);
    }

    /**
     *
     * @param item A item.
     * @return Whether the queue contains the given item.
     */
    @Override
    public boolean contains(T item) {
        return existance.containsKey(item);
    }


    /**
     *
     * @return The item with the lowest priority.
     * NoSuchElementException if the PQ is empty.
     */
    @Override
    public T getSmallest() {
        if (size() == 0) {
            throw new NoSuchElementException();
        }
        return queue.get(1).item;
    }
    /**
     *
     * @return The item with the lowest priority and removes it from the list.
     * NoSuchElementException if the PQ is empty.
     */
    @Override
    public T removeSmallest() {
        if (size() == 0) {
            throw new NoSuchElementException();
        }
        if (size() == 1) {
            return queue.remove(1).item;
        }
        Collections.swap(queue, 1, queue.size() - 1);
        existance.replace(queue.get(1).item, 1);
        T removal = queue.remove(queue.size() - 1).item;
        existance.remove(removal);
        sink(1);
        return removal;
    }

    /**
     *
     * @return The number for items in the queue.
     */
    @Override
    public int size() {
        return queue.size() - 1;
    }

    /**
     *
     * @param item A item in the list.
     * @param priority A new priority.
     * Changes the priority of the an item in the list, throws a
     *                 NoSuchElementException if it doesn't exist.
     */
    @Override
    public void changePriority(T item, double priority) {
        if (!contains(item)) {
            throw new NoSuchElementException();
        }
        int currNode = existance.get(item);
        queue.get(currNode).priority = priority;
        swim(currNode);
        sink(currNode);

    }
}
