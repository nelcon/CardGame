using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card9 : Card
{
    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {

            base.Use();
            Battle.GetInstance().player1.armor += defenseAfterCal;
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(Battle.GetInstance().player1, 4);
            ChangeCardUIAfterUse();
            UI.GetInstance().ArmorDamageAnima(defenseAfterCal, Battle.GetInstance().player1);
        }
        else
        {
            Debug.Log("Battery not enough");
        }
    }

    public override void AIUse()
    {
        if (cost <= Turn.GetInstance().aibattery)
        {

            base.AIUse();
            Battle.GetInstance().player2.armor += defenseAfterCal;
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(Battle.GetInstance().player2, 4);
            UI.GetInstance().ArmorDamageAnima(defenseAfterCal, Battle.GetInstance().player2);
        }
        else
        {
            Debug.Log("Battery not enough");

        }
    }

}
