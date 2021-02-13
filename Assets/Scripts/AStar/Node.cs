using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AStar
{
    [Serializable]
    public class Node 
    { 
        public float cost;
        readonly public float x, y;
        readonly public int xIndex, yIndex;
        public bool blocked;
        public float sum, distanceToEnd, distanceToStart = 0f;
        public Node(float x, float y, float cost, bool blocked, int xIndex, int yIndex)
        {
            this.x = x;
            this.y = y;
            this.cost = cost;
            this.blocked = blocked;
            this.xIndex = xIndex;
            this.yIndex = yIndex;
        }

        public Node(float x, float y, int xIndex, int yIndex) : this(x, y, 0f, false, xIndex, yIndex) 
        {
            
        }
    }
}

