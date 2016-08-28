using UnityEngine;
using System.Collections.Generic;

public class Unit {
    public enum unitType {Air, Sea, Land};
    unitType uType;
    public string Name { get; private set; }
    public bool isSelected = false;
    int health;
    public int dmg;
    int movement;
    public List<Node> currentPath;
    public string spriteName { get; private set;}
    public Tile tile;


    //TODO: if we are creating a unit, we know the Tile does not have a unit on it already.
    public static Unit createNewUnit(Unit unitProtoType) {
        Unit newUnit = new Unit ();
        newUnit = unitProtoType;
        return newUnit;
    }

    public static Unit createUnitPrototype ( unitType uTypeIn , string name , int health , int damage , int movement , string spriteName ) {
        Unit newUnit = new Unit ();
        newUnit.uType = uTypeIn;
        newUnit.Name = name;
        newUnit.health = health;
        newUnit.dmg = damage;
        newUnit.movement = movement;
        newUnit.spriteName = spriteName;
        return newUnit;
    }
    public unitType getUnitType() {
        return uType;
    }

    public void moveUnit ( int x , int y ) {
        World.current.generatePathTo ( x , y , this );
        if ( currentPath != null ) {
            //int currNode = 0;
            //while (currNode < currentPath.Count - 1) {
            //    Vector3 start = new Vector3 ( currentPath [ currNode ].x , currentPath [ currNode ].y , -1f );
            //    Vector3 end = new Vector3 ( currentPath [ currNode + 1 ].x, currentPath [ currNode  +  1].y, -1f);


            //    currNode++;
            //}

            foreach (Node n in currentPath) {
                Tile oldTile = tile;
                World.current.NodeToTile ( n ).newUnit ( this );
                World.current.worldController.moveUnitSprite ( this , oldTile, World.current.NodeToTile ( n ) );
                oldTile.deleteUnit ();


            }

        }
    }

    public bool equals (Unit unit) {
        if ( unit.tile == this.tile ) {
            return true;
        }
        else {
            return false;
        }
    }
}
