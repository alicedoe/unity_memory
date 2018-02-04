using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour {

    public static float highscore;
    public Text highscoreText;

    void start() {
        // highscore = PlayerPrefs.GetFloat ("highscore", highscore);
        
        // highscoreText.text = "Best time : "+highscore;
    }

    public void triggerMenuBehavior (int i) {
        switch(i) {
            default:
            case (0):
                SceneManager.LoadScene("Level");
                break;
            case (1):
                Application.Quit();
                break;
        }
    }

}
