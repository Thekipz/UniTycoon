using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using VoxelBusters.NativePlugins;

public class MainMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public float sliderVolume;

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

    //Start new game button
    public void StartNewGame()
    {
        SaveManager.Instance.DeleteSave();
    }

    //Settings Menu Volume Control
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        sliderVolume = volume;
    }

    //Setting Audio button (Turns Audio off)
    public void OnAudioOff()
    {
        audioMixer.SetFloat("volume", -88);
    }

    //Setting Audio button (Turns Audio off)
    public void OnAudioOn()
    {
        audioMixer.SetFloat("volume", sliderVolume);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
