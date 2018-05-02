using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile
{

    public enum TileType { Empty, Building, Class, Gym, Lab, Cafe, Library, Parking, Stadium, Admin };
    TileType type = TileType.Empty;

    int x, y;
    int studentCapacity, residentCapacity;
    Action<Tile> cbTileTypeChanged;

    /*  Used in conjunction with TileTypeChanged 
     * in WorldControler to update sprites
     */
    public void RegisterCallBack(Action<Tile> callback)
    {
        cbTileTypeChanged = callback;
    }

    //Tile Constructor
    public Tile(int x, int y,TileType tileType)
    {
        this.x = x;
        this.y = y;
        type = tileType;
        studentCapacity = 0;
        residentCapacity = 0;
    }
    public Tile(int x, int y)
    {
        this.x = x;
        this.y = y;
        type = TileType.Empty;
        studentCapacity = 0;
        residentCapacity = 0;
    }
    public Tile()
    {
        
    }
    /*
     * Mutators for private tile variables
     */
     public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
            if(cbTileTypeChanged!=null)
            cbTileTypeChanged(this);
        }
    }
    public int X
    {
        get
        {
            return x;
        }
        set{
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
        set{
            y = value;
        }
    }
	public int StudentCapacity
	{
		get{ 
			return studentCapacity;
		}
		set{
			studentCapacity = value;

		}
	}
	public int ResidentCapacity
	{
		get{
			return residentCapacity;
		}
		set{ 
			residentCapacity = value;
		}
	}

}
