using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dscreen_button : MonoBehaviour {

    public void try_again() {
        string prev = PlayerPrefs.GetString("previous");
        SceneManager.LoadScene(prev);
    }

    public void quit() {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
