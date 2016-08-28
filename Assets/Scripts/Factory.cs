using UnityEngine;
using System.Collections;

public class Factory {
    // Factory type
    public enum factoryType {Air, Barracks, Armor, Sea};
    factoryType fType;

    // Factory placement
    Tile tile;

    // True if factory isSelected
     public bool isSelected = false;

    // Cost $ cost of the factory
    public static float airCost = 10f;
    public static float barracksCost = 2f;
    public static float armorCost = 4f;
    public static float seaCost = 5f;

    // Constructor
    public Factory(Tile tileIn, factoryType fTypeIn ) {
        tile = tileIn;
        fType = fTypeIn;

    }

    // Methods
    public void createUnit(Unit unitProtoType) {

        Unit unit = Unit.createNewUnit (unitProtoType);
        for ( int i = 0; i < tile.getNeighbors().Length; i++ ) {
            if ( tile.getNeighbors () [ i ] != null ) {
                if ( unit.getUnitType () != Unit.unitType.Sea ) {
                    if ( tile.getNeighbors () [ i ].hasFactory != true && tile.getNeighbors () [ i ].hasUnit != true && tile.getNeighbors () [ i ].getTileType () != Tile.TileType.Water ) {
                        tile.getNeighbors () [ i ].newUnit ( unit );
                        
                        World.current.worldController.createUnitGO (unit);
                        World.current.unitsInWorld.Add (unit);
                        Debug.Log ( "New unit created at (" + tile.x + "," + tile.y + ")" );
                        break;
                    }
                    else {
                        Debug.Log ( "Tile already has a factory/unit." );
                    }
                }
                else if ( unit.getUnitType () == Unit.unitType.Sea ) {
                    if ( tile.getNeighbors () [ i ].hasFactory != true && tile.getNeighbors () [ i ].hasUnit != true && tile.getNeighbors () [ i ].getTileType () == Tile.TileType.Water ) {
                        tile.getNeighbors () [ i ].newUnit ( unit );
                        World.current.worldController.createUnitGO ( unit );
                        World.current.unitsInWorld.Add ( unit );
                        Debug.Log ( "New unit created at (" + tile.x + "," + tile.y + ")" );
                        break;
                    }
                    else {
                        Debug.Log ( "Tile already has a factory/unit." );
                    }
                }
                else {
                    Debug.Log ( "No sutible place for the unit." );
                }
            }
        }


       
    }

    public bool equals(Factory.factoryType factoryTypeIn) {
        if (factoryTypeIn == fType) {
            return true;
        }
        else {
            return false;
        }
    }

    public factoryType getFactoryType() {
        return fType;
    }
 
}
