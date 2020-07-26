using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card10 : Card
{

    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {
            
            base.Use();
            Battle.GetInstance().player1.trapcards.Add(this);
            GameObject.Find("UIManager").GetComponent<UIManager>().AddTrap(Battle.GetInstance().player1,id);
            UI.GetInstance().SetTrapCardUnuse();
            ChangeCardUIAfterUse();
        }
        else
        {
            Debug.Log("Battery not enough");

        }
    }

    public override void UseTrap(Card attackcard){
        Battle.GetInstance().player1.power += 1;
        GameObject.Find("UIManager").GetComponent<UIManager>().DelTrap(Battle.GetInstance().player1,id);
        GameObject.Find("UIManager").GetComponent<UIManager>().PlayTrapAnimation(this);
        attackcard.noattack = true;
        UI.GetInstance().SetTrapCardUnuse();
    }

    public override void AIUseTrap(Card attackcard){
        Battle.GetInstance().player2.power += 1;
        attackcard.noattack = true;
        TalkAfterUse(2, "");
        GameObject.Find("UIManager").GetComponent<UIManager>().DelTrap(Battle.GetInstance().player2,id);
        GameObject.Find("UIManager").GetComponent<UIManager>().PlayTrapAnimation(this);
    }
    public override void AIUse()
    {
        if (cost <= Turn.GetInstance().aibattery)
        {
            base.AIUse();
            Battle.GetInstance().player2.trapcards.Add(this);
            GameObject.Find("UIManager").GetComponent<UIManager>().AddTrap(Battle.GetInstance().player2, id);
        }
        else
        {
            Debug.Log("Battery not enough");

        }
        

    }

    public override void TalkAfterUse(int playerid, string content)
    {
        content = "太慢了！";
        base.TalkAfterUse(playerid, content);
    }

}
