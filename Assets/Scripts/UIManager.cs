using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI
{
    static UI ui;

    public List<GameObject> batteryobjs = new List<GameObject>();
    public List<Card> trapobjs = new List<Card>();
    public List<Card> AItrapobjs = new List<Card>();
    public GameObject trapcarddes;
    public List<Buff> Buffobjs = new List<Buff>();
    public List<Buff> AIBuffobjs = new List<Buff>();
    public Image bloodyellow ;
    /*
     *   常量定义
    */
    public  float r = 150;                        //椭圆短轴
    public  float R = 500;                        //椭圆长轴
    public  float centerx = 0;                    //椭圆中心x
    public float centery = -650f;                //椭圆中心y
    public  float AIcentery = 750f;                //AI椭圆中心y
    public  float ZoomOutScale = 1.5f;            //放大倍数
    public  float RotationAngle = -5f;            //旋转角
    public  float DeltaAngle = 20;                //两张相邻的卡牌间的角度差
    public  float ChangeRank = 0.5f;              //排斥后两遍卡牌的偏移rank
    public  float ChangeNextRank = 0.2f;          //排斥后间隔一张卡牌的偏移rank
    public  float MoveTime = 0.5f;                //移动时间
    public  float DealTotalTime = 3f;             //总发牌时间=开场动画时间
    public  float DealDeltaTime = 0.3f;           //相邻两张牌发牌间隔时间     （n-1)*DealDeltaTime + DealSingleTime = DealTotalTime
    public  float DiscardDeltaTime = 0.2f;

    public  float ZoomOutTime = 0f;                //放大时间
    public  float ZoomOutMoveOtherTime = 1f;
    public  float ZoomInTime = 1f;

    public int StartBloodDamage;

    public static UI GetInstance()
    {
        if (ui == null)
        {
            ui = new UI();
        }
        return ui;

    }
    /**************************** Battery ****************************************/
    //调整电池位置
    public void CalBatteryPos()
    {
        int dy = -660;
        int dx = 0;
        int dbattery = 30;
        int maxbattery = Battle.GetInstance().player1.maxbattery;
        int battery = Turn.GetInstance().battery;
        int amount = maxbattery;
        if (battery > amount) amount = battery;
        float midamount = amount / 2;
        if (amount % 2 == 1) midamount += 0.5f;
        GameObject batteryicon = GameObject.Find("Canvas/Battery_frame/battery");
        float firstrank = 0 - midamount;
        batteryicon.transform.position = new Vector3(dx + firstrank * dbattery, dy, 0);
        for (int i = 0; i < batteryobjs.Count; i++)
        {
            float rank = (i + 1) - midamount;
            //Debug.Log(rank);
            batteryobjs[i].transform.position = new Vector3(dx + rank * dbattery, dy, 0);

        }
    }

    //控制电池显示(控制maxbettery以内,超出的调用add/delbattery控制)
    public void SetBatteryGreen()
    {
        int maxbattery = Battle.GetInstance().player1.maxbattery;
        int battery = Turn.GetInstance().battery;

        for (int i = 0; i < maxbattery; i++)
        {
            GameObject greenlight = batteryobjs[i].transform.Find("greenlight").gameObject;
            if (i < battery) greenlight.SetActive(true);
            else greenlight.SetActive(false);
        }
    }

    /**************************** Buff ****************************************/
    //调整Buff位置
    public void AdjustBuffPos()
    {
        int dbuff = 50;
        int dx = 180;
        int dy = -255;
        for (int i = 0; i < Buffobjs.Count; i++)
        {
            Buffobjs[i].BuffObj.transform.position = new Vector3(dx + i * dbuff, dy, 0);
        }
        for (int i = 0; i < AIBuffobjs.Count; i++)
        {
            AIBuffobjs[i].BuffObj.transform.position = new Vector3(dx + i * dbuff, dy + 610, 0);
        }

    }
    /**************************** Trap ****************************************/
    public void AdjusttrapPos(){
        int dy = 0;



        if (trapobjs.Count == 1)
        {
            trapobjs[0].trapobject.transform.position = new Vector3(0, -100 + dy, 0);
        }
        else if (trapobjs.Count == 2)
        {
            trapobjs[0].trapobject.transform.position = new Vector3(-60, -115 + dy, 0);
            trapobjs[1].trapobject.transform.position = new Vector3(60, -115 + dy, 0);
        }
        else if (trapobjs.Count == 3)
        {
            trapobjs[1].trapobject.transform.position = new Vector3(0, -100 + dy, 0);
            trapobjs[0].trapobject.transform.position = new Vector3(-80, -130 + dy, 0);
            trapobjs[2].trapobject.transform.position = new Vector3(80, -130 + dy, 0);
        }
        else if (trapobjs.Count == 4)
        {
            trapobjs[0].trapobject.transform.position = new Vector3(-90, -160 + dy, 0);
            trapobjs[1].trapobject.transform.position = new Vector3(-45, -100 + dy, 0);
            trapobjs[2].trapobject.transform.position = new Vector3(45, -100 + dy, 0);
            trapobjs[3].trapobject.transform.position = new Vector3(90, -160 + dy, 0);
        }
        else if (trapobjs.Count == 5)
        {
            trapobjs[0].trapobject.transform.position = new Vector3(-100, -180 + dy, 0);
            trapobjs[1].trapobject.transform.position = new Vector3(-65, -125 + dy, 0);
            trapobjs[2].trapobject.transform.position = new Vector3(0, -100 + dy, 0);
            trapobjs[3].trapobject.transform.position = new Vector3(65, -125 + dy, 0);
            trapobjs[4].trapobject.transform.position = new Vector3(100, -180 + dy, 0);
        }

        dy += 610;

        if (AItrapobjs.Count == 1)
        {
            AItrapobjs[0].trapobject.transform.position = new Vector3(0, -100 + dy, 0);
        }
        else if (AItrapobjs.Count == 2)
        {
            AItrapobjs[0].trapobject.transform.position = new Vector3(-60, -115 + dy, 0);
            AItrapobjs[1].trapobject.transform.position = new Vector3(60, -115 + dy, 0);
        }
        else if (AItrapobjs.Count == 3)
        {
            AItrapobjs[1].trapobject.transform.position = new Vector3(0, -100 + dy, 0);
            AItrapobjs[0].trapobject.transform.position = new Vector3(-80, -130 + dy, 0);
            AItrapobjs[2].trapobject.transform.position = new Vector3(80, -130 + dy, 0);
        }
        else if (AItrapobjs.Count == 4)
        {
            AItrapobjs[0].trapobject.transform.position = new Vector3(-90, -160 + dy, 0);
            AItrapobjs[1].trapobject.transform.position = new Vector3(-45, -100 + dy, 0);
            AItrapobjs[2].trapobject.transform.position = new Vector3(45, -100 + dy, 0);
            AItrapobjs[3].trapobject.transform.position = new Vector3(90, -160 + dy, 0);
        }
        else if (AItrapobjs.Count == 5)
        {
            AItrapobjs[0].trapobject.transform.position = new Vector3(-100, -180 + dy, 0);
            AItrapobjs[1].trapobject.transform.position = new Vector3(-65, -125 + dy, 0);
            AItrapobjs[2].trapobject.transform.position = new Vector3(0, -100 + dy, 0);
            AItrapobjs[3].trapobject.transform.position = new Vector3(65, -125 + dy, 0);
            AItrapobjs[4].trapobject.transform.position = new Vector3(100, -180 + dy, 0);
        }

    }
    /**************************** HandCards Blooming ****************************************/
    //卡牌闪光判定
    public void SetHandCardsBlooming()
    {
        List<Card> HandcardsList = Turn.GetInstance().HandcardsList;
        int battery = Turn.GetInstance().battery;
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            GameObject bloom_use = HandcardsList[i].cardobject.transform.Find("blooming_use").gameObject;
            GameObject bloom_useless = HandcardsList[i].cardobject.transform.Find("blooming_useless").gameObject;
            if (HandcardsList[i].cost <= battery && HandcardsList[i].canuse == 1)
            {
                bloom_use.SetActive(true);
                bloom_useless.SetActive(false);
            }
            else
            {
                bloom_use.SetActive(false);
                bloom_useless.SetActive(true);
            }
        }
    }

    public void blood_damage_anima(Player player)
    {
        GameObject blood = null;
        if (player == Battle.GetInstance().player1)
        {
            blood = GameObject.Find("Canvas/Hp_me/Image");
            StartBloodDamage = 1;
        }
        else
        {
            blood = GameObject.Find("Canvas/Hp_oppo/Image");
            StartBloodDamage = 2;
        }
        blood.GetComponent<Image>().fillAmount = player.hp/(float)player.hp_max;

    }


    public void ArmorDamageAnima(int damage, Player player)
    {
        GameObject DamageInfo = GameObject.Find("Canvas").transform.Find("DamageInfo").gameObject;
        Vector3 startpos = new Vector3(0, 0, 0);
        Vector3 endpos = new Vector3(0, 0, 0);


        if (damage > 0)
        {
            DamageInfo.GetComponent<Text>().color = Color.green;
            DamageInfo.GetComponent<Text>().text = "+" + damage;
        }
        else
        {
            DamageInfo.GetComponent<Text>().color = Color.red;
            DamageInfo.GetComponent<Text>().text = damage.ToString();
        }

        if (player == Battle.GetInstance().player2) startpos = new Vector3(137, 315, 0);
        if (player == Battle.GetInstance().player1) startpos = new Vector3(143, -296, 0);
        endpos = new Vector3(startpos.x, startpos.y + 140, startpos.z);


        DamageInfo.SetActive(true);
        DamageInfo.transform.GetComponent<Text>().CrossFadeAlpha(1, 0f, true);
        DamageInfo.transform.position = startpos;
        iTween.MoveTo(DamageInfo, endpos, 2f);
        DamageInfo.transform.GetComponent<Text>().CrossFadeAlpha(0, 2f, true);

    }
    /**************************** Handcards Move ****************************************/
    public void InitialCardPosition(Player player)
    {
        List<Card> HandcardsList = Turn.GetInstance().HandcardsList;
        if(player == Battle.GetInstance().player2) HandcardsList = Turn.GetInstance().AIHandcardList;
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            float rank = 0;
            if (HandcardsList.Count % 2 == 1) rank = i - HandcardsList.Count / 2;
            else rank = 0.5f + i - HandcardsList.Count / 2;

            float angle = 90 - rank * DeltaAngle;
            if (player == Battle.GetInstance().player2) angle = 270 + rank * DeltaAngle;
            float radian = (angle / 180) * Mathf.PI;
            float xx = centerx + R * Mathf.Cos(radian);
            float yy = centery + r * Mathf.Sin(radian);
            if (player == Battle.GetInstance().player2) yy = AIcentery + r * Mathf.Sin(radian);

            HandcardsList[i].cardobject.transform.position = new Vector3(xx, yy, 0);
            HandcardsList[i].cardobject.transform.localScale = new Vector3(1, 1, 1);
            HandcardsList[i].cardobject.transform.rotation = Quaternion.Euler(0, 0, rank * RotationAngle);
            if (player == Battle.GetInstance().player1) 
                HandcardsList[i].cardobject.transform.rotation = Quaternion.Euler(0, 0, rank * RotationAngle);
            else
                HandcardsList[i].cardobject.transform.rotation = Quaternion.Euler(0, 0, -rank * RotationAngle);
        }

    }


    public  void MoveCardPosition(Player player)
    {
        List<Card> HandcardsList = Turn.GetInstance().HandcardsList;
        if (player == Battle.GetInstance().player2) HandcardsList = Turn.GetInstance().AIHandcardList;
        bool isOdd = HandcardsList.Count % 2 == 1 ? true : false;
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            float rank = 0;
            if (isOdd) rank = i - HandcardsList.Count / 2;
            else rank = 0.5f + i - HandcardsList.Count / 2;

            float angle = 90 - rank * DeltaAngle;
            if (player == Battle.GetInstance().player2) angle = 270 + rank * DeltaAngle;
            float radian = (angle / 180) * Mathf.PI;

            float xx = centerx + R * Mathf.Cos(radian);
            float yy = centery + r * Mathf.Sin(radian);
            if (player == Battle.GetInstance().player2) yy = AIcentery + r * Mathf.Sin(radian);

            iTween.MoveTo(HandcardsList[i].cardobject, new Vector3(xx, yy, 0), MoveTime);
            iTween.ScaleTo(HandcardsList[i].cardobject, new Vector3(1, 1, 1), MoveTime);
            if (player == Battle.GetInstance().player1) 
                iTween.RotateTo(HandcardsList[i].cardobject, new Vector3(0, 0, rank * RotationAngle), MoveTime);
            else
                iTween.RotateTo(HandcardsList[i].cardobject, new Vector3(0, 0, -rank * RotationAngle), MoveTime);   
        }
    }

    /**************************** HandCards ZoomOut&ZoomIn ****************************************/

    public  void ZoomOutHandCard(GameObject handcard)
    {
        List<Card> HandcardsList = Turn.GetInstance().HandcardsList;
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            if (HandcardsList[i].cardobject == handcard)
            {
                HandcardsList[i].SaveCardPos();
                Vector3 newPosition = handcard.transform.position;
                newPosition.y = -400;

                iTween.MoveTo(HandcardsList[i].cardobject, newPosition, ZoomOutTime);
                iTween.ScaleTo(HandcardsList[i].cardobject, new Vector3(ZoomOutScale, ZoomOutScale, ZoomOutScale), ZoomOutTime);
                iTween.RotateTo(HandcardsList[i].cardobject, Quaternion.Euler(0, 0, 0).eulerAngles, ZoomOutTime);


                handcard.transform.SetAsLastSibling();

                if (HandcardsList.Count == 1)
                {

                }
                else if (HandcardsList.Count == 2)
                {
                    if (i == 0)
                    {
                        HandcardsList[i + 1].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i + 1], i + 1, 1, ChangeRank);
                    }
                    else
                    {
                        HandcardsList[i - 1].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i - 1], i - 1, -1, ChangeRank);
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        HandcardsList[i + 1].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i + 1], i + 1, 1, ChangeRank);

                        HandcardsList[i + 2].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i + 2], i + 2, 1, ChangeNextRank);
                    }
                    else if (i == HandcardsList.Count - 1)
                    {
                        HandcardsList[i - 1].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i - 1], i - 1, -1, ChangeRank);

                        HandcardsList[i - 2].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i - 2], i - 2, -1, ChangeNextRank);
                    }
                    else
                    {
                        HandcardsList[i - 1].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i - 1], i - 1, -1, ChangeRank);
                        HandcardsList[i + 1].SaveCardPos();
                        ZoomOutMoveOther(HandcardsList[i + 1], i + 1, 1, ChangeRank);

                        if (i != 1)
                        {
                            HandcardsList[i - 2].SaveCardPos();
                            ZoomOutMoveOther(HandcardsList[i - 2], i - 2, -1, ChangeNextRank);
                        }
                        if (i != HandcardsList.Count - 2)
                        {
                            HandcardsList[i + 2].SaveCardPos();
                            ZoomOutMoveOther(HandcardsList[i + 2], i + 2, 1, ChangeNextRank);
                        }

                    }
                }

            }
        }
    }

    public  void ZoomInHandCard(GameObject handcard)
    {
        List<Card> HandcardsList = Turn.GetInstance().HandcardsList;
        for (int i = 0; i < HandcardsList.Count; i++)
        {
            if (HandcardsList[i].cardobject == handcard)
            {


                handcard.transform.SetSiblingIndex(HandcardsList[i].SiblingIndex);
                RestorreCardTransform(HandcardsList[i]);

                if (HandcardsList.Count == 1)
                {
                }
                else if (HandcardsList.Count == 2)
                {
                    if (i == 0)
                    {
                        RestorreCardTransform(HandcardsList[i + 1]);
                    }
                    else
                    {
                        RestorreCardTransform(HandcardsList[i - 1]);
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        RestorreCardTransform(HandcardsList[i + 1]);
                        RestorreCardTransform(HandcardsList[i + 2]);
                    }
                    else if (i == HandcardsList.Count - 1)
                    {
                        RestorreCardTransform(HandcardsList[i - 1]);
                        RestorreCardTransform(HandcardsList[i - 2]);
                    }
                    else
                    {
                        RestorreCardTransform(HandcardsList[i - 1]);
                        RestorreCardTransform(HandcardsList[i + 1]);

                        if (i != 1)
                        {
                            RestorreCardTransform(HandcardsList[i - 2]);
                        }
                        if (i != HandcardsList.Count - 2)
                        {
                            RestorreCardTransform(HandcardsList[i + 2]);
                        }

                    }
                }
            }
        }
    }

    private  void RestorreCardTransform(Card card)
    {

        iTween.MoveTo(card.cardobject, card.position, 0f);
        iTween.RotateTo(card.cardobject, card.rotation.eulerAngles, 0f);
        iTween.ScaleTo(card.cardobject, card.scale, 0f);

    }

    private  void ZoomOutMoveOther(Card card, int index, int left, float MoveRank)
    {

        float rank = 0;
        if (Turn.GetInstance().HandcardsList.Count % 2 == 1) rank = index - Turn.GetInstance().HandcardsList.Count / 2;
        else rank = 0.5f + index - Turn.GetInstance().HandcardsList.Count / 2;

        if (left == -1) rank -= MoveRank;
        if (left == 1) rank += MoveRank;


        float angle = 90 - rank * DeltaAngle;
        float radian = (angle / 180) * Mathf.PI;

        float xx = centerx + R * Mathf.Cos(radian);
        float yy = centery + r * Mathf.Sin(radian);


        iTween.MoveTo(card.cardobject, new Vector3(xx, yy, 0), ZoomOutMoveOtherTime);
        iTween.RotateTo(card.cardobject, new Vector3(0, 0, rank * RotationAngle), 0f);


    }


    /**************************** Deal & Discard HandCards ****************************************/

    public  void DealHandCard(int index){
        List<Card> HandcardsList = Turn.GetInstance().HandcardsList;
        float DealSingleTime = DealTotalTime - (HandcardsList.Count - 1) * DealDeltaTime;
        GameObject card = HandcardsList[index].cardobject;

        card.SetActive(true);
        card.transform.position = new Vector3(850, -500, 0);
        card.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        float rank = 0;
        if (HandcardsList.Count%2==1) rank = index - HandcardsList.Count / 2;
        else rank = 0.5f + index - HandcardsList.Count / 2;

        float angle = 90 - rank * DeltaAngle;
        float radian = (angle / 180) * Mathf.PI;

        float xx = centerx + R * Mathf.Cos(radian);
        float yy = centery + r * Mathf.Sin(radian);

        iTween.MoveTo(card, new Vector3(xx, yy, 0), DealSingleTime);
        iTween.RotateTo(card, new Vector3(0, 0, rank * RotationAngle), DealSingleTime);
    }

    public  void DiscardHandCards(Card card,int amount){
        float DealSingleTime = DealTotalTime - (amount - 1) * DealDeltaTime;
        iTween.MoveTo(card.cardobject, new Vector3(-820, -500, 0), DealSingleTime);
        iTween.RotateTo(card.cardobject, new Vector3(0, 0, 0), DealSingleTime);
    }

    public IEnumerator TrapAnimation(Card card)
    {
        GameObject cardobj = GameObject.Find("UIManager").GetComponent<UIManager>().AddTrapCardObj(card);
        GameObject TrapInfo = GameObject.Find("UIManager").GetComponent<UIManager>().AddTrapInfo();
        cardobj.transform.position = new Vector3(520, 450, 0);
        cardobj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        TrapInfo.transform.position = new Vector3(0, 120, 0);
        float time = 1.5f;
        iTween.MoveTo(cardobj, new Vector3(-800, 0, 0), time);
        iTween.ScaleTo(cardobj, new Vector3(1.3f, 1.3f, 1.3f), time);
        iTween.PunchScale(TrapInfo, new Vector3(1.7f, 1.7f, 1.7f), time);
        yield return new WaitForSeconds(time);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelObj(cardobj);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelObj(TrapInfo);
    }


    public void ChangePlayer1CardDescription(){

        for (int i = 0; i < Turn.GetInstance().HandcardsList.Count;i++){
            ChangeOneCardUI(Turn.GetInstance().HandcardsList[i], Battle.GetInstance().player1);
        }
    }

    public void ChangeOneCardUI(Card card,Player player){
        card.InitCard();
        card.PlayerPropertyCheck(player);
        card.BuffCheck(player);
        Transform des = card.cardobject.transform.Find("Text_card");
        if (card.attack != card.attackAfterCal)
        {
            des.GetComponent<Text>().text = des.GetComponent<Text>().text.Replace(card.attack.ToString(), "<color=#00FF00><b>" + card.attackAfterCal.ToString() + "</b></color>");
        }
        if (card.defense != card.defenseAfterCal)
        {
            des.GetComponent<Text>().text = des.GetComponent<Text>().text.Replace(card.defense.ToString(), "<color=#00FF00><b>" + card.defenseAfterCal.ToString() + "</b></color>");
        }
    }

    public IEnumerator AIUseCardAnimation (int cardid){
        int k = Random.Range(0, Turn.GetInstance().AIHandcardList.Count);
        GameObject cardback = Turn.GetInstance().AIHandcardList[k].cardobject;
        GameObject parent = GameObject.Find("Canvas/Card_use");
        iTween.MoveTo(cardback, parent.transform.position, 1);
        iTween.RotateTo(cardback, new Vector3(0, 0, 0), 1);
        yield return new WaitForSeconds(1);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelObj(cardback);
        Turn.GetInstance().AIHandcardList.RemoveAt(k);
        UI.GetInstance().MoveCardPosition(Battle.GetInstance().player2);
        Card card= GameObject.Find("UIManager").GetComponent<UIManager>().CreateCard(cardid);
        ChangeOneCardUI(card, Battle.GetInstance().player2);
        card.AIUse();
        yield return new WaitForSeconds(2);
        GameObject.Find("UIManager").GetComponent<UIManager>().DelObj(card.cardobject);
    }

    public void SetTrapCardUnuse(){
        for (int i = 0; i < Battle.GetInstance().player1.trapcards.Count;i++){
            for (int j = 0; j < Turn.GetInstance().HandcardsList.Count;j++){
                if(Battle.GetInstance().player1.trapcards[i].id == Turn.GetInstance().HandcardsList[j].id){
                    Turn.GetInstance().HandcardsList[j].canuse = 0;
                }
            }
        }
    }
}



