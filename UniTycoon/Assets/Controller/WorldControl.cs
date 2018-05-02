using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldControl : MonoBehaviour {


    public Text text_money,adminf1,adminf2,adminf3,hintCurrLvl,hintNxtLvl,hintReq1,hintReq2,hintReq3,hintReq4,advanceButtonTxt;
    public CanvasGroup uigrouptop, uigroupbottom, buildMenu, popup, funds_message, admin_menu,hintpopup,randompanel;
    public float elapsed = 0f;
    public float timeInterval = 2f;
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
    public InputField AMF1, AMF2, AMF3; //admin menu inputfields
    int amf1val, amf2val, amf3val; //value of admin menu entries
    public Button bdorm, bclass, bgym, blib, bcafe, badmin, bstadium, bpark, blab, bcancel;
	
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
        hideCG(admin_menu);
        hideCG(hintpopup);

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
        updateHUD();
        elapsed += Time.deltaTime;
        if(elapsed >= timeInterval)
        {
            elapsed = elapsed % timeInterval;
			university.updateUniversityVars (world);
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
                            if (this.university.Coffers >= 50000)
                            {
                                this.university.Coffers = this.university.Coffers - 50000;
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
                }else{
                    Debug.LogError("The selected tile is not empty");
                }

                this.buildingType = BuildingType.None;
                this.mode = Mode.Play;
            } 
            else if (this.mode == Mode.Destroy)
            {
			showCG (uigrouptop);
			showCG (uigroupbottom);
			showCG (popup);
            }
            else if (this.mode == Mode.Play)
            {
                switch (select.Type)
                {
                    case Tile.TileType.Building:
                        
                        break;
                    case Tile.TileType.Class:
                        
                        break;
                    case Tile.TileType.Gym:
                        
                        break;
                    case Tile.TileType.Library:
                        
                        break;
                    case Tile.TileType.Cafe:
                        
                        break;
                    case Tile.TileType.Admin:
                        showCG(admin_menu);
                        break;
                    case Tile.TileType.Stadium:
                       
                        break;
                    case Tile.TileType.Parking:
                        
                        break;
                    case Tile.TileType.Lab:
                       
                        break;
                }
            }
        }
        
            
          }

    //UTILITY FUNCTIONS AND CALLBACKS

    public void hideCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;

    }
    public void showCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;

    }
    public void updateHUD(){ text_money.text = university.HUD(); }

    public void timeAcc(bool on){
        if(on){
            //university.activateTimeAcc();
            timeInterval = timeInterval / 100;
        }else{
            //university.deactivateTimeAcc();
            timeInterval = 2f;
        }
    }

    public void AMF1changed(){
        amf1val = int.Parse(AMF1.text);
        int total = (int)((double)amf1val * university.TuitionRate);
        adminf1.text = "Dedicate $" + total.ToString() + " to recruit " + amf1val.ToString() + " students.";
    }
    public void AMF1pressed(){
        university.addScholarship((double)amf1val * university.TuitionRate);
        university.addStudents(amf1val);
    }

    public void AMF2changed(){
        amf2val = int.Parse(AMF2.text);
        int total = (int)((double)amf2val * university.RentRate);
        adminf2.text = "Dedicate $" + total.ToString() + " to recruit " + amf2val.ToString() + " residents.";
    }
    public void AMF2pressed(){
        university.addGrant((double)amf2val * university.RentRate);
        university.addResidents(amf2val);
    }

    public void AMF3changed(){
        amf3val = int.Parse(AMF3.text);
        if (amf3val <= university.Coffers)
            adminf3.text = "Spend $" + amf3val.ToString() + " on advertising";
        else
            adminf3.text = "$"+amf3val.ToString()+" is bigger than your budget of $"+university.Coffers.ToString();
    }
    public void AMF3pressed(){
        if(amf3val <= university.Coffers)
            university.advertise(amf3val);
    }

    public void AMFclose(){
        hideCG(admin_menu);
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
        if (university.breakingGround()){
            hideCG(bgym.GetComponent<CanvasGroup>());
            hideCG(blib.GetComponent<CanvasGroup>());
            hideCG(bcafe.GetComponent<CanvasGroup>());
            hideCG(bstadium.GetComponent<CanvasGroup>());
            hideCG(bpark.GetComponent<CanvasGroup>());
            hideCG(blab.GetComponent<CanvasGroup>());
        }else if(university.initialExpansion()){
            showCG(bgym.GetComponent<CanvasGroup>());
            showCG(blib.GetComponent<CanvasGroup>());
            hideCG(bcafe.GetComponent<CanvasGroup>());
            hideCG(bstadium.GetComponent<CanvasGroup>());
            hideCG(bpark.GetComponent<CanvasGroup>());
            hideCG(blab.GetComponent<CanvasGroup>());
        }else if(university.stage1()){
            showCG(bcafe.GetComponent<CanvasGroup>());
            showCG(bpark.GetComponent<CanvasGroup>());
            hideCG(blab.GetComponent<CanvasGroup>());
            hideCG(bstadium.GetComponent<CanvasGroup>());
        }else if(university.stage2()){
            showCG(bstadium.GetComponent<CanvasGroup>());
            hideCG(blab.GetComponent<CanvasGroup>());
        }else if(university.stage3()){
            showCG(blab.GetComponent<CanvasGroup>());
        }
        this.mode = Mode.Build;
    }
    //Called on "Destroy" button click
    public void SetMode_Destroy()
    {
        this.mode = Mode.Destroy;
    }
    public void Advance_Clicked()
    {
        university.advanceLevel(world);
    }
    //Called on Get Hint button click
    public void Get_Hint_Clicked()
    {
        if (university.breakingGround()){
            hintCurrLvl.text = "Current Level\nBreaking Ground";
            hintNxtLvl.text = "Next Level\nInitial Expansion";
            hintReq1.text = "Build an administration building to manage your university from.";
            hintReq2.text = "Build a campus with a student capacity of 1000 or greater.";
            hintReq3.text = "Build a campus with a resident capacity of 500 or greater.";
            hintReq4.text = "";
        }
        else if (university.initialExpansion()){
            hintCurrLvl.text = "Current Level\nInitial Expansion";
            hintNxtLvl.text = "Next Level\nStage 1";
            hintReq1.text = "Build a Gym";
            hintReq2.text = "Build a Library";
            hintReq3.text = "Have a student capacity greater than or equal to 5000";
            hintReq4.text = "Have a resident capacity greater than or equal to 1000";
        }
        else if (university.stage1()){
            hintCurrLvl.text = "Current Level\nStage 1";
            hintNxtLvl.text = "Next Level\nStage 2";
            hintReq1.text = "Build a Cafe";
            hintReq2.text = "Build a Parking Garage";
            hintReq3.text = "Have a student population greater than or equal to 5000";
            hintReq4.text = "Have a resident population greater than or equal to 1000";
        }
        else if (university.stage2()){
            hintCurrLvl.text = "Current Level\nStage 2";
            hintNxtLvl.text = "Next Level\nStage 3";
            hintReq1.text = "Build a Stadium";
            hintReq2.text = "Have a student population greater than or equal to 10000";
            hintReq3.text = "Have a resident population greater than or equal to 3000";
            hintReq4.text = "Have $150,000 in the bank or more";
        }
        else if (university.stage3()){
            hintCurrLvl.text = "Current Level\nStage 3";
            hintNxtLvl.text = "Next Level\nVictory!";
            hintReq1.text = "Build a lab";
            hintReq2.text = "Have a student population greater than or equal to 5000";
            hintReq3.text = "Have a resident population greater than or equal to 1000";
            hintReq4.text = "Have $1,000,000 in the bank or more";
        }
        advanceButtonTxt.text = "Check ability to advance";
        hideCG(uigrouptop);
        hideCG(uigroupbottom);
        showCG(hintpopup);
    }
    //Exit hint button
    public void Exit_Hint()
    {
        showCG(uigrouptop);
        showCG(uigroupbottom);
        hideCG(hintpopup);
        advanceButtonTxt.text = "Check ability to advance";
    }
    //Check upgrade button
    public void Check_Upgrade()
    {
        if (university.canAdvanceLevel(world))
            advanceButtonTxt.text = "Able to Advance!";
        else
            advanceButtonTxt.text = "Unable to advance, requirements not met!";
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
        hideCG(popup);
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
