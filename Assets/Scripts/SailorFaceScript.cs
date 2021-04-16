using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailorFaceScript : MonoBehaviour
{
    public SpeechBubble speechBubble;

 

    private float faceRate=0.6f;
    private int faceIndex;
    private GameObject[] faceList;
    public Animator animator;
    private string currentBool="Talking";
    float talkTimer = 6.66f;
    //private IEnumerator talkRoutine;

    private void Start()
    {
        faceList = new GameObject[4];
        for (int i = 0; i <= 3; i++)
        {
            faceList[i] = transform.GetChild(i).gameObject;
        }
        speechBubble.QV += ChangeToEmote;

        //talkRoutine = ChangeFace();
        StartCoroutine(ChangeFace());
        //Emote(0);
        //ChangeToEmote(2);
    }

    /// <summary>
    /// 0=greet, 1=wave, 2=talk
    /// </summary>
    /// <param name="index"></param>
    public void Emote(int index)
    {
        string trigger="Talk";
        switch (index)
        {
            case 0:
                trigger = "Salute";
                break;
            case 1:
                trigger = "Wave";
                break;
            case 2:
                trigger = "Talk";
                break;
        }
        animator.SetTrigger(trigger);
    }
    /// <summary>
    /// 0=greet, 1=wave, 2=talk
    /// </summary>
    /// <param name="index"></param>
    public void ChangeToEmote(int index)
    {
        string trigger = "Talking";
        switch (index)
        {
            case 0:
                trigger = "Saluting";
                break;
            case 1:
                trigger = "Waving";
                break;
            case 2:
                trigger = "Talking";
                break;
        }
        animator.SetBool(currentBool, false);
        currentBool = trigger;
        animator.SetBool(trigger,true);
        Emote(0);
        Debug.Log("changetoCalled");
        StopCoroutine(ChangeFace());
        talkTimer = 5.5f;
        StartCoroutine(ChangeFace());
    }
    

    public IEnumerator ChangeFace()
    {
        while (talkTimer >= 0f)
        {
            faceRate = Random.Range(0.1f, 0.6f);

            faceIndex = Random.Range(0, 3);
            foreach (var item in faceList)
            {
                item.SetActive(false);
            }
            faceList[faceIndex].SetActive(true);
            talkTimer -= faceRate;
            yield return new WaitForSeconds(faceRate);
        }
        foreach (var item in faceList)
        {
            item.SetActive(false);
        }
        faceList[0].SetActive(true);
        yield return null;
        //StopCoroutine();
    }
    private void OnDisable()
    {
        speechBubble.QV -= ChangeToEmote;
    }
}
