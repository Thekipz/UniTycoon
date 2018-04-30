using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that does all of the saving/loading logic
/// </summary>
public class SaveManager : MonoBehaviour {

    public static SaveManager Instance { get; set; }
    public SaveLoad saveState;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

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
            Debug.Log("Loading saved file. " + saveState.univeristyName);
        }
    }

    //Sets the university name and saves it
    public void SetUniversityName(string newName)
    {
        saveState = new SaveLoad();
        saveState.univeristyName = newName;
        Save();
        Debug.Log(saveState.univeristyName + " is saved.");
    }

    //Delete save file in player prefs and start a clean save
    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
