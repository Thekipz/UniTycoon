using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldControl : MonoBehaviour {


   
    public CanvasGroup uigroup, buildMenu;
    
    public Sprite groundSprite, buildingSprite;
    SpriteRenderer tile_sr;
    World world;
    Vector3 lastFramePosition;
    public enum Mode { Build, Destroy, Play };
    Mode mode = Mode.Play;
    Dictionary<Tile, GameObject> tileGameObjectMap;
    Tile select = null;
    public enum BuildingType {None, Dorm,Class};
    BuildingType buildingType = BuildingType.None;
    // Initialize World
    void Start () {
        world = new World(30,15,20);
		int scale = world.Scale;
        uigroup.alpha = 0;
        uigroup.blocksRaycasts = false;
        uigroup.interactable = false;
        buildMenu.alpha = 0;
        buildMenu.blocksRaycasts = false;
        buildMenu.interactable = false;


        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        //Create a display object for all of the tiles
        for (int i = 0; i < world.Width; i = i + 1){
            for (int j = 0; j < world.Height; j = j + 1) { 
                GameObject tile_go = new GameObject();
                Tile tile_data = world.GetTileAt(i, j);
                tileGameObjectMap.Add(tile_data, tile_go);
                tile_go.name = "Tile_" + i + "_" + j;
				tile_go.transform.localScale = new Vector3 (scale,scale,scale);
                tile_go.transform.position = new Vector3(world.GetTileAt(i, j).X*scale, world.GetTileAt(i, j).Y*scale,0);
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
			Debug.Log ("Mouse clicked at X:"+select.X+", Y: "+select.Y);
            if (this.mode == Mode.Build) 
            {
             
              
                if (select.Type == Tile.TileType.Empty)
                {
                    if (this.buildingType == BuildingType.Dorm)
                    {
                        select.Type = Tile.TileType.Building;
						Debug.Log ("Dorm placed at X:"+select.X+", Y: "+select.Y);
                    }else if (this.buildingType == BuildingType.Class)
                    {
                        //select.Type = Tile.TileType.Class
                    }

                    this.buildingType = BuildingType.None;
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
       
        return world.GetTileAt(x/world.Scale, y/world.Scale);
    }
    //Called on "Build" button click
    public void SetMode_Build()
    {
        buildMenu.alpha = 1;
        buildMenu.blocksRaycasts = true;
        buildMenu.interactable = true;
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
    public void Cancel()
    {
        this.mode = Mode.Play;
        uigroup.alpha = 0;
        uigroup.blocksRaycasts = false;
        uigroup.interactable = false;
        buildMenu.alpha = 0;
        buildMenu.blocksRaycasts = false;
        buildMenu.interactable = false;
    }
    public void BuildingSelect(int type)
    {
		/*
		switch (type) {
		case "Dorm":
			this.buildingType = BuildingType.Dorm;
			break;
		}
		*/
		//Debug.Log (type);
        if (type != 1000)
        {
            this.buildingType = BuildingType.Dorm;
        }
        buildMenu.alpha = 0;
        buildMenu.blocksRaycasts = false;
        buildMenu.interactable = false;
    }


}
