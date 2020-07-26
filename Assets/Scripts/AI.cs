using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour {
    int id;
    public static GameObject AI1Chat;
    public static GameObject AI2Chat;
    //public GameObject[] AICards = new GameObject[100];

 
    public void AITurnStart() {
        Turn.GetInstance().turnid++;

    }



    public void AISpeak(int who,string text) {
        if (who == 1)
        {
            
            AI1Chat.SetActive(true);
            AI1Chat.GetComponent<DestroyAfterUse>().Start();
            AI1Chat.GetComponent<Text>().text = text;

        }
        else {

            AI2Chat.SetActive(true);
            AI2Chat.GetComponent<DestroyAfterUse>().Start();
            AI2Chat.GetComponent<Text>().text = text;

        }
        


    }

    public void AIDrawTrue(string cards) {  
        string[] cardsstr = new string[10];
        cardsstr = cards.Split(',');
        int i = 0;
        for(i=0;i<cardsstr.Length;i++) {
            Card card = new Card();
            card.id = int.Parse(cardsstr[i]);
            Turn.GetInstance().HandcardsList.Add(card);
            Battle.GetInstance().player1stack.Remove(card);
        }
        Debug.Log("True Draw"+cards);
    }
    public void AIDrawFake(string cards)    //不完全的
    {
        //int count;
        //count = cards.Split(',').Length;
        //Debug.Log("Fake draw"+count.ToString());
        //Turn.GetInstance().aihandcardnum += count;
    }
    public void AIThink(){
        Debug.Log("Thinking");
    }
    public void AIPlayCard(int cardid) {
        GameObject.Find("UIManager").GetComponent<UIManager>().PlayAIPlayCardAnimation(cardid);
    }

    public int MyAIPlayCard(int cardid) {
        int findflag = 0;
        int i = 0;
        for (i=0; i < Turn.GetInstance().HandcardsList.Count; i++) {
            if (Turn.GetInstance().HandcardsList[i].id == cardid) {
                findflag = 1;
                break;
            }
        }
        if (findflag == 1)
        {
            iTween.MoveTo(Turn.GetInstance().HandcardsList[i].cardobject, GameObject.Find("Canvas/Card_use").transform.position, 0.8f);
            iTween.RotateTo(Turn.GetInstance().HandcardsList[i].cardobject,new Vector3(0,0,0),0.3f);
            return i;
        }
        else {
            Debug.Log("not find this card");
            return -1;
        }
    }

    private void Awake()
    {
        AI1Chat = GameObject.Find("Canvas/Chat_me");
        AI2Chat = GameObject.Find("Canvas/Chat_oppo");
        GameObject.Find("Canvas/Chat_me").SetActive(false);
        GameObject.Find("Canvas/Chat_oppo").SetActive(false);

    }
}


