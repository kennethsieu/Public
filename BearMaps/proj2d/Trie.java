package proj2d;

import java.util.Map;
import java.util.HashMap;
import java.util.List;
import java.util.LinkedList;

import static bearmaps.proj2d.AugmentedStreetMapGraph.cleanString;

/**
 * @author Kenneth Sieu
 * Trie implementation.
 */
public class Trie {

    /**
     * Node for holding information.
     */
    private class TNode {

        /**
         * Holds the child link to this node.
         */
        private Map<Character, TNode> links;

        /**
         * determines if this is the last letter of a word.
         */
        private boolean key;

        /**
         * Stores words based on character.
         */
        private Map<Character, List<String>> words;

        /**
         * Constructor for TNode.
         */
        private TNode() {
            links = new HashMap();
            key = false;
            words = new HashMap<>();
        }
    }

    /**
     * The root of the Trie.
     */
    private TNode root;

    /**
     * Constructor for the Trie.
     */
    public Trie() {
        root = new TNode();
    }

    /**
     *
     * @param word Takes a word.
     * Adds the word to the trie.
     */
    public void add(String word) {
        if (word.equals("")) {
            if (!root.words.containsKey("?")) {
                List<String> temp = new LinkedList<>();
                temp.add("");
                root.words.put('?', temp);
            }
        } else {
            addString(word, root);
        }
    }

    /**
     *
     * @param word A word.
     * @param curr The current Node.
     * Recursively add each letter of a given word.
     */
    private void addString(String word, TNode curr) {
        Character first = word.charAt(0);
        String rest = word.substring(1);
        if (rest.equals("")) {
            curr.key = true;
        } else {
            if (curr.links.containsKey(first)) {
                TNode next = curr.links.get(first);
                curr.words.get(first).add(rest);
                addString(rest, next);
            } else {
                curr.links.put(first, new TNode());
                curr.words.put(first, new LinkedList<>());
                curr.words.get(first).add(rest);
                TNode next = curr.links.get(first);
                addString(rest, next);
            }
        }
    }

    /**
     *
     * @param word Takes a word.
     * @return If the given string is in the trie.
     */
    public boolean containString(String word) {
        return containString(word, root);
    }

    /**
     *
     * @param word A word.
     * @param curr The current Node.
     * @return Whether the work exist in the trie.
     */
    private boolean containString(String word, TNode curr) {
        Character first = word.charAt(0);
        String rest = word.substring(1);
        if (rest.equals("")) {
            return curr.key;
        } else {
            if (curr.links.containsKey(first)) {
                TNode next = curr.links.get(first);
                return containString(rest, next);
            } else {
                return false;
            }
        }
    }

    /**
     *
     * @param prefix Takes a prefix.
     * @return A list of all words that share the cleaned prefix.
     */
    public List<String> keysWithPrefix(String prefix) {
        List<String> temp = new LinkedList<>();
        if (prefix.equals("")) {
            if(root.words.containsKey('?')) {
                temp.add("");
            }
            return temp;
        } else {
            return prefixSearch(prefix, prefix, root);
        }
    }

    /**
     *
     * @param prefix The current prefix.
     * @param prefix2 The prefix to be added.
     * @param curr The current node.
     * @return A list of cleaned words with the prefix.
     */
    private List<String> prefixSearch(String prefix,
                                      String prefix2, TNode curr) {
        Character first = prefix.charAt(0);
        String rest = prefix.substring(1);
        if (rest.equals("") && curr.links.containsKey(first)) {
            List<String> result = new LinkedList<>();
            for (String word: curr.words.get(first)) {
                result.add(prefix2 + word);
            }
            return result;
        } else {
            if (curr.links.containsKey(first)) {
                TNode next = curr.links.get(first);
                return prefixSearch(rest, prefix2, next);
            } else {
                return new LinkedList<>();
            }
        }
    }

}
