using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldControl : MonoBehaviour {


   
    public CanvasGroup uigroup;
    
    public Sprite groundSprite, buildingSprite;
    SpriteRenderer tile_sr;
    World world;
    Vector3 lastFramePosition;
    public enum Mode { Build, Destroy, Play };
    Mode mode = Mode.Play;
    Dictionary<Tile, GameObject> tileGameObjectMap;
    Tile select = null;

    // Initialize World
    void Start () {
        world = new World();

        uigroup.alpha = 0;
        uigroup.blocksRaycasts = false;
        uigroup.interactable = false;

        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        //Create a display object for all of the tiles
        for (int i = 0; i < world.Width; i++){
            for (int j = 0; j < world.Height; j++){
                GameObject tile_go = new GameObject();
                Tile tile_data = world.GetTileAt(i, j);
                tileGameObjectMap.Add(tile_data, tile_go);
                tile_go.name = "Tile_" + i + "_" + j;
                tile_go.transform.position = new Vector3(world.GetTileAt(i, j).X, world.GetTileAt(i, j).Y,0);
                tile_sr = tile_go.AddComponent<SpriteRenderer>();
                tile_sr.sprite = groundSprite;
                tile_go.transform.SetParent(this.transform, true);
                //This will update the tile sprite when the type is changed
                tile_data.RegisterCallBack(TileTypeChanged);
            }
        }     
	}


    //Function called when a tiletype is changed in order to update the sprite
    void TileTypeChanged(Tile tile_sr)
    {
        if (tileGameObjectMap.ContainsKey(tile_sr) == false)
        {
            Debug.LogError("tileGameObject doesnt contain the tile_data");
            return;
        }
        GameObject tile_go = tileGameObjectMap[tile_sr];
        if(tile_go == null)
        {
            Debug.LogError("tileGameObject game object is null");
            return;
        }
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

            
            select = ClickTile(lastFramePosition);
            if (this.mode == Mode.Build) 
            {   
                //TODO: open build menu
                if (select.Type == Tile.TileType.Empty)
                {
                    select.Type = Tile.TileType.Building;
                    this.mode = Mode.Play;
                } else
                {
                    Debug.LogError("The selected tile is not empty");
                }

            }
             else if (this.mode == Mode.Destroy)
            {

                uigroup.alpha = 1;
                uigroup.blocksRaycasts = true;
                uigroup.interactable = true;
                
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
    //confirm button for demolish popup
    public void Confirm_Destroy()
    {
        select.Type = Tile.TileType.Empty;
        this.mode = Mode.Play;
        uigroup.alpha = 0;
        uigroup.blocksRaycasts = false;
        uigroup.interactable = false;
    }
    // Cancel button for demolish popup
    public void Cancel_Destroy()
    {
        this.mode = Mode.Play;
        uigroup.alpha = 0;
        uigroup.blocksRaycasts = false;
        uigroup.interactable = false;
    }


}
