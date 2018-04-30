using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VoxelBusters.NativePlugins;

public class MainMenu : MonoBehaviour {

    public void PlayGame()
    {
 
        if (PlayerPrefs.HasKey("save"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            NPBinding.UI.ShowToast("There is no saved game. Start a new game!", eToastMessageLength.SHORT);
        }

    }

    public void StartNewGame()
    {
        SaveManager.Instance.DeleteSave();
    }
}
