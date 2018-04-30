using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldControl : MonoBehaviour {


    public Text text_money;
    public CanvasGroup uigrouptop, uigroupbottom, buildMenu, popup, funds_message;
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
	University university;
    BuildingType buildingType = BuildingType.None;

	public void hideCG(CanvasGroup cg){
		cg.alpha = 0;
		cg.blocksRaycasts = false;
		cg.interactable = false;
	}
	public void showCG(CanvasGroup cg){
		cg.alpha = 1;
		cg.blocksRaycasts = true;
		cg.interactable = true;
	}
	public void updateHUD(){
		text_money.text = university.HUD();
	}
    // Initialize World
    void Start() {
		Debug.Log ("Start...");
		GameDifficulty gamediff = GameDifficulty.Normal;
		university = new University (gamediff);
		updateHUD ();
        world = new World(10, 10, 35);
        int scale = world.Scale;
		showCG (uigrouptop);
		showCG (uigroupbottom);
		hideCG (buildMenu);
		hideCG (popup);
        hideCG(funds_message);


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
                
				tile_go.GetComponent<SpriteRenderer> ().sprite = groundSprite;
				tile_sr.StudentCapacity = 0;
				tile_sr.ResidentCapacity = 0;
                break;
			case Tile.TileType.Building:
				tile_go.GetComponent<SpriteRenderer> ().sprite = buildingSprite;
				tile_sr.StudentCapacity = 0;
				tile_sr.ResidentCapacity = 500;
                break;
            case Tile.TileType.Class:
                tile_go.GetComponent<SpriteRenderer>().sprite = classSprite;
				tile_sr.StudentCapacity = 200;
				tile_sr.ResidentCapacity = 0;
                break;
			case Tile.TileType.Gym:
				tile_go.GetComponent<SpriteRenderer> ().sprite = gymSprite;
				tile_sr.StudentCapacity = 250;
				tile_sr.ResidentCapacity = 50;
				break;
            case Tile.TileType.Lab:
                tile_go.GetComponent<SpriteRenderer>().sprite = labSprite;
				tile_sr.StudentCapacity = 250;
				tile_sr.ResidentCapacity = 50;
                break;
            case Tile.TileType.Cafe:
                tile_go.GetComponent<SpriteRenderer>().sprite = cafeSprite;
				tile_sr.StudentCapacity = 0;
				tile_sr.ResidentCapacity = 50;
                break;
            case Tile.TileType.Library:
                tile_go.GetComponent<SpriteRenderer>().sprite = librarySprite;
				tile_sr.StudentCapacity = 500;
				tile_sr.ResidentCapacity = 50;
                break;
            case Tile.TileType.Parking:
                tile_go.GetComponent<SpriteRenderer>().sprite = parkingSprite;
				tile_sr.StudentCapacity = 0;
				tile_sr.ResidentCapacity = 100;
                break;
            case Tile.TileType.Stadium:
                tile_go.GetComponent<SpriteRenderer>().sprite = stadiumSprite;
				tile_sr.StudentCapacity = 1000;
				tile_sr.ResidentCapacity = 100;
				break;
            case Tile.TileType.Admin:
                tile_go.GetComponent<SpriteRenderer>().sprite = adminSprite;
				tile_sr.StudentCapacity = 100;
				tile_sr.ResidentCapacity = 10;
				break;
            default:
                Debug.LogError("TileTypeChanged - Unrecognized Tile Type.");
                break;
        }
		updateHUD ();
    }

    // Update is called once per frame
    void Update() {

        elapsed += Time.deltaTime;
        if(elapsed >= 2f)
        {
            elapsed = elapsed % 2f;
			university.updateUniversityVars (world);
			updateHUD ();
		}
        //Checks if there is a UI element in front of a tile on touch, if there is not, then selects the tile
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else if (Input.GetMouseButtonDown(0)&&buildMenu.alpha == 0&&popup.alpha == 0)
        {
            lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastFramePosition.z = 0;
            select = ClickTile(lastFramePosition);
			if (select == null)
				return;
            Debug.Log("Mouse clicked at X:" + select.X.ToString() + ", Y: " + select.Y.ToString());
            if (this.mode == Mode.Build)
            {
                if (select.Type == Tile.TileType.Empty)
                {
                    switch (this.buildingType)
                    {
                        case BuildingType.Dorm:
                            if (this.university.Coffers >= 10000)
                            {
                                this.university.Coffers = this.university.Coffers - 10000;
                                select.Type = Tile.TileType.Building;
                                Debug.Log("Dorm placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Class:
                            if (this.university.Coffers >= 10000)
                            {
                                this.university.Coffers = this.university.Coffers - 10000;
                                select.Type = Tile.TileType.Class;
                                Debug.Log("Class placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Gym:
                            if (this.university.Coffers >= 10000)
                            {
                                this.university.Coffers = this.university.Coffers - 10000;
                                select.Type = Tile.TileType.Gym;
                                Debug.Log("Gym placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Library:
                            if (this.university.Coffers >= 100000)
                            {
                                this.university.Coffers = this.university.Coffers - 100000;
                                select.Type = Tile.TileType.Library;
                                Debug.Log("Library placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Cafe:
                            if (this.university.Coffers >= 100000)
                            {
                                this.university.Coffers = this.university.Coffers - 100000;
                                select.Type = Tile.TileType.Cafe;
                                Debug.Log("Cafe placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Admin:
                            if (this.university.Coffers >= 100000)
                            {
                                this.university.Coffers = this.university.Coffers - 100000;
                                select.Type = Tile.TileType.Admin;
                                Debug.Log("Admin placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Stadium:
                            if (this.university.Coffers >= 1)
                            {
                                this.university.Coffers = this.university.Coffers - 1;
                                select.Type = Tile.TileType.Stadium;
                                Debug.Log("Stadium placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Parking:
                            if (this.university.Coffers >= 1)
                            {
                                this.university.Coffers = this.university.Coffers - 1;
                                select.Type = Tile.TileType.Parking;
                                Debug.Log("Parking placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
                            break;
                        case BuildingType.Lab:
                            if (this.university.Coffers >= 1)
                            {
                                this.university.Coffers = this.university.Coffers - 1;
                                select.Type = Tile.TileType.Lab;
                                Debug.Log("Lab placed at X:" + select.X + ", Y: " + select.Y);
                            }
                            else
                            {
                                showCG(funds_message);
                                Debug.Log("Not enough money");
                            }
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
			showCG (uigrouptop);
			showCG (uigroupbottom);
			showCG (popup);
        }
            else if (this.mode == Mode.Play)
            {
                //------------------------------------------------------#TODO: Add popup for building status or w/e
            }
        }
        
            
          }

    //Load when when scene is enabled
    private void OnEnable()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            SaveManager.Instance.Load();
        }
        else //No saved data to load means this is a new game
        {
            //Initailize data 

            //Example
            //SaveManager.Instance.saveState.gamediff = GameDifficulty.Normal;
            //SaveManager.Instance.saveState.university = new University(gamediff);

            //SaveManager.Instance.saveState.tileGameObjectMap = new Dictionary<Tile, GameObject>();
            ////Create a display object for all of the tiles
            //for (int i = 0; i < world.Width; i = i + 1)
            //{
            //    for (int j = 0; j < world.Height; j = j + 1)
            //    {
            //        GameObject tile_go = new GameObject();
            //        Tile tile_data = world.GetTileAt(i, j);
            //        tileGameObjectMap.Add(tile_data, tile_go);
            //        tile_go.name = "Tile_" + i + "_" + j;
            //        tile_go.transform.localScale = new Vector3(scale, scale, scale);
            //        tile_go.transform.position = new Vector3(world.GetTileAt(i, j).X * scale, world.GetTileAt(i, j).Y * scale, 0);
            //        tile_sr = tile_go.AddComponent<SpriteRenderer>();
            //        tile_sr.sprite = groundSprite;
            //        tile_go.transform.SetParent(this.transform, true);
            //        //This will update the tile sprite when the type is changed
            //        tile_data.RegisterCallBack(TileTypeChanged);
            //    }
            //}

        }
    }

    //Save data when scene is diabled
     private void OnDisable()
    {
        SaveManager.Instance.Save();
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
		showCG (buildMenu);
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
		
		
		hideCG (popup);
    }
    // Cancel button for demolish popup
    public void Cancel()
    {
        this.mode = Mode.Play;
        hideCG(funds_message);
		hideCG (buildMenu);
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


       
		hideCG (buildMenu);
    }

}
