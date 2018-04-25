using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
}
