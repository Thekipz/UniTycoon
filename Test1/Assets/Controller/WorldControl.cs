using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControl : MonoBehaviour {

    public Sprite groundSprite, buildingSprite;
    SpriteRenderer tile_sr;
    World world;


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
        Vector3 lastFramePosition;

        if (Input.GetMouseButtonDown(0))
        {
            lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastFramePosition.z = 0;

            //Test line to check mouse select functionality
            Tile test = ClickTile(lastFramePosition);
            test.Type = Tile.TileType.Building;
        }
    }
    // Function to get tile at mouse location
    Tile ClickTile(Vector3 coord)
     {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        return world.GetTileAt(x, y);
     }


    
}
