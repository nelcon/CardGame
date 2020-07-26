using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Buff{
    public int id;
    public string buffname;
    public int type;
    public int reactturn;
    public int lastturn;
    public int existturn;

    public GameObject BuffObj;

    public Buff ReadFromFile()
    {
        string jsondata = Resources.Load<TextAsset>("Data/Buffs/" + id.ToString()).text;
        Buff bufffromjson = new Buff();
        bufffromjson = JsonUtility.FromJson<Buff>(jsondata);
        return bufffromjson;
    }

}


public class BuffManager : MonoBehaviour {

    List<Buff> totalbuffs = new List<Buff>();


    void LoadAllBuffs() {
        int i = 0;
        for (i=0;i< Resources.LoadAll("Data/Buffs").Length;i++) {
            Buff thisbuff = new Buff();
            thisbuff.id = i;
            thisbuff = thisbuff.ReadFromFile();
            totalbuffs.Add(thisbuff);
        }

    }

    

    public void AddBuff(Player player,int buffid) { 
        player.buffinbody.Add(totalbuffs[buffid]);
        GameObject.Find("UIManager").GetComponent<UIManager>().AddBuffUI(player, buffid);
        BuffEffect(player,totalbuffs[buffid]);
    }
    private void Awake()
    {
        LoadAllBuffs();
    }

    public void BuffUpdatePlayer(){
        BuffUpdateOnePlayer(Battle.GetInstance().player1);
        BuffUpdateOnePlayer(Battle.GetInstance().player2);
    }

    public void BuffUpdateOnePlayer(Player player) {
        for (int i = 0; i < player.buffinbody.Count; i++) {
            player.buffinbody[i].existturn ++;
            if (player.buffinbody[i].id == 5){
                if (player.power == 0)
                {
                    BuffClear(player, player.buffinbody[i]);
                    i--;
                }
                else
                {
                    BuffEffect(player, player.buffinbody[i]);
                }
            }else{
                if (player.buffinbody[i].existturn == player.buffinbody[i].lastturn)
                {
                    BuffClear(player, player.buffinbody[i]);
                    i--;
                }
                else
                {
                    BuffEffect(player, player.buffinbody[i]);
                }
            }

        }
    }


    public void BuffEffect(Player player, Buff thisbuff)
    {
            switch (thisbuff.id)
        {
                case 0:
                if(player == Battle.GetInstance().player1){
                    for (int i = 0; i < Turn.GetInstance().HandcardsList.Count; i++)
                    {
                        if (Turn.GetInstance().HandcardsList[i].type == 1)
                        {
                            Turn.GetInstance().HandcardsList[i].canuse = 0;
                        }
                    } 
                }

                break;
                
                case 3:
                if( thisbuff.existturn%2==0 ){
                    player.power += 2;
                }
                    break;
                case 4:
                if(thisbuff.existturn == 2){
                    Card card = Turn.GetInstance().GetCardById(2);
                    card.defense = 8;
                    card.Use();
                }
                    break;
                case 5:
                if (thisbuff.existturn == 2 && thisbuff.existturn %2==0)
                {
                    player.power--;
                }
                break;
            default:
                break;

        }



    }

    public void BuffClear(Player player,Buff thisbuff) {
        
            switch (thisbuff.id)
            {
                case 0:
                if(player == Battle.GetInstance().player1){
                    for (int i = 0; i < Turn.GetInstance().HandcardsList.Count; i++)
                    {
                        if (Turn.GetInstance().HandcardsList[i].type == 1)
                        {
                            Turn.GetInstance().HandcardsList[i].canuse = 1;
                        }
                    }
                }
  
                    break;
                default:
                    break;

            }

        
        player.buffinbody.Remove(thisbuff);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelBuffUI(player, thisbuff.id);

    }

    public void OnMouseEnterBuff()
    {
        this.gameObject.transform.Find("description").gameObject.SetActive(true);
    }
    public void OnMouseExitBuff()
    {
        this.gameObject.transform.Find("description").gameObject.SetActive(false);
    }

}
