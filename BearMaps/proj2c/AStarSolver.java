package bearmaps.proj2c;
import bearmaps.proj2ab.ArrayHeapMinPQ;
import bearmaps.proj2ab.ExtrinsicMinPQ;
import edu.princeton.cs.algs4.Stopwatch;

import java.util.List;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;
import java.util.LinkedList;
import java.util.HashSet;

/**
 * @author Kenneth Sieu 11//2020
 * @param <Vertex> Uses object Vertex.
 * Class for A* Solver program.
 */
public class AStarSolver<Vertex> implements ShortestPathsSolver<Vertex> {

    /**
     * Stores dequeued vertices.
     */
    private Set<Vertex> dequeued;
    /**
     * Stores the weighted distance from the starting point.
     */
    private Map<Vertex, Double> distTo;
    /**
     * Stores the vertex that put explored it first.
     */
    private Map<Vertex, Vertex> edgeTo;
    /**
     * Stores the points to be check based on priority.
     */
    private ExtrinsicMinPQ<Vertex> fringe;
    /**
     * Stores the outcome of the A* Solver.
     */
    private SolverOutcome outcome;
    /**
     * Stores the solution to the A* Solver.
     */
    private LinkedList<Vertex> solution;
    /**
     * The overall distance to the solution.
     */
    private double solutionWeight;
    /**
     * Stores the number of vertices conversed.
     */
    private int states;
    /**
     * Stores the time spent conversing the graph.
     */
    private double timeSpent;
    /**
     * Stores which vertices have been visited.
     */

    /**
     *
     * @param input Takes a Graph.
     * @param start Takes a starting vertex.
     * @param end Takes a ending vertex.
     * @param timeout Takes a time limit.
     * Constructs the A* Solver and solve the given problem.
     */
    public AStarSolver(AStarGraph<Vertex> input,
                       Vertex start, Vertex end, double timeout) {
        Stopwatch sw = new Stopwatch();
        dequeued = new HashSet<>();
        distTo = new HashMap<>();
        edgeTo = new HashMap<>();
        distTo.put(start, new Double(0));
        fringe = new ArrayHeapMinPQ<>();
        fringe.add(start, input.estimatedDistanceToGoal(start, end));
        while ((fringe.size() != 0)
                && (!fringe.getSmallest().equals(end))
                && (timeSpent < timeout)) {
            Vertex p = fringe.removeSmallest();
            List<WeightedEdge<Vertex>> neighborEdges = input.neighbors(p);
            for (WeightedEdge<Vertex> e : neighborEdges) {
                if (!dequeued.contains(e.to())) {
                    if (!distTo.containsKey(e.to())) {
                        distTo.put(e.to(), Double.POSITIVE_INFINITY);
                    }
                    relax(e, input, end);
                }
            }
            dequeued.add(p);
            states++;
            timeSpent = sw.elapsedTime();
        }

        if (timeSpent >= timeout) {
            outcome = SolverOutcome.TIMEOUT;
        } else if (fringe.size() == 0) {
            outcome = SolverOutcome.UNSOLVABLE;
        } else {
            outcome = SolverOutcome.SOLVED;
        }

        if (outcome.equals(SolverOutcome.TIMEOUT)
                || outcome.equals(SolverOutcome.UNSOLVABLE)) {
            solution = new LinkedList<>();
            solutionWeight = 0;
        } else {
            solutionWeight = distTo.get(end);
            Vertex curr = end;
            solution = new LinkedList<>();
            while (!curr.equals(start)) {
                solution.addFirst(curr);
                curr = edgeTo.get(curr);
            }
            solution.addFirst(curr);
        }

    }

    /**
     *
     * @param e Takes an edge.
     * @param graph Takes a graph.
     * @param goal Takes a goal vertex.
     * Relaxes a given vertex and adds it to the PQ if it qualifies.
     */
    private void relax(WeightedEdge e, AStarGraph<Vertex> graph, Vertex goal) {
        Vertex p = (Vertex) e.from();
        Vertex q = (Vertex) e.to();
        double w = e.weight();
        if (distTo.get(p) + w < distTo.get(q)) {
            distTo.put(q, distTo.get(p) + w);
            edgeTo.put(q, p);
            if (fringe.contains(q)) {
                fringe.changePriority(q, distTo.get(q)
                        + graph.estimatedDistanceToGoal(q, goal));
            } else {
                fringe.add(q, distTo.get(q)
                        + graph.estimatedDistanceToGoal(q, goal));

            }
        }
    }


    @Override
    /**
     * Returns the outcome of the A* Solver.
     */
    public SolverOutcome outcome() {
        return outcome;
    }

    /**
     *
     * @return A list of Vertices corresponding to the solution.
     */
    @Override
    public List<Vertex> solution() {
        return solution;
    }

    /**
     *
     * @return The weight of the shortest path.
     */
    @Override
    public double solutionWeight() {
        return solutionWeight;
    }

    /**
     *
     * @return The number of vertices explored.
     */
    @Override
    public int numStatesExplored() {
        return states;
    }

    /**
     *
     * @return The time required to run A* Solver.
     */
    @Override
    public double explorationTime() {
        return timeSpent;
    }
}
