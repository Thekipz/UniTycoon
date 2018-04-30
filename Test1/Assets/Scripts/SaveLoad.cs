using System;
using System.Collections.Generic;
using UnityEngine;

//This class holds all the data that needs to be saved or is loaded.
[Serializable]
public class SaveLoad {

    public string univeristyName;
    public int money;
    public Dictionary<Tile, GameObject> tileGameObjectMap;    //Save tiles and objects in them
}
