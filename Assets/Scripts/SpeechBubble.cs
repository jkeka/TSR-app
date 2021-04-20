using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Button bubble;
    public Button qrButton;
    Transform child;
    int count = 0;


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

    void ClickTextBubble()
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
                    count = 0;
                    
                    qrButton.gameObject.SetActive(true);

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
}
