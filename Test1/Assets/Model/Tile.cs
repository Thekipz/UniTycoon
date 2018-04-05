using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile {

    public enum TileType { Empty, Building };
    TileType type = TileType.Empty;
    World world;
    int x, y;
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
  
   

}
