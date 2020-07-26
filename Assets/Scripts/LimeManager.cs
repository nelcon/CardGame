using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LimeManager : MonoBehaviour {
    static LimeManager Lime;
    public GameObject[] chatobject = new GameObject[10];


    public void Clear() {
        for (int i = 0; i < chatobject.Length; i++) {
            Destroy(chatobject[i]);
        }        

    }

    public void Speak(string content) {
        MoveUp();
        int havepos = 0;
        GameObject parent = GameObject.Find("Canvas/Panel_lime");
        GameObject chat = Resources.Load("Prefab/Chat_player") as GameObject;
        for (int i = 0; i < chatobject.Length; i++) {
            if (chatobject[i] == null) {
                chatobject[i]= Instantiate(chat, parent.transform.position, Quaternion.identity, parent.transform);
                chatobject[i].GetComponent<Text>().text = content;
                StartCoroutine(Fadeout(chatobject[i]));
                havepos = 1;
                break;
            }

        }
        if (havepos == 0) {
            for (int j = 0; j < 8; j++) {
                chatobject[j] = chatobject[j + 1];
            }
            chatobject[9]= Instantiate(chat, parent.transform.position, Quaternion.identity, parent.transform);
            chatobject[9].GetComponent<Text>().text = content;
            StartCoroutine(Fadeout(chatobject[10]));
        }

    }

    void MoveUp() {
        int i = 0;
        for (i=0; i < 9; i++) {
            if (chatobject[i] != null) {
                chatobject[i].transform.position = new Vector3(chatobject[i].transform.position.x, chatobject[i].transform.position.y + 110, 0);
            }
        }
    }

    public IEnumerator MakeChoice(string text) {


        yield return new WaitForSeconds(1);
    }

    public IEnumerator Fadeout(GameObject chat) {
        yield return new WaitForSeconds(5);
        //MoveUp();
        Destroy(chat);

    }
    // Use this for initialization
    void Start () {
        Clear();
	}
	

	// Update is called once per frame
	void Update () {
		
	}
}
