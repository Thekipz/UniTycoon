using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldControl : MonoBehaviour {
    public Image popup;
    

    public Sprite groundSprite, buildingSprite;
    SpriteRenderer tile_sr;
    World world;
    Vector3 lastFramePosition;
    public enum Mode { Build, Destroy, Play };
    Mode mode = Mode.Play;
    // Initialize World
    void Start () {
        world = new World();
       

        //Create a display object for all of the tiles
        for (int i = 0; i < world.Width; i++){
            for (int j = 0; j < world.Height; j++){
                GameObject tile_go = new GameObject();
                Tile tile_data = world.GetTileAt(i, j);
                tile_go.name = "Tile_" + i + "_" + j;
                tile_go.transform.position = new Vector3(world.GetTileAt(i, j).X, world.GetTileAt(i, j).Y,0);
                tile_sr = tile_go.AddComponent<SpriteRenderer>();
                tile_sr.sprite = groundSprite;
                tile_go.transform.SetParent(this.transform, true);
                //This will update the tile sprite when the type is changed
                tile_data.RegisterCallBack((tile) => { TileTypeChanged(tile, tile_go); });
            }
        }     
	}
    //Function called when a tiletype is changed in order to update the sprite
    void TileTypeChanged(Tile tile_sr, GameObject tile_go)
    {
        if (tile_sr.Type == Tile.TileType.Empty)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = groundSprite;
        }
        else if (tile_sr.Type == Tile.TileType.Building)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = buildingSprite;
        }
        else
        {
            Debug.LogError("TileTypeChanged - Unrecognized Tile Type.");
        }
    }
    // Update is called once per frame
    void Update() {


        //Checks if there is a UI element in front of a tile on touch, if there is not, then selects the tile
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastFramePosition.z = 0;

            //Test line to check mouse select functionality
            Tile select = ClickTile(lastFramePosition);
            if (this.mode == Mode.Build)
            {
                select.Type = Tile.TileType.Building;
                this.mode = Mode.Play;
            } else if (this.mode == Mode.Destroy)
            {
                
                //------------------------------------------------------#TODO: Add confirmation popup for destroy
                select.Type = Tile.TileType.Empty;
                this.mode = Mode.Play;
            }
            else if (this.mode == Mode.Play)
            {
                //------------------------------------------------------#TODO: Add popup for building status or w/e
            }
        }
    }
    // Function to get tile at mouse location
    Tile ClickTile(Vector3 coord)
     {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        return world.GetTileAt(x, y);
     }
    //Called on "Build" button click
    public void SetMode_Build()
    {
        this.mode = Mode.Build;
    }
    //Called on "Destroy" button click
    public void SetMode_Destroy()
    {
        this.mode = Mode.Destroy;
         
    }

    
}
