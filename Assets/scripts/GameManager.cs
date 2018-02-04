using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Sprite[] cardFace;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text matchText;
    public GameObject winBlock;
    public GameObject findMatch;

    private bool _init = false;
    private int _matches = 0;
	
	// Update is called once per frame
	void Update () {
        if (!_init) {
            winBlock.gameObject.SetActive(false);
            findMatch.gameObject.SetActive(false);
            initializeCards(); 
        }
        if (Input.GetMouseButtonUp(0))
            checkCards();
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
        if (!_init)
            _init = true;

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
        if(cards[c[0]].GetComponent<Card>().cardValue == cards[c[1]].GetComponent<Card>().cardValue)
        {
            x = 2;
            displayMessage( "matche" );
            _matches++;
            matchText.text = "Number of matches: " + _matches;
            if (_matches == 13)
            {
                displayMessage( "win" );
            }
        }

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<Card>().state = x;
            
            cards[c[i]].GetComponent<Card>().falseCheck(x);
        }
    }   

    public void displayMessage( string message )
    {
        StartCoroutine( winMessage( message ) );
    }

    IEnumerator winMessage( string message )
    {
        if ( message == "win") {
            winBlock.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Menu");
        }

        if ( message == "matche") {
            findMatch.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            findMatch.gameObject.SetActive(false);
        }
        
    }
}