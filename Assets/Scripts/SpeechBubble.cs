using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Button bubble;
    public Button qrButton;
    public RectTransform infoScreen;
    Coroutine routine = null;
    Transform child;
    public static int count = 0;
    private bool isEnabled = false;


    //SailorInteraction
    public delegate void AnimTrigger(int i);
    public AnimTrigger QV;
    public bool test;


    // Start is called before the first frame update
    void Start()
    {
       
        child = bubble.transform.GetChild(0);
        bubble.onClick.AddListener(ClickTextBubble);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = "Hei, tervetuloa! Klikkaa tekstikuplaa niin kerron sinulle aluksesta.";
    }

    public void ClickTextBubble()
    // Changes bubble text content when clicked.
    {
        try
        {
            if (test)
            {
                if (QV != null)
                    QV(2);
            }
            else
            {
                Debug.Log(Description.shipInfo.Count);
                if (QV != null)
                    QV(2);


                if (count == Description.shipInfo.Count - 1)
                {
                    bubble.onClick.RemoveListener(ClickTextBubble);
                    count = 1;
                    
                    qrButton.gameObject.SetActive(true);
                    Description.shipInfo.Clear();
                    Debug.Log(Description.shipInfo);
                    //routine = StartCoroutine(ResetBubble());
                    return;
                }

                child.GetComponent<TMPro.TextMeshProUGUI>().text = Description.shipInfo[count];
                count++;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
            child.GetComponent<TMPro.TextMeshProUGUI>().text = "Hups, tästä kohteesta puuttuu esittelyteksti!";
        }
    }

    private void OnDisable()
    {
        bubble.onClick.RemoveAllListeners();
        bubble.onClick.AddListener(ClickTextBubble);
        Debug.Log("Disabled");
    }

    IEnumerator ResetBubble()
    // Resets button listener and text after quitting infoscreen.
    {
        bool reset = false;

        while(reset == false)
        {
            yield return new WaitForSecondsRealtime(1);
            if (infoScreen.GetSiblingIndex() != MapSceneManager.siblingIndex || !infoScreen.gameObject.activeSelf )
            {
                reset = true;
                bubble.onClick.AddListener(ClickTextBubble);
                child.GetComponent<TMPro.TextMeshProUGUI>().text = "Hei, tervetuloa! Klikkaa tekstikuplaa niin kerron sinulle aluksesta.";
                StopCoroutine(routine);
                Debug.Log("speechbubble reset!");
            }      
        }        
    }


}
