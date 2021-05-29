using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;


public class SpeechBubble : MonoBehaviour
{
    public Button bubble;
    public Button qrButton;
    public RectTransform infoScreen;
    Coroutine routine = null;
    Transform child;
    LocalizedString localizedString = new LocalizedString() {TableReference = "Translations"};
    public static int count = 0;

    //SailorInteraction
    public delegate void AnimTrigger(int i);
    public AnimTrigger QV;
    public bool test;


    // Start is called before the first frame update
    void Start()
    {
     
        child = bubble.transform.GetChild(0);
        bubble.onClick.AddListener(ClickTextBubble);
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
                    routine = StartCoroutine(ResetBubble());
                    return;
                }

                child.GetComponent<TMPro.TextMeshProUGUI>().text = Description.shipInfo[count];
                count++;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
            if (QV != null)
                QV(2);
            localizedString.TableEntryReference = "INFO_ERROR";
            child.GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.Log(e);
        }
    }

    /*private void OnDisable()
    {
        bubble.onClick.RemoveAllListeners();
        bubble.onClick.AddListener(ClickTextBubble);
        qrButton.gameObject.SetActive(false);
        Debug.Log("speechbubble reset!");
    }*/

    IEnumerator ResetBubble()
    // Resets button listener and text after quitting infoscreen.
    {
       
        while(infoScreen.GetSiblingIndex() == MapSceneManager.siblingIndex)
        {
            yield return new WaitForSecondsRealtime(1);
        }

        bubble.onClick.RemoveAllListeners();
        bubble.onClick.AddListener(ClickTextBubble);
        qrButton.gameObject.SetActive(false);
        Debug.Log("speechbubble reset!");
        StopCoroutine(routine);               
    }
}
