using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AStar
{
    public class Node 
    { 
        public float cost;
        readonly public Vector2 position;
        readonly public int xIndex, yIndex;
        public bool blocked;
        public float sum, distanceToEnd, distanceToStart = 0f;
        public Node(Vector2 position, float cost, bool blocked, int xIndex, int yIndex)
        {
            this.position = position;
            this.cost = cost;
            this.blocked = blocked;
            this.xIndex = xIndex;
            this.yIndex = yIndex;
        }

        public Node(Vector2 position, int xIndex, int yIndex) : this(position, 0f, false, xIndex, yIndex) 
        {
            
        }
    }
}

