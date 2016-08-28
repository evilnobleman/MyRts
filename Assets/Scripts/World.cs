using UnityEngine;
using System.Collections.Generic;

public class World {
    Tile [,] tiles;
    Tile Tile;
    Node [,] graph;
    List<Node> currentPath;
    public List<Unit> unitsInWorld {get; private set;}
    public WorldController worldController;
    public static World current {
        get; private set;
    }
    public int maxWorldSize {
        get; private set;
    }
    public  int Height {
        get; private set;
    }
    public int Width {
        get; private set;
    }

    public bool generationDone = false;


    public World ( WorldController worldControllerIn , int width = 25 , int height = 25 ) { //Default size fo civ 5 map
        Height = height;
        Width = width;
        maxWorldSize = 10000;
        tiles = new Tile [Width, Height];
        worldController = worldControllerIn;
        unitsInWorld = new List<Unit>();
        current = this;
    }


    public Tile getTileAt ( int x , int y ) {
        if ( x > Width || x < 0 || y > Height || y < 0 ) {
            Debug.LogError ( "Tile (" + x + "," + y + ") is out of range." );
            return null;
        }
        else {
            return tiles [ x, y ];
        }
    }
    public bool generateWorld () {

        if ( ( Height * Width ) > maxWorldSize ) {
            Debug.LogError ( "World size is too large. World cannot be created" );
            return false;
        }
        else {
            for ( int x = 0; x < Width; x++ ) {
                for ( int y = 0; y < Height; y++ ) {
                    tiles [ x , y ] = new Tile ( x , y );
                    Tile = getTileAt ( x , y );
                    Tile.setTileType ( worldController.mapGen.tileTypeforWorldgeneration ( x , y ) );
                }
            }
            Debug.Log ( "New world created with " + ( Width * Height ) + " Tiles." );
            graph = new Node [ Width , Height ];
            GeneratePathfindingGraph ();
            generationDone = true;
            return true;
        }
    }

    
    public void GeneratePathfindingGraph () {
        graph = new Node [ Width , Height ];
        for ( int x = 0; x < Width; x++ ) {
            for ( int y = 0; y < Height; y++ ) {
                graph [ x , y ] = new Node ();
                graph [ x , y ].x = x;
                graph [ x , y ].y = y;
            }
        }
        for ( int x = 0; x < Width; x++ ) {
            for ( int y = 0; y < Height; y++ ) {
                // We have a 4-way connected map
                // This also works with 6-way hexes and 8-way tiles and n-way variable areas (like EU4)

                if ( x > 0 )
                    graph [ x , y ].neighbors.Add ( graph [ x - 1 , y ] );
                if ( x < Width - 1 )
                    graph [ x , y ].neighbors.Add ( graph [ x + 1 , y ] );
                if ( y > 0 )
                    graph [ x , y ].neighbors.Add ( graph [ x , y - 1 ] );
                if ( y < Height - 1 )
                    graph [ x , y ].neighbors.Add ( graph [ x , y + 1 ] );
            }
        }
    }

    public bool createFactoryOnWorld ( int x , int y , Factory.factoryType fType ) {
        Tile tile = getTileAt ( x , y );
        if ( tile.createFactory ( fType ) ) {
            worldController.createFactoryGO ( tile );
            return true;
        }
        return false;
    }

    public bool canBuyFactory ( Tile tile , Factory.factoryType fType ) {
        if ( fType == Factory.factoryType.Sea ) {
            if ( worldController.pc.money >= Factory.seaCost ) {
                worldController.pc.subtractMoney ( Mathf.FloorToInt ( Factory.seaCost ) );
                return true;
            }
            return false;
        }
        else if ( fType == Factory.factoryType.Air ) {
            if ( worldController.pc.money >= Factory.airCost ) {
                worldController.pc.subtractMoney ( Mathf.FloorToInt ( Factory.airCost ) );
                return true;
            }
            return false;
        }
        else if ( fType == Factory.factoryType.Armor ) {
            if ( worldController.pc.money >= Factory.armorCost ) {
                worldController.pc.subtractMoney ( Mathf.FloorToInt ( Factory.armorCost ) );
                return true;
            }
            return false;
        }
        else if ( fType == Factory.factoryType.Barracks ) {
            if ( worldController.pc.money >= Factory.barracksCost ) {
                worldController.pc.subtractMoney ( Mathf.FloorToInt ( Factory.barracksCost ) );
                return true;
            }
            return false;
        }
        else {
            return false;
        }
    }

    public float CostToEnterTile (Tile tile) {
        return tile.walkingCost;
    }

    public void generatePathTo ( int x , int y , Unit unit ) {
        currentPath = null;
        unit.currentPath = null;
        Tile oldTile = unit.tile;
        Tile newTile = getTileAt ( x , y );
        

        Dictionary<Node , float> dist = new Dictionary<Node , float> ();
        Dictionary<Node , Node> prev = new Dictionary<Node , Node> ();

        List<Node> unvisited = new List<Node>();

        Node source = TileToNode ( oldTile );
        Node target = TileToNode (newTile);

        dist [ source ] = 0;
        prev [ source ] = null;

        foreach (Node v in graph) {
            if (v != source) {
                dist [ v ] = Mathf.Infinity;
                prev [ v ] = null;
            }
            unvisited.Add ( v );
        }

        while (unvisited.Count > 0) {
            Node u = null;

            foreach (Node possibleU in unvisited) {
                if (u == null || dist[possibleU] < dist[u]) {
                    u = possibleU;
                }
            }

            if (u == target) {
                break;
            }

            unvisited.Remove ( u );
            foreach (Node v in u.neighbors) {
                //float alt = dist [ u ] + u.distanceTo ( v );
                float alt = dist [ u ] + CostToEnterTile ( NodeToTile ( v ) );
                if (alt < dist[v]) {
                    dist [ v ] = alt;
                    prev [ v ] = u;
                }
            }
        }

        if (prev[target] == null ) {
            //No Route from target to source
            return;
        }

        currentPath = new List<Node> ();
        Node curr = target;
        while (curr != null) {
            currentPath.Add ( curr );
            curr = prev [ curr ];
        }
         currentPath.Reverse ();
         unit.currentPath = currentPath;
    }

    public Node TileToNode (Tile tile) {
        return graph [ tile.x , tile.y ];
    }

    public Tile NodeToTile (Node n) {
        return getTileAt (n.x, n.y);
    }
}