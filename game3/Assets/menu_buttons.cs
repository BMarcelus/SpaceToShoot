using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menu_buttons : MonoBehaviour {

    public void start_game() {
        SceneManager.LoadScene("b_1");
    }

    public void how_to_play(){
        SceneManager.LoadScene("instructions");
    }

    public void quit(){
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void main_menu() {
        SceneManager.LoadScene("main_menu");
    }
}
