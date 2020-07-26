using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn 
{
    static Turn turn;
    public int turnid;
    public int battery;
    public int aibattery;
    public int maxbattery;
    public List<Card> HandcardsList = new List<Card>();
    public List<Card> AIHandcardList = new List<Card>();
    public int drawnumber;
    public int aioverflag;    //tell aiturn over
    public int aistartflag;
    public int can_endbutton;
    public int aipausedflag;


    public Turn()
    {
        turnid = 1;
        aioverflag = 0;
        aipausedflag = 0;
    }
    public static Turn GetInstance()
    {
        if (turn == null)
        {
            turn = new Turn();
        }
        return turn;
    }

    /****************************我的回合****************************************/

    public int StartTurn()
    {
        
        //初始化属性值
        drawnumber = Battle.GetInstance().player1.drawnum;
        maxbattery = Battle.GetInstance().player1.maxbattery;
        battery = maxbattery;
        aibattery = Battle.GetInstance().player2.maxbattery;

        aistartflag = 0;
        can_endbutton = 0;
        Battle.GetInstance().player1.armor = Battle.GetInstance().player1.armor / 2;
        GameObject.Find("BuffManager").GetComponent<BuffManager>().BuffUpdatePlayer();
        //抽卡
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            HandcardsList[i] = HandcardsList[i].Get(i, drawnumber);
        }
        UI.GetInstance().SetHandCardsBlooming();
        UI.GetInstance().SetBatteryGreen();
        UI.GetInstance().ChangePlayer1CardDescription();
        UI.GetInstance().SetTrapCardUnuse();
        GameObject.Find("TurnManager").GetComponent<TurnManager>().PlayStartTurnAnimation();

        //结束回合按钮变化
        GameObject.Find("Canvas/Button_endturn/Text_end").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Canvas/Button_endturn/Text_oppo").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Canvas/Button_endturn/Text_wait").transform.localScale = new Vector3(1, 1, 1);

        GameObject TurnStart = GameObject.Find("Canvas").transform.Find("Tx_TurnStart").gameObject;
        TurnStart.GetComponent<Text>().text = turnid/2+1 + "st TurnStart";
        TurnStart.SetActive(true);
        TurnStart.transform.GetComponent<Text>().CrossFadeAlpha(1, 0f, true);
        TurnStart.transform.GetComponent<Text>().CrossFadeAlpha(0, 3f, true);

        return 0;
    }

    public void EndTurn()
    {
        GameObject.Find("TurnManager").GetComponent<TurnManager>().PlayEndTurnAnimation();
        turnid++;
        aistartflag = 0;
        aioverflag = 1;
        can_endbutton = 0;

    }

    public IEnumerator EndTurnAnimation()
    {
        for (int i = HandcardsList.Count - 1; i >= 0; i--)
        {
            UI.GetInstance().DiscardHandCards(HandcardsList[i], HandcardsList.Count);
            yield return new WaitForSeconds(UI.GetInstance().DiscardDeltaTime);
        }

        DeleteAllHandCards();
    }

    public IEnumerator StartTurnAnimation()
    {

        for (int j = 0; j < HandcardsList.Count; j++)
        {
            UI.GetInstance().DealHandCard(j);
            yield return new WaitForSeconds(UI.GetInstance().DealDeltaTime);
        }
        yield return new WaitForSeconds(1.5f);
    }


    /****************************抽卡****************************************/

    public void Draw()
    {
        if (drawnumber > Battle.GetInstance().player1stack.Count)
        {
            int firstdraw = Battle.GetInstance().player1stack.Count;
            for (int i = 0; i < firstdraw; i++)
            {
                HandcardsList.Add(Battle.GetInstance().player1stack[0]);
                Battle.GetInstance().player1stack.Remove(Battle.GetInstance().player1stack[0]);
            }
            Battle.GetInstance().ReloadCheck();
            for (int i = 0; i < (drawnumber - firstdraw); i++)
            {
                HandcardsList.Add(Battle.GetInstance().player1stack[0]);
                Battle.GetInstance().player1stack.Remove(Battle.GetInstance().player1stack[0]);
            }
        }
        else
        {
            for (int i = 0; i < drawnumber; i++)
            {
                HandcardsList.Add(Battle.GetInstance().player1stack[0]);
                Battle.GetInstance().player1stack.Remove(Battle.GetInstance().player1stack[0]);
            }
        }


    }


    public void DisplayAIHand()
    {
        int backid = Battle.GetInstance().player2.cardback;
        int num = Battle.GetInstance().player2.drawnum;
        for (int i = 0; i < num; i++)
        {
            Card card = new Card();
            card.cardobject = Object.Instantiate(Resources.Load("Prefab/Cardback/" + backid.ToString()) as GameObject, new Vector3(0,0,0), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas/Handcard_oppo").transform);
            AIHandcardList.Add(card);
        }
        UI.GetInstance().InitialCardPosition(Battle.GetInstance().player2);
    }

    /****************************HandCardsList处理****************************************/

    public Card GetCard(GameObject gameObject)
    {
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            if (HandcardsList[i].cardobject == gameObject)
            {
                return HandcardsList[i];
            }
        }
        return null;
    }

    public Card GetCardById(int id){
        GameObject gameObject = Object.Instantiate(Resources.Load("Prefab/Cards/" + id.ToString()) as GameObject, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas").transform);
        Card card = gameObject.GetComponent<Card>();
        card.cardobject = gameObject;
        card.DestoryMyself();
        return card;
    }
    //使用卡牌后删除指定gameObject卡牌
    public void DeleteHandCard(GameObject gameObject)
    {
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            if (HandcardsList[i].cardobject == gameObject)
            {
                //消耗卡此处不需要
                Battle.GetInstance().player1cemetery.Add(HandcardsList[i]);
                HandcardsList[i].DestoryMyself();
                HandcardsList.RemoveAt(i);
                break;
            }
        }
    }

    //回合结束后删除所有卡牌
    public void DeleteAllHandCards()
    {
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            Battle.GetInstance().player1cemetery.Add(HandcardsList[i]);
            HandcardsList[i].DestoryMyself();
            HandcardsList.RemoveAt(i);
            i--;
        }
    }




    /****************************AI回合****************************************/

    void AITurnStart()
    {
        aibattery = Battle.GetInstance().player2.maxbattery;
        DisplayAIHand();
        GameObject.Find("BuffManager").GetComponent<BuffManager>().BuffUpdatePlayer();
    }


    public void AITurnOver()
    {
        for (int i = 0; i < AIHandcardList.Count; i++)
        {
            Object.Destroy(AIHandcardList[i].cardobject);
        }
        AIHandcardList.Clear();
        turnid++;
    }

    public IEnumerator AIPerform()
    {
        string[] instructions = new string[200];
        string[] lastinstructions = new string[200];

        if (Battle.GetInstance().battlescript[turnid] != null)
        {
            instructions = Battle.GetInstance().battlescript[turnid].Split(';');
        }


        if (turnid % 2 == 0 && Battle.GetInstance().battlescript[turnid] == "")
        {
            instructions = lastinstructions;
        }
        else {
            lastinstructions = instructions;
        }


        int fakedraw = 0;

        if (instructions[0] != null)
        {
            if (instructions[0].Contains("draw(1"))
            {
                fakedraw = 1;
            }
            else
            {
                fakedraw = 0;
            }
        }
        else
        {
            fakedraw = 0;
        }

        if (turnid % 2 == 0)
        {
            AITurnStart();
        }
        else
        { //我的回合开场动画
            if (fakedraw == 0)
            {
                Draw();
            }
            else
            {
                if (instructions[0].Length > 8)
                {
                    Battle.GetInstance().ai_me.AIDrawTrue(instructions[0].Substring(7, instructions[0].Length - 8));
                }
                
            }
            StartTurn();

        }
        LimeManager lime = GameObject.Find("LimeManager").GetComponent<LimeManager>();
        aistartflag = 1;

        int endflag = instructions.Length - 2;
        if (endflag < 0) { endflag = 0; }

        if (instructions[0] != null)
        {
            if (!instructions[endflag].StartsWith("end"))
            {                                //judge card if they can use
                for (int m = 0; m < HandcardsList.Count; m++)
                {
                    HandcardsList[m].canuse = 1;
                }
            }
            else
            {

                for (int m = 0; m < HandcardsList.Count; m++)
                {
                    HandcardsList[m].canuse = 0;
                }
            }
        }
        else {
            for (int m = 0; m < HandcardsList.Count; m++)
            {
                HandcardsList[m].canuse = 1;
            }
        }
        
        UI.GetInstance().SetHandCardsBlooming();

        //这里开始翻译data脚本文件
        int i = 0;
        if (instructions[0] != null)
        {
            while (instructions[i] != "")
            {
                CheckAIPause();
                if (instructions[i].StartsWith("[lock"))
                {
                    int lockid = int.Parse(instructions[i].Substring(5, 1));
                    if (Battle.GetInstance().scriptlock[lockid] == 0)
                    {
                        instructions[i] = "noyoucant";
                    }
                    else
                    {
                        instructions[i] = instructions[i].Remove(0, 7);
                    }
                }
                if (instructions[i].Contains("enlock("))
                {
                    int lockid = int.Parse(instructions[i].Substring(7, 1));
                    Battle.GetInstance().scriptlock[lockid] = 0;
                }
                if (instructions[i].Contains("unlock("))
                {
                    int lockid = int.Parse(instructions[i].Substring(7, 1));
                    Battle.GetInstance().scriptlock[lockid] = 1;
                }
                if (instructions[i].Contains("speak(1"))
                {

                    Battle.GetInstance().ai_me.AISpeak(1, instructions[i].Substring(8, instructions[i].Length - 9));
                    yield return new WaitForSeconds(2);
                    //GameObject.Find("Canvas/Chat_me").SetActive(false);

                }
                if (instructions[i].Contains("speak(2"))
                {
                    Battle.GetInstance().ai_oppo.AISpeak(2, instructions[i].Substring(8, instructions[i].Length - 9));
                    yield return new WaitForSeconds(2);
                    //GameObject.Find("Canvas/Chat_oppo").SetActive(false);
                }
                if (instructions[i].Contains("speak(3"))
                {
                    lime.Speak(instructions[i].Substring(8, instructions[i].Length - 9));
                    yield return new WaitForSeconds(1);
                }
                if (instructions[i].Contains("makechoice("))
                {

                    lime.MakeChoice(instructions[i].Substring(8, instructions[i].Length - 12));
                    aipausedflag = 1;
                    yield return new WaitForSeconds(3);
                    aipausedflag = 0;
                }
                if (instructions[i].Contains("wait"))
                {
                    yield return new WaitForSeconds(float.Parse(instructions[i].Substring(5, instructions[i].Length - 6)));

                }
                if (instructions[i].Contains("draw(2"))
                {
                    //Battle.GetInstance().ai_oppo.AIDrawFake(instructions[i].Substring(7, instructions[i].Length - 8));
                }
                if (instructions[i].Contains("think(1"))
                {
                    Battle.GetInstance().ai_me.AIThink();

                }
                if (instructions[i].Contains("think(2"))
                {
                    Battle.GetInstance().ai_oppo.AIThink();

                }
                if (instructions[i].Contains("playcard(1"))
                {
                    int aiusecardid = Battle.GetInstance().ai_me.MyAIPlayCard(int.Parse(instructions[i].Substring(11, instructions[i].Length - 12)));
                    yield return new WaitForSeconds(1.2f);
                    if (aiusecardid >= 0)
                    {
                        HandcardsList[aiusecardid].canuse = 1;
                        HandcardsList[aiusecardid].Use();
                    }
                    yield return new WaitForSeconds(1);
                }
                if (instructions[i].Contains("playcard(2"))
                {
                    Battle.GetInstance().ai_oppo.AIPlayCard(int.Parse(instructions[i].Substring(11, instructions[i].Length - 12)));
                    yield return new WaitForSeconds(3);
                }
                if (instructions[i].Contains("end"))
                {
                    if (turnid % 2 == 0)
                    {
                        yield return new WaitForSeconds(1.5f);
                        aioverflag = 1;
                        AITurnOver();
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);
                        aioverflag = 1;
                        EndTurn();
                    }

                }
                i++;
            }
        }
        if (i == 0) { i = 1; }
        if (instructions[0] != null)
        {
            if (!instructions[i - 1].Contains("end"))
            {

                can_endbutton = 1;
                GameObject.Find("Canvas/Button_endturn/Text_end").transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("Canvas/Button_endturn/Text_oppo").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Canvas/Button_endturn/Text_wait").transform.localScale = new Vector3(0, 0, 0);

            }
            else
            {
                if (turnid % 2 == 0)
                {
                    GameObject.Find("Canvas/Button_endturn/Text_end").transform.localScale = new Vector3(0, 0, 0);
                    GameObject.Find("Canvas/Button_endturn/Text_oppo").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("Canvas/Button_endturn/Text_wait").transform.localScale = new Vector3(0, 0, 0);
                }
                else
                {
                    GameObject.Find("Canvas/Button_endturn/Text_end").transform.localScale = new Vector3(0, 0, 0);
                    GameObject.Find("Canvas/Button_endturn/Text_oppo").transform.localScale = new Vector3(0, 0, 0);
                    GameObject.Find("Canvas/Button_endturn/Text_wait").transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        else
        {
            if (turnid % 2 == 1)
            {
                can_endbutton = 1;
                GameObject.Find("Canvas/Button_endturn/Text_end").transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("Canvas/Button_endturn/Text_oppo").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Canvas/Button_endturn/Text_wait").transform.localScale = new Vector3(0, 0, 0);
            }
            else {
                GameObject.Find("Canvas/Button_endturn/Text_end").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Canvas/Button_endturn/Text_oppo").transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("Canvas/Button_endturn/Text_wait").transform.localScale = new Vector3(0, 0, 0);

            }
        }
    }

    public void CheckAIPause()
    {
        while (aipausedflag == 1)
        {
            if (aipausedflag == 0)
            {
                break;
            }
        }

    }
}


public class TurnManager : MonoBehaviour
{
    public void Start()
    {

    }
    public void Update()
    {

    }

    public void ClickEndTurnButton()
    {

        if (Turn.GetInstance().can_endbutton == 1) //对手回合不能按
        {
            Turn.GetInstance().EndTurn();
        }
    }

    public void PlayEndTurnAnimation()
    {
        StartCoroutine(Turn.GetInstance().EndTurnAnimation());
    }
    public void PlayStartTurnAnimation()
    {
        StartCoroutine(Turn.GetInstance().StartTurnAnimation());
    }


}