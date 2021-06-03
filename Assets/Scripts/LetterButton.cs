using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    // This class attaches listener to letter buttons used for navigation in glossary screen and runs the script when clicked

    // References this button
    public Button letterButton;

    // References the the glossary scroll
    public GameObject scroll;

    void Start()
    {
        scroll = GameObject.Find("Glossary Scroll");
        letterButton.onClick.AddListener(LetterClick);      
    }

    // Scrolls to the corresponding letter in the glossary list that the button has.
    void LetterClick()
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
