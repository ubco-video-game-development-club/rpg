using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;
using UnityEngine.Tilemaps;

public class AStarExampleTilemap : MonoBehaviour
{
    public NodeMap nodeMap = new NodeMap(0.5f, 0.5f, 0, 50, 50, 0.2f); 
    /*
     * For Astar to work there needs to be a map for it to traverse
     * 
     * the parameters (in order): Origin x, Origin y, Origin z, # of x nodes, # of y nodes, radius of each nodes
     * 
     * Origin start at bottom left
     */


    // Path finder's target position to reach
    public Transform endPosition;

    // Tile map for untraversable tiles
    public Tilemap wallTileMap;

    // Path finder returns a Path object which contains an array of Node objects representing the waypoints the path finder took to get to target position
    Path pathToEnd = new Path(new NodeMap(0.5f, 0.5f, 0, 50, 50, 0.2f), new Node[0]);

    // Variables for a cool down system to constantly update path
    float timeMemory = 3f;
    public float pathUpdateTimer = 3f;


    // How many cycles to loop through before giving up on looking for a path to target position
    public int searchCycles = 2000;

    private void Start()
    {
        // To "Bake" the node map to capture untraversable areas. Whill scan the each node on wallTileMap for untraversable tiles on nodeMap
        nodeMap.BakeBlockedMap(wallTileMap);

        // Baking traversable but not desirable areas with 1.5 cost
        nodeMap.BakeCostMap(LayerMask.GetMask("Water"), 1.5f);

        /* 
         * For 3D map baking
         * 
         * Overloaded method, takes integer repersenting LayerMask id for *IGNORED* physics layer
         * 
         * Scans each node with Physics.CheckSphere() for colliders around node, any collider detected not in above mentioned LayerMask will be consider an obstacle.
         * Therefore, marking that node as untraversable.
         */
        //nodeMap.BakeBlockedMap(LayerMask.GetMask("Ignore Raycast"));
    }

    private void Update()
    {
        timeMemory += Time.deltaTime;
        if (timeMemory >= pathUpdateTimer)
        {
            timeMemory = 0f;



            // Find path from position (2, 2) to endPosition, the boolean is to specify whether or not diagonal pathing is legal
            pathToEnd = nodeMap.FindPath(new Vector2(2, 2), endPosition.position, searchCycles, true);





            // Path Finder Lite: A* can be an expensive algorithm and an overkill for simple terrain, a faster, cheaper but less optimal path finder was also implimented.
            // pathToEnd = nodeMap.FindPathLite(new Vector2(2, 2), endPosition.position, searchCycles, true);
        }
    }


    // visuals, white for basic nodes, green for pathing nodes, goes from light green to dark green representing the progression of the path.
    void OnDrawGizmos()
    {
        foreach (Node n in nodeMap.nodes)
        {
            if (n.blocked)
            {
                Gizmos.color = new Color(1, 0, 0, 1f);
            }
            else
            {
                Gizmos.color = new Color(1, 1, 1-n.cost, 1f);
            }
            Gizmos.DrawCube(n.position, new Vector3(0.2f, 0.2f, 0.2f));
        }

        for (int n = 0; n < pathToEnd.waypoints.Length; n++)
        {
            Gizmos.color = new Color(0, (1f / pathToEnd.waypoints.Length) * n, 0, 1f);
            Gizmos.DrawCube(pathToEnd.waypoints[n].position, new Vector3(0.2f, 0.2f, 0.2f));
        }

    }
}
