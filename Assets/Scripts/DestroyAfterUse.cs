using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DestroyAfterUse : MonoBehaviour
{
    // Use this for initialization
    public void Start()
    {
        StartCoroutine(DestoryDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator DestoryDelay()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
}