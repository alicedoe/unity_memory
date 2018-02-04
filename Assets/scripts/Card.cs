using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public static bool DO_NOT = false;

    [SerializeField]
    private int _state;
    [SerializeField]
    private int _cardValue;
    [SerializeField]
    private bool _initialized = false;

    private Sprite _cardBack;
    private Sprite _cardFace;

    private GameObject _mananager;

    private void Start()
    {
        _mananager = GameObject.FindGameObjectWithTag("Manager");
        _state = 1;
    }

    public void setupGraphics()
    {
        _cardBack = _mananager.GetComponent<GameManager>().getCardBack();
        _cardFace = _mananager.GetComponent<GameManager>().getCardFace(_cardValue);
        flipCard();
    }

    public void flipCard()
    {
        if (_state == 1 && !DO_NOT) _state = 0;
        else if (_state == 0 && !DO_NOT) _state = 1;
        if (_state == 0 && !DO_NOT)
        {
            GetComponent<Image>().sprite = _cardBack;
        }
        else if (_state == 1 && !DO_NOT)
        {
            GetComponent<Image>().sprite = _cardFace;
        }
    }

    public int cardValue
    {
        get { return _cardValue; }
        set { _cardValue = value; }
    }

    public int state
    {
        get { return _state; }
        set { _state = value; }
    }

    public bool initialized
    {
        get { return _initialized; }
        set { _initialized = value; }
    }

    public void falseCheck(int x)
    {
        StartCoroutine(pause(x));
    }

    IEnumerator pause(int x)
    {
        if ( x == 2) yield return new WaitForSeconds(0);
        else yield return new WaitForSeconds(1);
        if (_state == 0)
        {
            GetComponent<Image>().sprite = _cardBack;
        } else if (_state == 1)
        {
            GetComponent<Image>().sprite = _cardFace;
        }
        DO_NOT = false;
    }

}
