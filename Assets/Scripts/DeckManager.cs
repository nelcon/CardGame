using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Deck 
{
    public int id;
    public int amount;
    public List<int> cardid = new List<int>();



    public Deck()
    {

    }

    public int Delete()
    {

        return 0;
    }

    public int Add()
    {

        return 0;
    }

    public Deck ReadFromFile() {
        string jsondata = Resources.Load<TextAsset>("Data/Decks/" + id.ToString()).text;
        Deck deckfromjson = new Deck();
        deckfromjson = JsonUtility.FromJson<Deck>(jsondata);
        return deckfromjson;
    }

}

public class DeckManager : MonoBehaviour
{
    private void Start()
    {

    }
    private void Update()
    {

    }

}
