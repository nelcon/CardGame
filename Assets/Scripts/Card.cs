using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    public string cardname;
    public int cost;
    public int type;
    public string des;
    public string trapdes;
    public int canuse;

    public int attack;
    public int defense;

    public int attackAfterCal;
    public int defenseAfterCal;

    public int begindrag;
    public bool noattack;


    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;
    public int SiblingIndex;

    public GameObject cardobject;
    public GameObject cardbackobject;
    public GameObject trapobject;
    private readonly object TurnStart;

    public Card Get(int i, int drawnumber)
    {
        Card GetCard = new Card();
        GameObject HandCard = Instantiate(Resources.Load("Prefab/Cards/" + id.ToString()) as GameObject,
                                          new Vector3(150 * (i - drawnumber / 2) + 10, -465, 0),
                                          Quaternion.Euler(0, 0, (i - drawnumber / 2) * -5),
                                          GameObject.Find("Canvas/Handcard_me").transform);
        HandCard.SetActive(false);
        GetCard = HandCard.GetComponent<Card>();
        HandCard.transform.Find("Text_card").GetComponent<Text>().text = GetCard.des;
        HandCard.transform.Find("Text_Title").GetComponent<Text>().text = GetCard.cardname;
        HandCard.transform.Find("Text_cost").GetComponent<Text>().text = GetCard.cost.ToString();
        GetCard.cardobject = HandCard;
        return GetCard;
    }


    public void DestoryMyself()
    {
        Destroy(cardobject);
    }

    public void InitCard(){
        noattack = false;
        attackAfterCal = attack;
        defenseAfterCal = defense;
    }
    public virtual void Use()
    {
        InitCard();
        TrapCheck(Battle.GetInstance().player2);
        PlayerPropertyCheck(Battle.GetInstance().player1);
        BuffCheck(Battle.GetInstance().player1);
        Turn.GetInstance().battery -= cost;
    }

    public virtual void AIUse()
    {
        InitCard(); 
        TrapCheck(Battle.GetInstance().player1);
        PlayerPropertyCheck(Battle.GetInstance().player2);
        BuffCheck(Battle.GetInstance().player2);

    }

    public virtual void TalkAfterUse(int playerid,string content) {
        Battle.GetInstance().ai_me.AISpeak(playerid, content);
    }


    public virtual void UseTrap(Card attackcard)
    {

    }
    public virtual void AIUseTrap(Card attackcard)
    {

    }

    public void ExhaustUse()
    {

    }

    public void TrapCheck(Player player)
    {

        List<int> trapcards_possible = new List<int>();
        if (type == 1)
        {
            trapcards_possible.Add(8);
            trapcards_possible.Add(10);
        }
        Card trapcard = player.CheckPlayerTrap(trapcards_possible);
        noattack = false;
        if(trapcard.id !=-1){
            if (player == Battle.GetInstance().player1) trapcard.UseTrap(this);
            else trapcard.AIUseTrap(this); 
        }
    }
    public void PlayerPropertyCheck(Player player)
    {
        attackAfterCal += player.power;
        defenseAfterCal += player.agility;
    }
    public void BuffCheck(Player player)
    {
        for (int i = 0; i < player.buffinbody.Count; i++)
        {
            if (player.buffinbody[i].id == 1 && type == 1)
            {
                attackAfterCal *= 2;
            }
        }
        Player oppoplayer = Battle.GetInstance().player1;
        if (player == Battle.GetInstance().player1) oppoplayer = Battle.GetInstance().player2;
        for (int i = 0; i < oppoplayer.buffinbody.Count; i++)
        {
            if (oppoplayer.buffinbody[i].id == 2 && type == 1)
            {
                attackAfterCal = (int)(1.25 * attackAfterCal);
            }
        }
    }

    public void SetOtherTrapCardsUse(int id, int state)
    {
        for (int i = 0; i < Turn.GetInstance().HandcardsList.Count; i++)
        {
            if (Turn.GetInstance().HandcardsList[i].id == id)
            {
                Turn.GetInstance().HandcardsList[i].canuse = state;
            }
        }
    }
    public void ChangeCardUIAfterUse()
    {

        Turn.GetInstance().DeleteHandCard(this.gameObject);
        UI.GetInstance().MoveCardPosition(Battle.GetInstance().player1);
        UI.GetInstance().SetHandCardsBlooming();
        UI.GetInstance().SetBatteryGreen();
    }


    public void DragBegin()
    {
        begindrag = 1;
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnMouseDrag()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<RectTransform>().pivot.Set(0, 0);
        Vector3 offsetz = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        transform.position = offsetz;
    }

    public void DragOver()
    {
        Card card = Turn.GetInstance().GetCard(this.gameObject);
        canuse = card.canuse;
        if (this.transform.position.y > -100 && cost <= Turn.GetInstance().battery && canuse == 1)
        {
            Use();
            UI.GetInstance().ChangePlayer1CardDescription();
        }
        else
        {
            UI.GetInstance().MoveCardPosition(Battle.GetInstance().player1);
        }
        begindrag = 0;
    }


    public void SaveCardPos()
    {
        position = cardobject.transform.position;
        scale = cardobject.transform.localScale;
        rotation = cardobject.transform.rotation;
        SiblingIndex = cardobject.transform.GetSiblingIndex();
    }

    public void OnMouseEnter()
    {
        if (begindrag == 0)
        {
            UI.GetInstance().InitialCardPosition(Battle.GetInstance().player1);
            UI.GetInstance().ZoomOutHandCard(this.gameObject);
        }


    }
    public void OnMouseExit()
    {
        if (begindrag == 0)
        {
            UI.GetInstance().ZoomInHandCard(this.gameObject);
        }

    }
    private void Update()
    {

    }

    public void DamagewithArmor(int damage, Player player)
    {
        
        Player OppoPlayer = player.GetOppoPlayer();
        if (player.armor <= 0)
        {
            player.hp -= damage;
        }
        else
        {
            
            if (player.armor >= damage)
            {
                player.armor -= damage;
                UI.GetInstance().ArmorDamageAnima(-damage, player);
                if(OppoPlayer.FindBuffById(6)){
                    OppoPlayer.armor += damage;
                    UI.GetInstance().ArmorDamageAnima(damage, OppoPlayer);
                }
            }
            else
            {
                if (OppoPlayer.FindBuffById(6))
                {
                    OppoPlayer.armor += player.armor;
                    UI.GetInstance().ArmorDamageAnima(player.armor, OppoPlayer);
                }

                damage -= player.armor;
                player.armor = 0;
                player.hp -= damage;
                UI.GetInstance().ArmorDamageAnima(-player.armor, player);

            }
        }

        UI.GetInstance().blood_damage_anima(player);

    }

}
