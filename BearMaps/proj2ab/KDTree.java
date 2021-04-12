package bearmaps.proj2ab;
import bearmaps.proj2ab.Point;
import bearmaps.proj2ab.PointSet;

import java.util.List;
import static bearmaps.proj2ab.Point.distance;

/**
 * @author Kenneth Sieu
 * KDTree implimentation for searching.
 */
public class KDTree implements PointSet {

    /**
     * A node that stores information of a point.
     */
    private class KDNode {
        /**
         * Stores the point of the node.
         */
        private Point point;
        /**
         * Determines to compare along x (true) or along y (false).
         */
        private boolean compdir;
        /**
         * The child node to the left of this node.
         */
        private KDNode left;
        /**
         * The child node to the right of this node.
         */
        private KDNode right;

        /**
         *
         * @param p A point.
         * @param c Compare along x or y.
         * @param l The node on the left.
         * @param r The node on the right.
         */
        private KDNode(Point p, boolean c, KDNode l, KDNode r) {
            point = p;
            compdir = c;
            left = l;
            right = r;
        }
    }

    /**
     * The Root of the tree.
     */
    private KDNode root;

    /**
     *
     * @param points Takes a list of points.
     * Constructs a KDTree based on the value of the points given.
     */
    public KDTree(List<Point> points) {
        root = new KDNode(points.get(0), true, null, null);
        for (int i = 1; i < points.size(); i++) {
            insert(points.get(i), root);
        }
    }

    /**
     *
     * @param xy Takes a point.
     * @param curr Takes the current Node.
     * Recursively navigates along the KDTree until it reaches a leaf.
     * Places it on the left or right.
     */
    private void insert(Point xy, KDNode curr) {
        if (curr.point.getX() == xy.getX() && curr.point.getY() == xy.getY()) {
            curr.point = xy;
        } else if (curr.compdir) {
            if (curr.point.getX() <= xy.getX()) {
                if (curr.right == null) {
                    curr.right = new KDNode(xy, false, null, null);
                } else {
                    insert(xy, curr.right);
                }
            } else {
                if (curr.left == null) {
                    curr.left = new KDNode(xy, false, null, null);
                } else {
                    insert(xy, curr.left);
                }
            }
        } else {
            if (curr.point.getY() <= xy.getY()) {
                if (curr.right == null) {
                    curr.right = new KDNode(xy, true, null, null);
                } else {
                    insert(xy, curr.right);
                }
            } else {
                if (curr.left == null) {
                    curr.left = new KDNode(xy, true, null, null);
                } else {
                    insert(xy, curr.left);
                }
            }
        }
    }

    /**
     *
     * @param x Takes a x value.
     * @param y Takes a y value.
     * @return The point nearest to the point with the given values.
     */
    @Override
    public Point nearest(double x, double y) {
        Point goal = new Point(x, y);
        return nearestHelper(root, goal, root).point;
    }

    /**
     *
     * @param curr The current Node being transversed on.
     * @param goal The goal point.
     * @param best The current best Node with the shortest difference.
     * @return The Node with the nearest point.
     */
    private KDNode nearestHelper(KDNode curr, Point goal, KDNode best) {
        if (curr == null) {
            return best;
        }
        if (distance(curr.point, goal) < distance(best.point, goal)) {
            best = curr;
        }
        KDNode goodSide;
        KDNode badSide;
        if (curr.compdir) {
            if (goal.getX() >= curr.point.getX()) {
                goodSide = curr.right;
                badSide = curr.left;
            } else {
                goodSide = curr.left;
                badSide = curr.right;
            }
        } else {
            if (goal.getY() >= curr.point.getY()) {
                goodSide = curr.right;
                badSide = curr.left;
            } else {
                goodSide = curr.left;
                badSide = curr.right;
            }
        }
        best = nearestHelper(goodSide, goal, best);
        if (curr.compdir) {
            double transpose = Math.abs(goal.getX() - curr.point.getX());
            if (Math.pow(transpose, 2) < distance(best.point, goal)) {
                best = nearestHelper(badSide, goal, best);
            }
        } else {
            double transpose = Math.abs(goal.getY() - curr.point.getY());
            if (Math.pow(transpose, 2) < distance(best.point, goal)) {
                best = nearestHelper(badSide, goal, best);
            }
        }
        return best;
    }
}
