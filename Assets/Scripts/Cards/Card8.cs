using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card8 : Card
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

    public override void UseTrap(Card attackcard)
    {
        DamagewithArmor(attack, Battle.GetInstance().player2);
        GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(Battle.GetInstance().player2, 2);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelTrap(Battle.GetInstance().player1,id);
        GameObject.Find("UIManager").GetComponent<UIManager>().PlayTrapAnimation(this);
        attackcard.noattack = true;
        UI.GetInstance().SetTrapCardUnuse();
    }

    public override void AIUseTrap(Card attackcard)
    {
        DamagewithArmor(attack, Battle.GetInstance().player1);
        GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(Battle.GetInstance().player1, 2);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelTrap(Battle.GetInstance().player2,id);
        GameObject.Find("UIManager").GetComponent<UIManager>().PlayTrapAnimation(this);
        attackcard.noattack = true;
    }
    public override void AIUse()
    {
        if (cost <= Turn.GetInstance().aibattery)
        {
            base.AIUse();
            Battle.GetInstance().player2.trapcards.Add(this);
            GameObject.Find("UIManager").GetComponent<UIManager>().AddTrap(Battle.GetInstance().player2,id);
        }
        else
        {
            Debug.Log("Battery not enough");

        }


    }

}
