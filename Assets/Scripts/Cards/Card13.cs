using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card13 : Card
{

    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {

            base.Use();
            Player player = Battle.GetInstance().player1;
            player.power += 1;
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(player, 5);
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
            Player player = Battle.GetInstance().player2;
            player.power += 10;
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(player, 5);
        }
        else
        {
            Debug.Log("Battery not enough");

        }


    }

}
