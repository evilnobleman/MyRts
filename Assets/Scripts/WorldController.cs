using UnityEngine;
using System.Collections.Generic;
using System;
public class WorldController : MonoBehaviour {
    World world;
    public PlayerController pc { get; private set; }
    Sprite [] tileSprites;
    Sprite [] unitSprites;
    Sprite [] factorySprites;
    public List<GameObject> unitsInWorldController {get; private set;}
    public RandomMapGenerarionPerlinNoise mapGen {get; private set;}
    // Use this for initialization
    void Start () {
        world = new World ( this );
        mapGen = new RandomMapGenerarionPerlinNoise ();
        pc = GameObject.FindObjectOfType<PlayerController> ();
        unitsInWorldController = new List<GameObject> ();
        tileSprites = Resources.LoadAll<Sprite> ( "Sprites/TileSprites" );
        unitSprites = Resources.LoadAll<Sprite> ( "Sprites/UnitSprites" );
        factorySprites = Resources.LoadAll<Sprite> ( "Sprites/FactorySprites" );
        //foreach ( Sprite s in factorySprites  ) {
        //  Debug.Log (s.name);
        //}
        world.generateWorld ();
        if ( world.generationDone ) {
            for ( int x = 0; x < world.Width; x++ ) {
                for ( int y = 0; y < world.Height; y++ ) {

                    GameObject tile_go = new GameObject ();
                    tile_go.name = "Tile_" + x + "_" + y;
                    Tile tile_data = world.getTileAt ( x , y );
                    tile_go.transform.position = new Vector3 ( tile_data.x , tile_data.y , 0 );
                    tile_go.transform.SetParent ( this.transform );

                    SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer> ();
                    if ( tile_data.equals ( Tile.TileType.Water ) ) { 
                        tile_sr.sprite = tileSprites [ tileSprites.Length - 1 ];
                    }
                    else {
                        tile_sr.sprite = tileSprites [ 0 ];
                        setCoastSprite ( tile_data , tile_sr );
                    }
                }
            }
        }
        //Builds factory on the world
        //worldCreateFactory(2,3,Factory.factoryType.Armor);
        //world.getTileAt(2, 3).factory.createUnit(Unit.unitType.Land);

    }
    void update () {

    }

    public void createFactoryGO ( Tile tile_data ) {
        GameObject factory_go = new GameObject ();
        factory_go.transform.Translate ( new Vector2 ( tile_data.x , tile_data.y ) );
        factory_go.transform.SetParent ( GameObject.Find ( "Factories" ).transform );
        factory_go.name = "Factory_" + tile_data.x + "_" + tile_data.y + " (" + tile_data.factory.getFactoryType ().ToString () + ")";
        SpriteRenderer sr = factory_go.AddComponent<SpriteRenderer> ();

        if ( tile_data.factory.equals ( Factory.factoryType.Barracks ) ) {
            sr.sprite = factorySprites [ 0 ];
        }
        else if ( tile_data.factory.equals ( Factory.factoryType.Sea ) ) {
            sr.sprite = factorySprites [ 0 ];
        }
        else if ( tile_data.factory.equals ( Factory.factoryType.Armor ) ) {
            sr.sprite = factorySprites [ 0 ];
        }
        else if ( tile_data.factory.equals ( Factory.factoryType.Air ) ) {
            sr.sprite = factorySprites [ 0 ];
        }
        else {
            Debug.Log ( "Cannot Find Sprite. Using Default....." );
            sr.sprite = factorySprites [ 0 ];
        }
    }

    public void createUnitGO (Unit unit) {
        GameObject unit_go = new GameObject ();
        unit_go.transform.Translate ( new Vector2 ( unit.tile.x , unit.tile.y ) );
        unit_go.transform.SetParent ( GameObject.Find ( "Units" ).transform );
        unit_go.name = "Unit_" + unit.tile.x + "_" + unit.tile.y + " (" + unit.getUnitType ().ToString () + ")";
        SpriteRenderer sr = unit_go.AddComponent<SpriteRenderer> ();
        unitsInWorldController.Add (unit_go);
        foreach ( Sprite s in unitSprites ) {
            string name = s.name;
            if ( s.name == unit.spriteName ) {
                sr.sprite = s;
                Debug.Log ("Sprite set");
                break;
            }
        } 

    }

    private void setCoastSprite ( Tile tile_data , SpriteRenderer tile_sr ) {

        Tile [] neighbors = tile_data.getNeighbors ();

        string spriteName = "Grass_";

        if ( ( neighbors [ 0 ] != null ) && neighbors [ 0 ].equals ( Tile.TileType.Water ) ) {
            spriteName += "N";
        }
        if ( ( neighbors [ 1 ] != null ) && neighbors [ 1 ].equals ( Tile.TileType.Water ) ) {
            spriteName += "E";
        }
        if ( ( neighbors [ 2 ] != null ) && neighbors [ 2 ].equals ( Tile.TileType.Water ) ) {
            spriteName += "S";
        }
        if ( ( neighbors [ 3 ] != null ) && neighbors [ 3 ].equals ( Tile.TileType.Water ) ) {
            spriteName += "W";
        }

        //Debug.Log ( spriteName  + " " + tile_data.x + ", " + tile_data.y);
        if ( spriteName != "Grass_" ) {
            tile_data.setTileType ( Tile.TileType.Coast );
            foreach ( Sprite s in tileSprites ) {

                string name = s.name;
                if (s.name == spriteName) {
                    tile_sr.sprite = s;
                }
            }
        }
    }

    public void moveUnitSprite (Unit unit ,Tile oldTile, Tile newTile) {
        Vector2 newTileV = new Vector2 (newTile.x,newTile.y);
        for ( int i = 0; i < unitsInWorldController.Count; i++ ) {
            if (World.current.unitsInWorld[i].equals(unit) ) {
                unitsInWorldController [ i ].transform.Translate ( newTileV, Space.World);
                Debug.Log ( "Unit found" );
                break;
            }
        }
     }
}

