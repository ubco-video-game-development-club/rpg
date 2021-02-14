# A* (AStar) Path finder

## About

Simple A* path finder API for Unity. Supports both 3d collider based terrain and tilemap based terrain. 

**This API currently only support flat terrain! 3D walls are fine as long as you expect the path finder to move around it rather than to climb it! Will fail with hills or flying!**

Currently still in development phase, although it is usable, key features are missing. Key features missing are map saving and multi-threading optimization.

## How to use?

### Step 1: Bake the map

For A* to work a baked NodeMap object is requird. "Baking" is the process by which the map determins traversable and non-traversable locations.

*Note: Each location on the NodeMap is represented by a Node object, AStar will use these nodes as points it can or cannot walk on. You can increase the resolution of the map by increasing the Node count and decreasing Node radius*

**Example Code for NodeMap baking:**
```C#
/*
* the parameters (in order): Origin x, Origin y, Origin z, # of x nodes, # of y nodes, radius of each nodes
* 
* Origin start at bottom left
*/
NodeMap nodeMap = new NodeMap(0.5f, 0.5f, 0, 50, 50, 0.2f);

// Tile map for untraversable tiles
public Tilemap wallTileMap;

// Tile map for traversable but undesirable tiles
public Tilemap waterTileMap;


void Start()
{
    // To "Bake" the node map to capture untraversable areas.
    nodeMap.BakeBlockedMap(wallTileMap);

    // Bake traversable but not desirable areas with 1.5 cost for tilemap based terrain
    nodeMap.BakeCostMap(waterTileMap, 1.5f);

    /* 
    * For collider based map baking
    * 
    * Overloaded method, takes integer repersenting LayerMask (id + 1) for *IGNORED* physics layer
    */
    nodeMap.BakeBlockedMap(LayerMask.GetMask("Ignore Raycast"));

    // Bake traversable but not desirable areas with 1.5 cost for collider based terrain
    nodeMap.BakeCostMap(LayerMask.GetMask("Water"), 1.5f);
}
```

### Step 2: Find your path with AStar

Now that the map is baked it is ready for the AStar path finder to traverse it. To do this, you need to give the path finder a start location, an end location, how many search cycles and whether or not corner steps are legal.
The path finder will return a ```Path``` object that has an array of Nodes representing waypoints of the path to take to get to the target location.

Example Code:
```C#
public Transform endPosition;
int searchCycles = 2000;

// Extra code removed for visibility, usually looped in 3-5s intervals for moving targets
void Update()
{
    Path pathToEnd = nodeMap.FindPath(new Vector2(2, 2), endPosition.position, searchCycles, true);
}
```

### Bonus Step: Save / Load Premade Map

Most of the time it is a waste of resources to rebake the entire map, especially when the map is large. Therefore, it could be wise to save the map and load the already baked map for a certain terrain. Simply follow these examples to achieve map loading/saving in binary format.

**Note: SHOULD ONLY BE USED FOR TESTING. 
Because the Save() and Load() path points to Unity's project file which will not exist in exported game!**

Example Code:

```C#
// Create map
NodeMap nodeMap = new NodeMap(0.5f, 0.5f, 0, 50, 50, 0.2f);

// Bake map
nodeMap.BakeCostMap(waterTileMap, 1.5f);

// Save baked map as "MyNodeMap.bin" at ../UnityProjectFile/Assets/MyNodeMap.bin
nodeMap.Save("MyNodeMap");

// Load baked map from ../UnityProjectFile/Assets/MyNodeMap.bin
nodeMap = NodeMap.Load("MyNodeMap");
```


## Extras

One of the extra features is the ```nodeMap.FindPathLite()``` function, this function is great for when the terrain is very simple and therefore don't need the reletively more expensive AStar path finding algorithm. ```FindPathLite()``` will find your path in less cycles and less calculation but the path will not be the most optimal that AStar can almost always find.

Example Code:
```C#
public Transform endPosition;
int searchCycles = 2000;

// Extra code removed for visibility, usually looped in 3-5s intervals for moving targets
void Update()
{
    Path pathToEnd = nodeMap.FindPathLite(new Vector2(2, 2), endPosition.position, searchCycles, true);
}
```
