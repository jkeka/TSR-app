using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Button bubble;
    GameObject quizScreen;
    Transform child;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        quizScreen = GameObject.Find("QuizScreen");
        child = bubble.transform.GetChild(0);
        ClickTextBubble();
        bubble.onClick.AddListener(ClickTextBubble);
    }

    void ClickTextBubble()
    // Changes bubble text content when clicked.
    {
        Debug.Log(Description.shipInfo.Count);
        
        if (count == Description.shipInfo.Count - 1) 
        {
            bubble.onClick.RemoveListener(ClickTextBubble);
            count = 0;
            quizScreen.transform.SetSiblingIndex(1);
            return;
        }
        child.GetComponent<TMPro.TextMeshProUGUI>().text = Description.shipInfo[count];
        count++;
    }
}
