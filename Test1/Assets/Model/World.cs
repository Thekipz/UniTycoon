using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    Tile[,] tiles;
    int width;
    int height;
	int scale;
    //Default world constructor, width and height are the number of tiles on map
	public World(int width = 20, int height = 20, int scale=1)
    {
        this.Width = width;
        this.Height = height;
		this.Scale = scale;

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
        if (x > scale*width || x < 0 || y > scale*height || y < 0)
        {
            Debug.LogError("Tile ("+x+","+ y+") is out of range.");
            return null;
        }
        Debug.Log("Tile (" + x + "," + y + ")");
        return tiles[x, y];
    }




    //Mutators
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
  
	public int Scale{
		get{
			return scale;
		}
		set{
			scale = value;
		}
	}
}
