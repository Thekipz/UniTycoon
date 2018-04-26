﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VoxelBusters.NativePlugins;

public class MainMenu : MonoBehaviour {

    public SaveManager SaveManager { get; set; }

    public void Awake()
    {
        SaveManager = this.gameObject.AddComponent<SaveManager>();
    }
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


        //TODO: Make this work.

        //If there is not a save file, go to setup menu. If there is, go to game screen
        //if(!PlayerPrefs.HasKey("save"))
        //{
        //    this.gameObject.SetActive(false);

        //    //setup.gameObject.SetActive(true);
        //}
        //else
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}

    }

    public void StartNewGame()
    {
        SaveManager.DeleteSave();
    }
}
