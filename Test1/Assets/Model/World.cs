using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    Tile[,] tiles;
    int width;
    int height;
    //Default world constructor, width and height are the number of tiles on map
    public World(int width = 20, int height = 20)
    {
        this.Width = width;
        this.Height = height;

        tiles = new Tile[width, height];

        for( int i = 0; i < width; i++){
            for(int j = 0; j < height; j++) {
                tiles[i, j] = new Tile(this, i, j);
            }
        }
        Debug.Log("World created with " + (width * height) + "tiles");
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > width || x < 0 || y > height || y < 0)
        {
            Debug.LogError("Tile ("+x+","+ y+") is out of range.");
            return null;
        }
        return tiles[x, y];
    }
    public int Width{
        get{
            return width;
        }
        set{
            width = value;
        }
    }

    public int Height{
        get{
            return height;
        }
        set{
            height = value;
        }
    }
  
}
