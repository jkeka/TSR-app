using System.Collections;
using UnityEngine;

public class SailorFaceScript : MonoBehaviour
{
    // This class operates the sailor animations
    
    // Sailor face animation timer
    private float faceRate=0.6f;

    // Sailor face antimation index
    private int faceIndex;   

    // Sailor body animation selected
    private string currentBool="Talking";

    // Sailor animation timer
    private float talkTimer = 6.66f;

    // List of sailor animations
    private GameObject[] faceList;

    // Reference to gameobject where sailor faces are stored
    public GameObject SailorFaces;

    // Reference to sailor animator
    public Animator animator;

    // Reference to speech bubble
    public SpeechBubble speechBubble;

    private void Start()
    {
        faceList = new GameObject[4];
        for (int i = 0; i <= 3; i++)
        {
            faceList[i] = SailorFaces.transform.GetChild(i).gameObject;
        }
        speechBubble.QV += ChangeToEmote;
       
        StartCoroutine(ChangeFace());      
    }
   
    // Set animation for sailor
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
    
    // Changes sailor animation when speechbubble is clicked
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
    
    // Changes sailor face animation
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
        
    }

    // Disables sailor animation
    private void OnDisable()
    {
        speechBubble.QV -= ChangeToEmote;
    }
}
