using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailorFaceScript : MonoBehaviour
{
    private float faceRate=0.6f;
    private int faceIndex;
    private GameObject[] faceList;
    public Animator animator;
    private string currentBool="Talking";
    private void Start()
    {
        faceList = new GameObject[4];
        for (int i = 0; i <= 3; i++)
        {
            faceList[i] = transform.GetChild(i).gameObject;
        }

        StartCoroutine(ChangeFace());
        Emote(0);
        ChangeToEmote(2);
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
                trigger = "Greet";
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
                trigger = "Greeting";
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
    }
    

    public IEnumerator ChangeFace()
    {
        while (true)
        {
            faceRate = Random.Range(0.1f, 0.6f);

            faceIndex = Random.Range(0, 3);
            foreach (var item in faceList)
            {
                item.SetActive(false);
            }
            faceList[faceIndex].SetActive(true);
            yield return new WaitForSeconds(faceRate);
        }
    }
}
