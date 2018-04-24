using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile {

    public enum TileType { Empty, Building, Class, Gym, Lab, Cafe, Library, Parking, Stadium, Admin };
    TileType type = TileType.Empty;
    World world;
    int x, y;
	int studentCapacity,residentCapacity;
    Action<Tile> cbTileTypeChanged;


    /*  Used in conjunction with TileTypeChanged 
     * in WorldControler to update sprites
     */
    public void RegisterCallBack(Action<Tile> callback)
    {
        cbTileTypeChanged = callback;
    }
 
    //Tile Constructor
     public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
		studentCapacity = 0;
		residentCapacity = 0;
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
    }

    public int Y
    {
        get
        {
            return y;
        }
    }
	public int StudentCapacity
	{
		get{ 
			return studentCapacity;
		}
		set{
			world.addStudentCapacity (-1*studentCapacity);
			studentCapacity = value;
			world.addStudentCapacity (studentCapacity);

		}
	}
	public int ResidentCapacity
	{
		get{
			return residentCapacity;
		}
		set{ 
			world.addResidentCapacity (-1 * residentCapacity);
			residentCapacity = value;
			world.addResidentCapacity (residentCapacity);
		}
	}

}
