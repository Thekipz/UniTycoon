using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

public class SetupMenu : MonoBehaviour {

    public Text Input;
    public string uniName;
    public GameObject MainMenu;
    public SaveManager SaveManager;

    public void Awake()
    {
        SaveManager = MainMenu.GetComponent<MainMenu>().SaveManager;
    }
    public void OnClickOkButton()
    {
        SaveManager.SetUniversityName(uniName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        NPBinding.UI.ShowToast("Welcome to " + uniName + "!", eToastMessageLength.SHORT);
    }

    public void EditUniName(string inputName)
    {
        uniName = inputName;      
    }
}