public class UIManager : MonoBehaviour
{


    public void Start()
    {

    }
    public void Update()
    {
        float speed = 0.013f;
        if(UI.GetInstance().StartBloodDamage == 1){
            Image image = GameObject.Find("Canvas/Hp_me/Yellow").GetComponent<Image>();
            Player player = Battle.GetInstance().player1;
            float endfillAmout = player.hp / (float)player.hp_max;
            if (image.fillAmount - endfillAmout < 0.8) speed = 0.010f;
            if (image.fillAmount - endfillAmout < 0.5) speed = 0.007f;
            if (image.fillAmount - endfillAmout < 0.2) speed = 0.005f;
            image.fillAmount -= speed;
            if (image.fillAmount <= endfillAmout ) {
                UI.GetInstance().StartBloodDamage = 0;
            }
        }else if(UI.GetInstance().StartBloodDamage == 2)
        {
            Image image = GameObject.Find("Canvas/Hp_oppo/Yellow").GetComponent<Image>();
            Player player = Battle.GetInstance().player1;
            float endfillAmout = player.hp / (float)player.hp_max;
            if (image.fillAmount - endfillAmout < 0.8) speed = 0.007f;
            if (image.fillAmount - endfillAmout < 0.5) speed = 0.003f;
            if (image.fillAmount - endfillAmout < 0.2) speed = 0.001f;
            image.fillAmount -= speed;
            if (image.fillAmount <= endfillAmout)
            {
                UI.GetInstance().StartBloodDamage = 0;
            } 
        }
    }

