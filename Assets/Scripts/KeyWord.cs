using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyWord : MonoBehaviour
{
    public string description;
    public Button keywordButton;
    public GameObject descriptionScreen;
  
    void Start()
    {
        descriptionScreen = GameObject.Find("DescriptionScreen");
        keywordButton.onClick.AddListener(ShowDescription);       
    }

    public void ShowDescription()
    // Shows the description stored in the button in pop-up screen.
    {
        string description = gameObject.GetComponent<KeyWord>().description;
        var child = descriptionScreen.transform.GetChild(0).transform.GetChild(0);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = description;
        RectTransform Pos = descriptionScreen.GetComponent<RectTransform>();
        Pos.SetSiblingIndex(8);

    }
    
}
