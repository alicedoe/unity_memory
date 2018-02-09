using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Sprite[] cardFace;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text winText;
    public Text matchText;
    public Text timerText;
    public GameObject winBlock;
    public GameObject findMatch;
    public static float highscore;

    private bool _init = false;
    private int _matches = 0;
    private float startTime;
    private float finalTime;
    private float t;

    void Start() {
        highscore = PlayerPrefs.GetFloat ("highscore");
    }

	// Update is called once per frame
	void Update () {
        if (!_init) {
            winBlock.gameObject.SetActive(false);
            findMatch.gameObject.SetActive(false);
            initializeCards(); 
        }
        if (Input.GetMouseButtonUp(0))
            checkCards();
        
        t = Time.time - startTime;

        if (_matches == 13)
            {
                string time = timerText.text;
                timerText.text = "Finish";
            } else timerText.text = highscoreToString(t);
        
	}

    void initializeCards()
    {
        for(int id=0; id < 2; id++)
        {
            for(int i=1; i < 14; i++)
            {
                bool test = false;
                int choice = 0;
                while(!test)
                {
                    choice = Random.Range(0, cards.Length);
                    test = !(cards[choice].GetComponent<Card>().initialized);
                }
                cards[choice].GetComponent<Card>().cardValue = i;
                cards[choice].GetComponent<Card>().initialized = true;
            }
        }

        foreach (GameObject c in cards)
            c.GetComponent<Card>().setupGraphics();
        if (!_init) {
            startTime = Time.time;
            _init = true;
        }           

    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return cardFace[i-1];
    }

    void checkCards()
    {       
        List<int> c = new List<int>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<Card>().state == 1)
                c.Add(i);
        }
        if (c.Count == 2)
            cardComparaison(c);
    }

    void cardComparaison(List<int> c)
    {
        Card.DO_NOT = true;
        int x = 0;
        Debug.Log(string.Format("highscore = {0} - t = {1}", highscore,t));
        if(cards[c[0]].GetComponent<Card>().cardValue == cards[c[1]].GetComponent<Card>().cardValue)
        {
            x = 2;
            displayMessage( "matche",null );
            _matches++;
            matchText.text = "Number of matches: " + _matches;
            if (_matches == 13)
            {
                string time = timerText.text;
                displayMessage( "win", time );
            }
        }

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<Card>().state = x;
            
            cards[c[i]].GetComponent<Card>().falseCheck(x);
        }
    }   

    public void displayMessage( string message, string time )
    {
        StartCoroutine( winMessage( message, time ) );
    }

    IEnumerator winMessage( string message, string time )
    {
        if ( message == "win") {
            winBlock.gameObject.SetActive(true);
            
            if ( highscore > t || highscore == 0 ) {
                PlayerPrefs.SetFloat ("highscore", t);
                winText.text = "New reccord : "+time;
            } else {
                winText.text = "Your time : "+time;
            }

            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Menu");
        }

        if ( message == "matche") {
            findMatch.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            findMatch.gameObject.SetActive(false);
        }
        
    }

    public static string highscoreToString(float score) {
        string minutesStr;
        int minutes = ((int) score / 60);
        if ( minutes < 10 ) minutesStr = "0"+minutes.ToString();
        else minutesStr = minutes.ToString();

        string secondesStr;
        float secondes = score % 60;
        if ( secondes < 10 ) secondesStr = "0"+secondes.ToString("f0");
        else secondesStr = secondes.ToString("f0");
        
        string scoreString = minutesStr+":"+secondesStr;
        return scoreString;
    }
}