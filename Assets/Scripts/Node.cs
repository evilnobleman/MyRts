using UnityEngine;
using System.Collections.Generic;

public class Node {
    public List<Node> neighbors;

    public int x;
    public int y;

    public Node () {
        neighbors = new List<Node> ();
        //Debug.Log ("Neighbors Created");
    }

    public float distanceTo (Node n) {
        return Vector2.Distance (
                new Vector2(x,y),
                new Vector2(n.x,n.y)
            );
    }
}