    public void AddBuffUI(Player player,int id)
    {
        GameObject parent = GameObject.Find("Canvas/Buff_frame");
        GameObject buffobj = Instantiate(Resources.Load("Prefab/Buff/" + id.ToString()) as GameObject, parent.transform.position, Quaternion.identity, parent.transform);
        Buff newbuff = new Buff();
        newbuff.id = id;
        newbuff.BuffObj = buffobj;
        if(player == Battle.GetInstance().player1) UI.GetInstance().Buffobjs.Add(newbuff);
        else UI.GetInstance().AIBuffobjs.Add(newbuff);
        UI.GetInstance().AdjustBuffPos();
    }
    public void DelBuffUI(Player player,int id)
    {
        if(player == Battle.GetInstance().player1){
            for (int i = 0; i < UI.GetInstance().Buffobjs.Count; i++)
            {
                if (UI.GetInstance().Buffobjs[i].id == id)
                {
                    Destroy(UI.GetInstance().Buffobjs[i].BuffObj);
                    UI.GetInstance().Buffobjs.RemoveAt(i);
                    break;
                }
            }
        }else{
            for (int i = 0; i < UI.GetInstance().AIBuffobjs.Count; i++)
            {
                if (UI.GetInstance().AIBuffobjs[i].id == id)
                {
                    Destroy(UI.GetInstance().AIBuffobjs[i].BuffObj);
                    UI.GetInstance().AIBuffobjs.RemoveAt(i);
                    break;
                }
            } 
        }

         UI.GetInstance().AdjustBuffPos();
    }

