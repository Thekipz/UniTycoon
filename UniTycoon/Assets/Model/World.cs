using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    Tile[,] tiles;
    int width,height,scale;
	int totalStudentCapacity,totalResidentCapacity;
    //Default world constructor, width and height are the number of tiles on map
	//scale changes the size of tiles
    public World(int width = 10, int height = 10, int scale=1)
    {
        this.Width = width;
        this.Height = height;
		this.Scale = scale;

        tiles = new Tile[width, height];

        for( int i = 0; i < width; i++){
            for(int j = 0; j < height; j++) {
                tiles[i, j] = new Tile(i, j,Tile.TileType.Empty);
            }
        }
        Debug.Log("World created with " + (width * height) + "tiles");
    }
    public World()
    {
        width = 10;
        height = 10;
        scale = 35;

        tiles = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = new Tile(i, j);
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
        Debug.Log("Tile (" + x + "," + y + ") Type: " + tiles[x, y].Type);
        return tiles[x, y];
    }
	public void addStudentCapacity(int cap)
	{
		totalStudentCapacity += cap;
	}
	public void addResidentCapacity(int cap)
	{
		totalResidentCapacity += cap;
	}
    public void setTileData(Tile[,] newTiles)
    {
        for (int i = 0;i < Width;i++){
            for (int j = 0; j < Height;j++){
                tiles[i, j] = newTiles[i, j];
            }
        }
    }


    //Mutators
    public Tile[,] Tiles{
        get{
            return tiles;
        }
        set{
            tiles = value;
        }
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
  
	public int Scale{
		get{
			return scale;
		}
		set{
			scale = value;
		}
	}

	public int TotalStudentCapacity
	{
		get{ 
			return totalStudentCapacity;
		}
		set{
			totalStudentCapacity = value;
		}
	}
	public int TotalResidentCapacity
	{
		get{
			return totalResidentCapacity;
		}
		set{ 
			totalResidentCapacity = value;
		}
	}
}
