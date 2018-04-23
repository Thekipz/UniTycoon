using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldControl : MonoBehaviour {


    public Text text_money;
    int money = 0;
    public CanvasGroup uigroup, buildMenu;
    public float elapsed = 0f;
    public Sprite groundSprite, buildingSprite, classSprite, gymSprite, labSprite, cafeSprite, librarySprite, parkingSprite, stadiumSprite, adminSprite;
    SpriteRenderer tile_sr;
    World world;
    Vector3 lastFramePosition;
    public enum Mode { Build, Destroy, Play };
    Mode mode = Mode.Play;
    Dictionary<Tile, GameObject> tileGameObjectMap;
    Tile select = null;
    public enum BuildingType { None, Dorm, Class, Gym, Lab, Cafe, Library, Parking, Stadium, Admin };
    BuildingType buildingType = BuildingType.None;
    // Initialize World
    void Start() {
        text_money.text = "Money: $" + money.ToString();
        world = new World(10, 10, 35);
        int scale = world.Scale;
        uigroup.alpha = 0;
        uigroup.blocksRaycasts = false;
        uigroup.interactable = false;
        buildMenu.alpha = 0;
        buildMenu.blocksRaycasts = false;
        buildMenu.interactable = false;


        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        //Create a display object for all of the tiles
        for (int i = 0; i < world.Width; i = i + 1) {
            for (int j = 0; j < world.Height; j = j + 1) {
                GameObject tile_go = new GameObject();
                Tile tile_data = world.GetTileAt(i, j);
                tileGameObjectMap.Add(tile_data, tile_go);
                tile_go.name = "Tile_" + i + "_" + j;
                tile_go.transform.localScale = new Vector3(scale, scale, scale);
                tile_go.transform.position = new Vector3(world.GetTileAt(i, j).X * scale, world.GetTileAt(i, j).Y * scale, 0);
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

        if (tile_go == null)
        {
            Debug.LogError("tileGameObject game object is null");
            return;
        }
  
        switch (tile_sr.Type)
        {
            case Tile.TileType.Empty:
                tile_go.GetComponent<SpriteRenderer>().sprite = groundSprite;
                break;
            case Tile.TileType.Building:
                tile_go.GetComponent<SpriteRenderer>().sprite = buildingSprite;
                break;
            case Tile.TileType.Class:
                tile_go.GetComponent<SpriteRenderer>().sprite = classSprite;
                break;
            case Tile.TileType.Gym:
                tile_go.GetComponent<SpriteRenderer>().sprite = gymSprite;
                break;
            case Tile.TileType.Lab:
                tile_go.GetComponent<SpriteRenderer>().sprite = labSprite;
                break;
            case Tile.TileType.Cafe:
                tile_go.GetComponent<SpriteRenderer>().sprite = cafeSprite;
                break;
            case Tile.TileType.Library:
                tile_go.GetComponent<SpriteRenderer>().sprite = librarySprite;
                break;
            case Tile.TileType.Parking:
                tile_go.GetComponent<SpriteRenderer>().sprite = parkingSprite;
                break;
            case Tile.TileType.Stadium:
                tile_go.GetComponent<SpriteRenderer>().sprite = stadiumSprite;
                break;
            case Tile.TileType.Admin:
                tile_go.GetComponent<SpriteRenderer>().sprite = adminSprite;
                break;
            default:
                Debug.LogError("TileTypeChanged - Unrecognized Tile Type.");
                break;
        }
    }

    // Update is called once per frame
    void Update() {

        elapsed += Time.deltaTime;
        if(elapsed >= 2f)
        {
            elapsed = elapsed % 2f;
            money++;
            text_money.text = "Money: $" + money.ToString();
        }
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
            Debug.Log("Mouse clicked at X:" + select.X + ", Y: " + select.Y);
            if (this.mode == Mode.Build)
            {
                if (select.Type == Tile.TileType.Empty)
                {
                    switch (this.buildingType)
                    {
                        case BuildingType.Dorm:
                            select.Type = Tile.TileType.Building;
                            Debug.Log("Dorm placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Class:
                            select.Type = Tile.TileType.Class;
                            Debug.Log("Class placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Gym:
                            select.Type = Tile.TileType.Gym;
                            Debug.Log("Gym placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Library:
                            select.Type = Tile.TileType.Library;
                            Debug.Log("Library placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Cafe:
                            select.Type = Tile.TileType.Cafe;
                            Debug.Log("Cafe placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Admin:
                            select.Type = Tile.TileType.Admin;
                            Debug.Log("Admin placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Stadium:
                            select.Type = Tile.TileType.Stadium;
                            Debug.Log("Stadium placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Parking:
                            select.Type = Tile.TileType.Parking;
                            Debug.Log("Parking placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                        case BuildingType.Lab:
                            select.Type = Tile.TileType.Lab;
                            Debug.Log("Lab placed at X:" + select.X + ", Y: " + select.Y);
                            break;
                    }
                }

                this.buildingType = BuildingType.None;
                this.mode = Mode.Play;
            } else
            {
                Debug.LogError("The selected tile is not empty");
            }

            if (this.mode == Mode.Destroy)
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
        switch (type)
        {
            case 1:
                this.buildingType = BuildingType.Dorm;
                break;
            case 2:
                this.buildingType = BuildingType.Class;
                break;
            case 3:
                this.buildingType = BuildingType.Gym;
                break;
            case 4:
                this.buildingType = BuildingType.Library;
                break;
            case 5:
                this.buildingType = BuildingType.Cafe;
                break;
            case 6:
                this.buildingType = BuildingType.Admin;
                break;
            case 7:
                this.buildingType = BuildingType.Stadium;
                break;
            case 8:
                this.buildingType = BuildingType.Parking;
                break;
            case 9:
                this.buildingType = BuildingType.Lab;
                break;
            default:
                this.buildingType = BuildingType.None;
                break;
        }


        buildMenu.alpha = 0;
        buildMenu.blocksRaycasts = false;
        buildMenu.interactable = false;
    }

}
