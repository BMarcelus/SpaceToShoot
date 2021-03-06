﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class b_level1 : MonoBehaviour {
    
    public static int killCount = 0;
    public GameObject winScreen;
    public int level;

    public void nextLevel() {
        if (level == 1) {
            SceneManager.LoadScene("b_2");
        }
        else if (level == 2) {
            SceneManager.LoadScene("b_3");
        }
        else if (level == 3) {
            SceneManager.LoadScene("b_4");
        }
    }

	// Use this for initialization
	void Start () {
        if (Time.timeScale != 1) {
            Time.timeScale = 1;
        }
        killCount = 0;
        winScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (level == 1) {
            if (killCount >= 18) {
                Time.timeScale = 0;
                winScreen.SetActive(true);
            }
        }
        if (level == 2) {
            if (killCount >= 19) {
                Time.timeScale = 0;
                winScreen.SetActive(true);
            }
        }
        if (level == 3) {
            if (killCount >= 24) {
                Time.timeScale = 0;
                winScreen.SetActive(true);
            }
        }
        if (level == 4) {
            if (killCount >= 30) {
                Time.timeScale = 0;
                winScreen.SetActive(true);
            }
        }
	}
}
