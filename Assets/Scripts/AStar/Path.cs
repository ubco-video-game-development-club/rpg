using UnityEngine;
using System.Collections;

namespace AStar
{
    public class Path
    {
        public NodeMap map;
        public Node[] waypoints;

        public Path(NodeMap map, Node[] waypoints)
        {
            this.waypoints = waypoints;
            this.map = map;
        }
    }
}

