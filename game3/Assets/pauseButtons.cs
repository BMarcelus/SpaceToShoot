using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseButtons : MonoBehaviour {

    public GameObject controlPanel;

    public void resume() {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void controls() {
        controlPanel.SetActive(true);
    }
    public void quit() {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void main() {
        Time.timeScale = 1;
        SceneManager.LoadScene("main_menu");
    }
}
