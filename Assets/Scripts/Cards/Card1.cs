using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card1 : Card {

    public override void Use() {
        if (cost <= Turn.GetInstance().battery)
        {
            
            base.Use();
            if(noattack==false) 
            {
                DamagewithArmor(attackAfterCal, Battle.GetInstance().player2);
            }
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
            if (noattack == false) 
            {
                DamagewithArmor(attackAfterCal, Battle.GetInstance().player1);
            }
        }
        else
        {
            Debug.Log("Battery not enough");

        }
        
        
    }

}
