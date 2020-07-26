using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Player 
{
    public int id;
    public int hp;
    public int hp_max;
    public int armor;
    public int[] inibuffid = new int[50];
    public int maxbattery;
    public int cardback;
    public int drawnum;
    public int power;
    public int agility;

    public List<Buff> buffinbody = new List<Buff>();
    public List<Card> trapcards = new List<Card>();

    public Player() {
        
    }

    public Player ReadFromFile()
    {
        string jsondata = Resources.Load<TextAsset>("Data/Players/" + id.ToString()+"_"+Battle.GetInstance().stage).text;
        Player playerfromjson = new Player();
        playerfromjson = JsonUtility.FromJson<Player>(jsondata);
        return playerfromjson;
        
    }

    public Player GetOppoPlayer(){
        if (this == Battle.GetInstance().player1) return Battle.GetInstance().player2;
        else return Battle.GetInstance().player1;
    }

    public bool FindBuffById(int buffid){
        for (int i = 0; i < buffinbody.Count;i++){
            if(buffinbody[i].id == buffid){
                return true;
            }
        }
        return false;
    }

    public bool FindTrap(int id){
        for (int i = 0; i < trapcards.Count;i++){
            if(trapcards[i].id == id){
                return true;
            }
        }
        return false;
    }
    public Card CheckPlayerTrap(List<int> trapcards_possible){

        Card card = new Card();
        card.id = -1;
        for (int i = trapcards.Count - 1; i >= 0;i--){
            for (int j = 0; j < trapcards_possible.Count;j++){
                if(trapcards_possible[j] == trapcards[i].id){
                    card = trapcards[i];
                    trapcards.RemoveAt(i);
                    return card;
                }
            }
        }
        return card;
    }


}

public class PlayerManager : MonoBehaviour
{
    private void Start()
    {
        /*Player buddy = new Player();   应该根据npc数据初始化所有player
        buddy.hp = 50;
        buddy.maxbattery = 3;*/
    }
    private void Update()
    {

    }

}









