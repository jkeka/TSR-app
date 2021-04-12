using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    public Button letterButton;
    public GameObject scroll;

    void Start()
    {
        scroll = GameObject.Find("Glossary Scroll");
        letterButton.onClick.AddListener(LetterClick);
        
    }

    void LetterClick()
    // Scrolls to the corresponding letter in the glossary list that the button has.
    {
        foreach (Button letter in LibraryScript.letterList)
        {
            string character = letterButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;

            if(character == letter.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text)
            {
                RectTransform letterRect = letter.GetComponent<RectTransform>();
                ScrollRect scrollRect = scroll.GetComponent<ScrollRect>();
                float scrollValue = 1 + letterRect.anchoredPosition.y / scrollRect.content.rect.height;
                scrollRect.verticalScrollbar.value = scrollValue;
            }           
        }
    }

   
}
