using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace AStar
{
    public class NodeMap
    {
        public Node[,] nodes;
        public int xNodeCount, yNodeCount;
        public float xOrigin, yOrigin, zOrigin; //Origin starts at bottom left of map
        public float radius;
        //Clock wise from Top, x, y
        private static int[,] searchOffset4 = new int[4, 2]
        {
            {0, 1},
            {1, 0},
            {0, -1},
            {-1, 0}
        };

        //Clock wise from Top, x, y
        private static int[,] searchOffset8 = new int[8, 2]
        {
            {0, 1},
            {1, 1},
            {1, 0},
            {1, -1},
            {0, -1},
            {-1, -1},
            {-1, 0},
            {-1, 1}
        };


        public NodeMap(float xOrigin, float yOrigin, float zOrigin, int xNodeCount, int yNodeCount, float radius)
        {
            this.xOrigin = xOrigin;
            this.yOrigin = yOrigin;
            this.zOrigin = zOrigin;

            this.xNodeCount = xNodeCount;
            this.yNodeCount = yNodeCount;

            this.radius = radius;

            MapNodeToWorld();
        }

        public NodeMap(float xOrigin, float yOrigin, int xNodeCount, int yNodeCount) : this(xOrigin, yOrigin, 0, xNodeCount, yNodeCount, 0.5f)
        {

        }

        private void MapNodeToWorld()
        {
            this.nodes = new Node[this.yNodeCount, this.xNodeCount];
            for (int y = 0; y < nodes.GetLength(0); y++)
            {
                for (int x = 0; x < nodes.GetLength(1); x++)
                {
                    this.nodes[y, x] = new Node(new Vector2(this.xOrigin + this.radius * x * 2, this.yOrigin + this.radius * y * 2), x, y);
                }
            }
        }


        public void BakeMap(LayerMask walkableMask, float scanRadius)
        {
            for (int y = 0; y < nodes.GetLength(0); y++)
            {
                for (int x = 0; x < nodes.GetLength(1); x++)
                {
                    this.nodes[y, x].blocked = Physics.CheckSphere(new Vector3(this.nodes[y, x].position.x, this.nodes[y, x].position.y, this.zOrigin), scanRadius, walkableMask);
                }
            }
        }

        public void BakeMap(LayerMask walkableMask)
        {
            for (int y = 0; y < nodes.GetLength(0); y++)
            {
                for (int x = 0; x < nodes.GetLength(1); x++)
                {
                    this.nodes[y, x].blocked = Physics.CheckSphere(new Vector3(this.nodes[y, x].position.x, this.nodes[y, x].position.y, this.zOrigin), this.radius, walkableMask);
                }
            }
        }

        public void BakeMap(Tilemap unwalkableMap)
        {
            for (int y = 0; y < this.yNodeCount; y++)
            {
                for (int x = 0; x < this.xNodeCount; x++)
                {
                    this.nodes[y, x].blocked = 
                        unwalkableMap.GetTile(
                            new Vector3Int(
                                (int)this.nodes[y, x].position.x, 
                                (int)this.nodes[y, x].position.y, 
                                (int)this.zOrigin
                                ));
                }
            }
        }

        public bool InMapPosition(float x, float y)
        {
            x = (x - this.xOrigin) / (2f * this.radius);
            y = (y - this.yOrigin) / (2f * this.radius);
            return InMapIndex((int)x, (int)y);
        }

        public bool InMapIndex(int x, int y)
        {
            return (x < this.xNodeCount && x >= 0 && y < this.yNodeCount && y >= 0);
        }

        //################################
        //# Path finder (A*)
        //################################

        public Path FindPath(Vector2 start, Vector2 end, int searchCycles, bool cornerSearch)
        {
            Debug.Log("Path finding..");
            int startX = (int)((start.x - this.xOrigin) / (2f*this.radius));
            int startY = (int)((start.y - this.yOrigin) / (2f * this.radius));
            int endX = (int)((end.x - this.xOrigin) / (2f * this.radius));
            int endY = (int)((end.y - this.yOrigin) / (2f * this.radius));
            if (!InMapIndex(startX, startY) || !InMapIndex(endX, endY) || this.nodes[endY, endX].blocked || this.nodes[startY, startX].blocked) return new Path(this, new Node[0]);
            Node[,] lastNodeMap = new Node[this.yNodeCount, this.xNodeCount];
            List<Node> waypoints = new List<Node>();
            Node currentNode = this.nodes[startY, startX];
            currentNode.sum = StartNodeValue(currentNode, this.nodes[endY, endX].position);
            List<Node> closedNodes = new List<Node>();
            List<Node> optionNodes = new List<Node>(); 
            closedNodes.Add(currentNode);

            for (int i = 0; i < searchCycles; i++)
            {
                int[,] searchOffset = searchOffset4;
                if (cornerSearch)
                    searchOffset = searchOffset8;

                //Update distance clock wise starting from the top
                for (int a = 0; a < searchOffset.GetLength(0); a++)
                {
                    int xCheckIndex = currentNode.xIndex + searchOffset[a, 0];
                    int yCheckIndex = currentNode.yIndex + searchOffset[a, 1];
                    if (InMapIndex(xCheckIndex, yCheckIndex))
                    {
                        Node checkingNode = this.nodes[yCheckIndex, xCheckIndex];
                        if (!checkingNode.blocked && !closedNodes.Contains(checkingNode))
                        {
                            float newValue = NodeValue(checkingNode, currentNode, this.nodes[endY, endX].position);

                            if (lastNodeMap[checkingNode.yIndex, checkingNode.xIndex] != null)
                            {
                                if (checkingNode.sum < newValue)
                                {
                                    continue;
                                }
                            }
                            checkingNode.sum = newValue;
                            lastNodeMap[checkingNode.yIndex, checkingNode.xIndex] = currentNode;
                            checkingNode.distanceToStart = newValue - checkingNode.distanceToEnd;
                            if (!optionNodes.Contains(checkingNode))
                            {
                                optionNodes.Add(checkingNode);
                            }
                        }
                    }
                }

                if (currentNode.xIndex == endX && currentNode.yIndex == endY)
                {
                    Debug.Log("Path found. " + i + " cycles");
                    break;
                }

                //Pick new current node
                currentNode = PickeNode(optionNodes);
                closedNodes.Add(currentNode);
                optionNodes.Remove(currentNode);
            }
            while (lastNodeMap[currentNode.yIndex, currentNode.xIndex] != null)
            {
                waypoints.Add(currentNode);
                currentNode = lastNodeMap[currentNode.yIndex, currentNode.xIndex];
            }
            return new Path(this, waypoints.ToArray());
        }

        private float StartNodeValue(Node node, Vector2 end)
        {
            node.distanceToEnd = Vector2.Distance(node.position, end);
            return node.distanceToEnd + node.cost;
        }

        private float NodeValue(Node node, Node lastNode, Vector2 end)
        {
            node.distanceToEnd = Vector2.Distance(node.position, end);
            return node.distanceToEnd + node.cost + Vector2.Distance(node.position, lastNode.position) + lastNode.distanceToStart;
        }

        private static Node PickeNode(IList<Node> nodes)
        {
            float minSum = 0f;
            Node pickedNode = null;
            foreach (Node node in nodes)
            {
                if (pickedNode == null)
                {
                    pickedNode = node;
                    minSum = pickedNode.sum;
                }
                if (node.sum < minSum)
                {
                    minSum = node.sum;
                    pickedNode = node;
                }
            }
            return pickedNode;
        }





        //################################
        //# Path finder lite
        //################################

        public Path FindPathLite(Vector2 start, Vector2 end, int searchCycles, bool cornerSearch)
        {
            Debug.Log("Path-Lite finding..");
            int startX = (int)((start.x - this.xOrigin) / (2f * this.radius));
            int startY = (int)((start.y - this.yOrigin) / (2f * this.radius));
            int endX = (int)((end.x - this.xOrigin) / (2f * this.radius));
            int endY = (int)((end.y - this.yOrigin) / (2f * this.radius));
            if (!InMapIndex(startX, startY) || !InMapIndex(endX, endY) || this.nodes[endY, endX].blocked || this.nodes[startY, startX].blocked) return new Path(this, new Node[0]);
            Node[,] lastNodeMap = new Node[this.yNodeCount, this.xNodeCount];
            List<Node> waypoints = new List<Node>();
            Node currentNode = this.nodes[startY, startX];
            List<Node> exploredNode = new List<Node>();
            currentNode.sum = NodeValueLite(currentNode, currentNode.position, this.nodes[endY, endX].position);
            List<Node> closedNodes = new List<Node>();
            closedNodes.Add(currentNode);
            for (int i = 0; i < searchCycles; i++)
            {
                int[,] searchOffset = searchOffset4;
                if (cornerSearch)
                    searchOffset = searchOffset8;
                //Update distance clock wise starting from the top
                for (int a = 0; a < searchOffset.GetLength(0); a++)
                {

                    int xCheckIndex = currentNode.xIndex + searchOffset[a, 0];
                    int yCheckIndex = currentNode.yIndex + searchOffset[a, 1];
                    if (InMapIndex(xCheckIndex, yCheckIndex))
                    {
                        Node checkingNode = this.nodes[yCheckIndex, xCheckIndex];
                        if (!checkingNode.blocked && !closedNodes.Contains(checkingNode))
                        {
                            float newValue = NodeValueLite(checkingNode, currentNode.position, this.nodes[endY, endX].position);

                            if (lastNodeMap[checkingNode.yIndex, checkingNode.xIndex] != null)
                            {
                                if (checkingNode.sum < newValue)
                                {
                                    continue;
                                }
                            }
                            checkingNode.sum = newValue;
                            lastNodeMap[checkingNode.yIndex, checkingNode.xIndex] = currentNode;
                            if (!exploredNode.Contains(checkingNode))
                                exploredNode.Add(checkingNode);
                        }
                    }
                }



                //Pick new current node
                currentNode = PickeNodeLite(exploredNode);
                closedNodes.Add(currentNode);
                exploredNode.Remove(currentNode);

                if (currentNode.xIndex == endX && currentNode.yIndex == endY)
                {
                    Debug.Log("Path Found. " + i + " cycles");
                    break;
                }
            }
            while (lastNodeMap[currentNode.yIndex, currentNode.xIndex] != null)
            {
                waypoints.Add(currentNode);
                currentNode = lastNodeMap[currentNode.yIndex, currentNode.xIndex];
            }
            return new Path(this, waypoints.ToArray());
        }

        private float NodeValueLite(Node node, Vector2 start, Vector2 end)
        {
            float distanceToEnd = Vector2.Distance(node.position, end);
            node.distanceToEnd = distanceToEnd;
            return distanceToEnd + Vector2.Distance(node.position, start)
                + node.cost;
        }

        private static Node PickeNodeLite(IList<Node> nodes)
        {
            float minSum = nodes[0].sum;
            Node pickedNode = nodes[0];
            foreach (Node node in nodes)
            {
                float sum = node.sum;
                if (sum < minSum)
                {
                    minSum = sum;
                    pickedNode = node;
                }
            }
            return pickedNode;
        }
    }
}

