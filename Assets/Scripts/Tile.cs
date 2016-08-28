using UnityEngine;
using System.Collections;

public class Tile {
    /// <summary>
    /// 1. The edge neighbors dont know what to do and the freek out. Fix the code that does that. 
    /// 2. Keep looking on how to generate a random map.
    /// 3. if random map gen cant be done right now, rewrite map code to make a pre made map.
    /// 4. Starting on unit movement would be cool. A* mabye. try to find some others.
    /// 
    ///  OVERALL GOOD WORK
    /// </summary>

    // Tyoes that Tiles can be.
    public enum TileType { Grass, Water, Coast };
    TileType tType;
    // The higher the number the less likely the unit will take this tile
    public float walkingCost {get; private set;}

    // Tile Placement
    public int x { get; private set; }
    public int y { get; private set; }
    
    public bool hasFactory = false;
    public bool hasUnit = false;

    // Tiles can hold one unit and one factory
    public Unit unit { get; private set; }
    public Factory factory { get; private set; }

    // Construstor
    public Tile( int X , int Y ) {
        this.x = X;
        this.y = Y;
        walkingCost = 1;
    }

    // Methods
    public TileType getTileType() {
        return tType;
    }

    public bool setTileType(TileType newType) {
        if (tType == newType) {
            return false;
        }
        else {
            tType = newType;
            return true;
        }
    }
    public bool createFactory(Factory.factoryType fType) { // FIXME: Make some "if"'s into private methods.

        // TODO: Only certain units should be able to build factories. RIGHT NOW ALL CAN.
            if ( hasFactory == false && hasUnit == false ) {
                if ( fType == Factory.factoryType.Sea ) {
                    if ( tType == TileType.Coast ) {
                        if ( World.current.canBuyFactory ( this , fType ) ) {
                            factory = new Factory ( this , fType );
                            Debug.Log ( "Factory Created.\ntype: " + fType + "  ||  Location: (" + x + "," + y + ")" );
                            walkingCost = 100;
                            hasFactory = true;
                            return true;
                    }
                        else {
                            Debug.Log ( "Can not create factory.  1" );
                            return false;
                        }
                    }
                    Debug.Log ( "Can only create sea factory on coast." );
                    return false;
                }
                else if ( !( fType == Factory.factoryType.Sea ) ) {
                    if ( !( tType == Tile.TileType.Water ) ) {
                        if ( World.current.canBuyFactory ( this , fType ) ) {
                            factory = new Factory ( this , fType );
                            Debug.Log ( "Factory Created.\ntype: " + fType + "  ||  Location: (" + x + "," + y + ")" );
                            walkingCost = 100;
                            hasFactory = true;
                            return true;
                        }
                    return false;
                }
                    else {
                        Debug.Log ( "Can not create factory.  2" );
                        return false;
                    }
                }
                else {
                    Debug.Log ( "Can not create factory. 3" );
                    return false;
                }
            }
            else {
                Debug.Log ( "Can not create factory.  4" );
                return false;
            }
    }
    public void deleteFactory() { // should be private made public for testing.
        factory = null;
        hasFactory = false;
        walkingCost -= 100;
        Debug.Log("Factory Deleted at location (" + x + "," + y + ")");
    }

    public void newUnit(Unit newUnit) { // IDK WHAT THIS DOES..... EDIT: lets Tile know thats a new unit has been moved here
        unit = newUnit;
        unit.tile = this;
        hasUnit = true;
        walkingCost++;
    }

    public bool equals(Tile.TileType TileTypeIn) {
        if (TileTypeIn == tType) {
            return true;
        }
        else {
            return false;
        }
    }

    public void deleteUnit () {
        walkingCost--;
        unit = null;
    }

    public Tile[] getNeighbors() {
        Tile[] neighbors = new Tile[4];
        // North Neighbor
        if (!(this.y >= World.current.Height - 1)) {
            neighbors [ 0 ] = World.current.getTileAt ( x , y + 1 );
        }
        // East Neighbor
        if (!(this.x >= World.current.Width - 1)) {
            neighbors [ 1 ] = World.current.getTileAt ( x + 1 , y );
        }
        // South Neighbor
        if (!(this.y <= 0)) {
            neighbors [ 2 ] = World.current.getTileAt ( x , y - 1 );
        }
        // West Neighbor
        if (!(this.x <= 0)) {
            neighbors [ 3 ] = World.current.getTileAt ( x - 1 , y );
        }
        return neighbors;
    }


}