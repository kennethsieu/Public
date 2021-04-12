package bearmaps.proj2d;

import bearmaps.proj2ab.Point;
import bearmaps.proj2ab.WeirdPointSet;
import bearmaps.proj2c.streetmap.StreetMapGraph;
import bearmaps.proj2c.streetmap.Node;
/**
 * @source imported from the princeton library.
 */
import edu.princeton.cs.algs4.TrieSET;

import java.util.*;

import static java.lang.Integer.parseInt;

/**
 * An augmented graph that is more powerful that a standard StreetMapGraph.
 * Specifically, it supports the following additional operations:
 *
 *
 * @author Alan Yao, Josh Hug, ________
 */
public class AugmentedStreetMapGraph extends StreetMapGraph {
    /**
     * Stores a KD Tree of all the vertices with neighbors.
     */
    private WeirdPointSet pointMap;
    /**
     * Stores a Point to ID map.
     */
    private Map<Point, Long> pointToID;

    /**
     * Stores a list of places.
     */
    private bearmaps.proj2d.Trie places;

    /**
     * A map from names to ids.
     */
    private Map<String, Long> ids;

    private Map<String, List<String>> cleantoOri;
    /**
     *
     * @param dbPath Something.
     *  Constructor for AugmentedStreetMapGraph.
     */
    public AugmentedStreetMapGraph(String dbPath) {
        super(dbPath);
        List<Node> nodes = this.getNodes();
        pointToID = new HashMap<>();
        List<Point> points = new ArrayList<>();
        places = new Trie();
        ids = new HashMap();
        cleantoOri = new HashMap<>();
        for (Node node: nodes) {
            String name = name(node.id());
            if (name != null) {
                String clean = cleanString(name).replace(" ", "");
                ids.put(name, node.id());

                if (clean.length() != 0) {
                    String dn = name.substring(0,1);
                    String dcn = name.substring(0,1);
                    if (!dn.equals(dcn)) {
                        if (cleantoOri.containsKey(" ")) {
                            cleantoOri.get(" ").add(name);
                        } else {
                            List<String> temp = new LinkedList<>();
                            temp.add(name);
                            cleantoOri.put(" ", temp);
                        }
                    } else {
                        places.add(clean);
                        if (cleantoOri.containsKey(clean)) {
                            cleantoOri.get(clean).add(name);
                        } else {
                            List<String> temp = new LinkedList<>();
                            temp.add(name);
                            cleantoOri.put(clean, temp);
                        }
                    }
                } else if (name.length() != 0) {
                    if (cleantoOri.containsKey(" ")) {
                        cleantoOri.get(" ").add(name);
                    } else {
                        List<String> temp = new LinkedList<>();
                        temp.add(name);
                        cleantoOri.put(" ", temp);
                    }
                }
            }

            if (neighbors(node.id()).size() != 0) {
                Point x = new Point(node.lon(), node.lat());
                pointToID.put(x, node.id());
                points.add(x);
            }
        }
        pointMap = new WeirdPointSet(points);
    }




    /**
     * For Project Part II
     * Returns the vertex closest to the given longitude and latitude.
     * @param lon The target longitude.
     * @param lat The target latitude.
     * @return The id of the node in the graph closest to the target.
     */
    public long closest(double lon, double lat) {
        Point x = pointMap.nearest(lon, lat);
        return pointToID.get(x);
    }


    /**
     * For Project Part III (gold points)
     * In linear time, collect all the names of OSM locations that prefix-match the query string.
     * @param prefix Prefix string to be searched for. Could be any case, with our without
     *               punctuation.
     * @return A <code>List</code> of the full names of locations whose cleaned name matches the
     * cleaned <code>prefix</code>.
     */
    public List<String> getLocationsByPrefix(String prefix) {
        if(prefix.equals(" ")) {
            return cleantoOri.get(" ");
        } else {
            List<String> results = new LinkedList<>();
            for(String clean: places.keysWithPrefix(prefix)) {
                for(String place: cleantoOri.get(clean)) {
                    results.add(place);
                }
            }
            return results;
        }
    }

    /**
     * For Project Part III (gold points)
     * Collect all locations that match a cleaned <code>locationName</code>, and return
     * information about each node that matches.
     * @param locationName A full name of a location searched for.
     * @return A list of locations whose cleaned name matches the
     * cleaned <code>locationName</code>, and each location is a map of parameters for the Json
     * response as specified: <br>
     * "lat" -> Number, The latitude of the node. <br>
     * "lon" -> Number, The longitude of the node. <br>
     * "name" -> String, The actual name of the node. <br>
     * "id" -> Number, The id of the node. <br>
     */
    public List<Map<String, Object>> getLocations(String locationName) {
        List<Map<String, Object>> results = new LinkedList<>();
        String clean = locationName.replace(" ","");

        if (cleantoOri.containsKey(clean)) {
            List<String> process = cleantoOri.get(clean);
            for (String place : process) {
                Map<String, Object> x = new HashMap<>();
                Long id = ids.get(place);
                x.put("lat", lat(id));
                x.put("lon", lon(id));
                x.put("name", place);
                x.put("id", id);
                results.add(x);
            }
        }
        return results;
    }


    /**
     * Useful for Part III. Do not modify.
     * Helper to process strings into their "cleaned" form, ignoring punctuation and capitalization.
     * @param s Input string.
     * @return Cleaned string.
     */
    public static String cleanString(String s) {
        return s.replaceAll("[^a-zA-Z ]", "").toLowerCase();
    }

}