    public void AddBattery(int num)
    {
        GameObject parent = GameObject.Find("Canvas/Battery_frame");
        for (int i = 0; i < num; i++)
        {
            UI.GetInstance().batteryobjs.Add(Instantiate(Resources.Load("Prefab/BatteryLight") as GameObject, parent.transform.position, Quaternion.identity, parent.transform));
        }
        UI.GetInstance().CalBatteryPos();
    }
    public void DelBattery(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Destroy(UI.GetInstance().batteryobjs[UI.GetInstance().batteryobjs.Count - 1]);
            UI.GetInstance().batteryobjs.RemoveAt(UI.GetInstance().batteryobjs.Count - 1);
        }
        UI.GetInstance().CalBatteryPos();
    }

    public void AddTrap(Player player,int id){
        Card card = new Card();
        card.id = id;
        GameObject parent = GameObject.Find("Canvas/Trap_frame");
        card.trapobject = Instantiate(Resources.Load("Prefab/trap") as GameObject, parent.transform.position, Quaternion.identity, parent.transform);
        if (player == Battle.GetInstance().player1) 
            UI.GetInstance().trapobjs.Add(card);
        else 
            UI.GetInstance().AItrapobjs.Add(card);
        UI.GetInstance().AdjusttrapPos();
    }
    public void DelTrap(Player player,int id){
        if (player == Battle.GetInstance().player1){
            for (int i = 0; i < UI.GetInstance().trapobjs.Count; i++)
            {
                if (UI.GetInstance().trapobjs[i].id == id)
                {
                    Destroy(UI.GetInstance().trapobjs[i].trapobject);
                    UI.GetInstance().trapobjs.RemoveAt(i);
                    break;
                }
            }
        }else{
            for (int i = 0; i < UI.GetInstance().AItrapobjs.Count; i++)
            {
                if (UI.GetInstance().AItrapobjs[i].id == id)
                {
                    Destroy(UI.GetInstance().AItrapobjs[i].trapobject);
                    UI.GetInstance().AItrapobjs.RemoveAt(i);
                    break;
                }
            }
        }

        UI.GetInstance().AdjusttrapPos();
    }

    public void OnMouseEnterTrap()
    {
        if(Turn.GetInstance().turnid % 2 == 1){
            for (int i = 0; i < UI.GetInstance().trapobjs.Count; i++)
            {
                if (this.gameObject == UI.GetInstance().trapobjs[i].trapobject)
                {
                    GameObject parent = GameObject.Find("Canvas/Trap_frame");
                    UI.GetInstance().trapcarddes = Instantiate(Resources.Load("Prefab/Cards/" + UI.GetInstance().trapobjs[i].id) as GameObject, parent.transform.position, Quaternion.identity, parent.transform);
                    Card GetCard = UI.GetInstance().trapcarddes.GetComponent<Card>();
                    UI.GetInstance().trapcarddes.transform.Find("Text_card").GetComponent<Text>().text = GetCard.des;
                    UI.GetInstance().trapcarddes.transform.Find("Text_Title").GetComponent<Text>().text = GetCard.cardname;
                    UI.GetInstance().trapcarddes.transform.Find("Text_cost").GetComponent<Text>().text = GetCard.cost.ToString();
                    UI.GetInstance().trapcarddes.transform.position = new Vector3(-400, -150, 0);
                    break;
                }
            }
        }
    }

    public void OnMouseExitTrap()
    {
        Destroy(UI.GetInstance().trapcarddes);
    }

    public GameObject AddTrapCardObj(Card card){
        GameObject parent = GameObject.Find("Canvas/Card_use");
        GameObject cardobject = Instantiate(Resources.Load("Prefab/Cards/" + card.id) as GameObject, parent.transform.position, Quaternion.identity, parent.transform);
        Card GetCard = cardobject.GetComponent<Card>();
        cardobject.transform.Find("Text_card").GetComponent<Text>().text = GetCard.trapdes;
        cardobject.transform.Find("Text_Title").GetComponent<Text>().text = GetCard.cardname;
        cardobject.transform.Find("Text_cost").GetComponent<Text>().text = GetCard.cost.ToString();
        return cardobject;
    }
    public GameObject AddTrapInfo(){
        GameObject parent = GameObject.Find("Canvas");
        GameObject TrapInfo = Instantiate(Resources.Load("Prefab/TrapInfo") as GameObject, parent.transform.position, Quaternion.identity, parent.transform);
        return TrapInfo;
    }
    public void DelObj(GameObject cardobj){
        Destroy(cardobj);
    }
    public void PlayTrapAnimation(Card card){
        StartCoroutine(UI.GetInstance().TrapAnimation(card));
    }

    public void PlayAIPlayCardAnimation(int cardid)
    {
        StartCoroutine(UI.GetInstance().AIUseCardAnimation(cardid));
    }

    public Card CreateCard(int cardid){
        GameObject parent = GameObject.Find("Canvas/Card_use");
        GameObject cardobject = Instantiate(Resources.Load("Prefab/Cards/" + cardid.ToString()) as GameObject, parent.transform.position, Quaternion.identity, parent.transform);
        Card GetCard = cardobject.GetComponent<Card>();
        cardobject.transform.Find("Text_card").GetComponent<Text>().text = GetCard.des;
        cardobject.transform.Find("Text_Title").GetComponent<Text>().text = GetCard.cardname;
        cardobject.transform.Find("Text_cost").GetComponent<Text>().text = GetCard.cost.ToString();
        GetCard.cardobject = cardobject;
        return GetCard;
    }

}