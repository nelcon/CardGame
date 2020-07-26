using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Battle 
{
    static Battle battle;
    public int id;
    public Player player1;
    public Player player2;
    public int hpinit_player1;
    public int hpinit_player2;
    public AI ai_me;
    public AI ai_oppo;
    public Deck deckoppo;
    public int battlerule;
    public int battleresult;
    public List<Card> player1stack = new List<Card>();
    public List<Card> player2stack = new List<Card>();
    public List<Card> player1cemetery = new List<Card>();
    public List<Card> player2cemetery = new List<Card>();
    public string[] battlescript = new string[30];    //用来存放AI行为
    public int stage;
    public int[] scriptlock = new int[10];

    public static Battle GetInstance()
    {
        if (battle == null)
        {
            battle = new Battle();
        }
        return battle;
    }
    public Battle()
    {
        player1 = new Player();
        player2 = new Player();
        deckoppo = new Deck();
        ai_me = new AI();
        ai_oppo = new AI();
        deckoppo.id = 0;
        battleresult = 0;
        stage = 0;
        deckoppo =deckoppo.ReadFromFile();
    }

    public int BattleStart()
    {
        LoadPlayer1();
        LoadPlayer2();
        LoadStack();
        LoadBattleScript();
        LoadIniBuffs();
        return 0;
    }

    public void LoadStack() {
        List<int> cardlist = Game.GetInstance().deckme.cardid;
        for (int i = 0; i < cardlist.Count;i++){
            Card card = new Card();
            card.id = cardlist[i];
            player1stack.Add(card);
        }        
        ShuffleStack();

    }

    public void LoadPlayer1() {
        player1.id = 0;
        player1 = player1.ReadFromFile();

    }

    public void LoadPlayer2()
    {
        player2.id = 1;
        player2 = player2.ReadFromFile();
    }

    public void LoadIniBuffs()
    {
        for (int i = 0; i <player1.inibuffid.Length; i++)
        {
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(player1, player1.inibuffid[i]);
        }
        for (int j = 0; j <player2.inibuffid.Length; j++)
        {
            GameObject.Find("BuffManager").GetComponent<BuffManager>().AddBuff(player2, player2.inibuffid[j]);
        }
    }

    public void ReloadCheck()
    {
        if (player1stack.Count == 0)
        {
            player1stack.AddRange(player1cemetery);
            ShuffleStack();
            player1cemetery.Clear();
        }

    }

    public int ShuffleStack()
    {
        System.Random r = new System.Random();
        for (int n = player1stack.Count - 1; n > 0; --n)
        {
            int k = r.Next(n + 1);
            Card temp = player1stack[n];
            player1stack[n] = player1stack[k];
            player1stack[k] = temp;
        }
        return 0;
    }

    public int RuleCheck() {
        if (battlerule == 1) {
            if (player1.hp <= 0) {
                Lose();
                return -1;
            }
            if (player2.hp <= 0) {
                if(NextStage()==0)
                {
                    Win();
                }
                
                return 1;
            }
            return 0;
        }

        return 0;
    }

    public void Win() {
        GameObject.Find("MessageBox").GetComponent<MessageBoxExample>().Show();
        battleresult = 1;
    }

    public void Lose() {

        GameObject.Find("MessageBox").GetComponent<MessageBoxExample>().title = "You Lose";
        GameObject.Find("MessageBox").GetComponent<MessageBoxExample>().Show();
        battleresult = -1;

    }

    int NextStage() {
        stage++;
        if (File.Exists(Application.streamingAssetsPath + "/Data/Battles/" + id.ToString() + "_" + stage.ToString() + ".res")) {
            Turn.GetInstance().turnid = 0;
            
            LoadPlayer2();
            LoadBattleScript();
            Turn.GetInstance().EndTurn();
            return 1;
        }
        else
        {
            return 0;
        }
        
    }

    public void LoadBattleScript() {
        StreamReader sr = null;
        battlescript = new string[30]; 
        try
        {
            sr = File.OpenText(Application.streamingAssetsPath + "/Data/Battles/"+id.ToString()+"_"+stage.ToString()+".res");
        }
        catch
        {
            Debug.Log("file not finded");
            return;
        }
        string line;
        int turn = 0;
        while ((line = sr.ReadLine()) != null)
        {
            if (line.StartsWith("{"))
            {
                turn = int.Parse(line.Substring(5, (line.IndexOf('}') - 5)));
            }
            else { 
            battlescript[turn] += line;
            }
        }
        sr.Close();
        sr.Dispose();
        //Debug.Log(battlescript[1]);
    }
}

public class BattleManager : MonoBehaviour
{
        
    private void Start()
    {

        Battle.GetInstance().id = 2;
        Battle.GetInstance().battlerule = 1;
        Battle.GetInstance().BattleStart();
        Turn.GetInstance().drawnumber = 6;
        Turn.GetInstance().maxbattery = Battle.GetInstance().player1.maxbattery;
        GameObject.Find("UIManager").GetComponent<UIManager>().AddBattery(Battle.GetInstance().player1.maxbattery);
        StartCoroutine(Turn.GetInstance().AIPerform());
    }
    private void Update()
    {
        BattleInfoUpdate();

        if (Battle.GetInstance().battleresult == 0) {
            Battle.GetInstance().RuleCheck();
        }
        
        if (Turn.GetInstance().aioverflag == 1)
        {
            StartCoroutine(Turn.GetInstance().AIPerform());
            Turn.GetInstance().aioverflag = 0;
        }
        
    }

    public void BattleInfoUpdate() {
        GameObject.Find("Canvas/Hp_me/value").GetComponent<Text>().text = Battle.GetInstance().player1.hp.ToString();
        GameObject.Find("Canvas/Hp_oppo/value").GetComponent<Text>().text = Battle.GetInstance().player2.hp.ToString();
        GameObject.Find("Canvas/Armor_me/Text").GetComponent<Text>().text = Battle.GetInstance().player1.armor.ToString();
        GameObject.Find("Canvas/Armor_oppo/Text").GetComponent<Text>().text = Battle.GetInstance().player2.armor.ToString();

        GameObject.Find("Canvas/My_Property/Power").GetComponent<Text>().text = Battle.GetInstance().player1.power.ToString();
        GameObject.Find("Canvas/My_Property/Agility").GetComponent<Text>().text = Battle.GetInstance().player1.agility.ToString();
        GameObject.Find("Canvas/AI_Property/Power").GetComponent<Text>().text = Battle.GetInstance().player2.power.ToString();
        GameObject.Find("Canvas/AI_Property/Agility").GetComponent<Text>().text = Battle.GetInstance().player2.agility.ToString();

        GameObject.Find("Canvas/Button_stack/Redpoint/Text").GetComponent<Text>().text = Battle.GetInstance().player1stack.Count.ToString();
        GameObject.Find("Canvas/Button_cemetery/Redpoint/Text").GetComponent<Text>().text = Battle.GetInstance().player1cemetery.Count.ToString();
    }

}