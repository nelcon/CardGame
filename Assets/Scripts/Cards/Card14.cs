using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card14 : Card
{
    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {

            base.Use();
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(Battle.GetInstance().player1, 6);
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
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(Battle.GetInstance().player2, 6);
        }
        else
        {
            Debug.Log("Battery not enough");

        }
    }

}
