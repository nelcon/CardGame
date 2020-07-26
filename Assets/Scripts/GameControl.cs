using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {
    static Game game;
    public Deck deckme = new Deck();

    Game() {
        deckme.id = 0;
        deckme = deckme.ReadFromFile();

        //Debug.Log("deck ok");
    }
    public static Game GetInstance()
    {
        if (game == null)
        {
            game = new Game();
        }
        return game;
    }
    
}


public class GameControl : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

}