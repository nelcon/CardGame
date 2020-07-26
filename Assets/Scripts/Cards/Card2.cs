using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card2 : Card {

    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {
            
            base.Use();
            Battle.GetInstance().player1.armor += defenseAfterCal;
            UI.GetInstance().ArmorDamageAnima(defenseAfterCal, Battle.GetInstance().player1);
            ChangeCardUIAfterUse();
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
            UI.GetInstance().ArmorDamageAnima(defenseAfterCal, Battle.GetInstance().player2);
        }
        else
        {
            Debug.Log("Battery not enough");

        }
    }
}
