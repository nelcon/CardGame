using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card11 : Card
{

    public override void Use()
    {
        if (cost <= Turn.GetInstance().battery)
        {

            base.Use();
            Player player = Battle.GetInstance().player1;
            player.power += 1;
            for (int i = 0; i < player.buffinbody.Count;i++){
                if(player.buffinbody[i].type == 3 ){
                    GameObject.Find("BuffManager").GetComponent<BuffManager>().BuffClear(player,player.buffinbody[i]);
                    i--;
                }
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
            Player player = Battle.GetInstance().player2;
            player.power += 1;
            for (int i = 0; i < player.buffinbody.Count; i++)
            {
                if (player.buffinbody[i].type == 3)
                {
                    GameObject.Find("BuffManager").GetComponent<BuffManager>().BuffClear(player, player.buffinbody[i]);
                    i--;
                }
            }
        }
        else
        {
            Debug.Log("Battery not enough");

        }


    }

}
