using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{

    public static float version = 0.1f;
    public static int highscore;
    public Text highscoreText;
    public static int deck;

    void Start()
    {

        LocalizationManager loc = LocalizationManager.instance;

        if (!PlayerPrefs.HasKey("version") || PlayerPrefs.GetFloat("version") < version)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat("version", version);
        }

        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
            highscoreText.text = loc.GetLocalizedValue("Reccord") + " : " + highscore + " " + loc.GetLocalizedValue("Points");
        }
        else
        {
            highscore = 0;
            highscoreText.text = loc.GetLocalizedValue("no_highscore");
        }

    }

    public void triggerMenuBehavior(int i)
    {
        switch (i)
        {
            default:
            case (0):
                deck = 1;
                SceneManager.LoadScene("Level");
                break;
            case (1):
                deck = 2;
                SceneManager.LoadScene("Level");
                break;
            case (2):
                Application.Quit();
                break;
        }
    }

}