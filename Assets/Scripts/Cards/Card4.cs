using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card4 : Card
{

    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {
            
            base.Use();
            Battle.GetInstance().player1.hp -= attack;
            Battle.GetInstance().player1.armor += defenseAfterCal;
            UI.GetInstance().blood_damage_anima(Battle.GetInstance().player1);
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
            Battle.GetInstance().player2.hp -= attack;
            Battle.GetInstance().player2.armor += defenseAfterCal;
            UI.GetInstance().blood_damage_anima(Battle.GetInstance().player2);
            UI.GetInstance().ArmorDamageAnima(defenseAfterCal, Battle.GetInstance().player2);
        }
        else
        {
            Debug.Log("Battery not enough");

        }
    }
}
