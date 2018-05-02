using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SaveLoad {
    public bool isNew = true;
    public string univeristyName = "DemoUniveristy";
    public int money = 10000;
    public University university;
    public int worldResCap;
    public int worldStudCap;
    public int worldWidth;
    public int worldHeight;
    public int worldScale;
    public Tile[] tiles;
}
