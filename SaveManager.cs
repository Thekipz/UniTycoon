using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance { get; set; }
    public SaveLoad saveState;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();
    }

    //Save the whole SaveLoad script the the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveLoad>(saveState));
    }

    //Load the previous save from the player pref
    public void Load()
    {
        //Do we already have a save?
        if(PlayerPrefs.HasKey("save"))
        {
            saveState = Helper.Deserialize<SaveLoad>(PlayerPrefs.GetString("save"));
            Debug.Log("Loading saved file. " + saveState.ToString());
        }
        else
        {
            saveState = new SaveLoad();
            Save();
            Debug.Log("No save file found, creating a new one!");
        }
    }

    //Sets the university name and saves it
    public void SetUniversityName(string newName)
    {
        saveState.univeristyName = newName;
        PlayerPrefs.Save();
        Debug.Log(saveState.univeristyName + " is saved.");
    }
}
