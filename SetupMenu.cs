using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupMenu : MonoBehaviour {

    //Function saving university name can be found in the SaveManager class

    public void OnClickOkButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
