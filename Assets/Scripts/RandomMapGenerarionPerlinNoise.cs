using UnityEngine;
using System.Collections;

public class RandomMapGenerarionPerlinNoise {
    public Tile.TileType tileTypeforWorldgeneration ( int x , int y ) {

        int mapheight = World.current.Height;
        int mapwidth = World.current.Width;

        Vector2 shift = new Vector2 ( Random.Range(0,5) , Random.Range(0,5) ); // play with this to shift map around
        float zoom = .03f; // play with this to zoom into the noise field


        Vector2 pos = zoom * ( new Vector2 ( x , y ) ) + shift;
        float noise = Mathf.PerlinNoise ( pos.x , pos.y );
        if ( noise < 0.4f ) {
            return Tile.TileType.Water;
        }
        else if ( noise < 0.9f ) {
            return Tile.TileType.Grass;

        }
        else {
            return Tile.TileType.Water;
        }
    }
}
