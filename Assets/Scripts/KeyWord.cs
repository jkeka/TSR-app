using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyWord : MonoBehaviour
{
    public string description;
    public Button keywordButton;
    public GameObject descriptionScreen;
    public GameObject descriptionText;
  
    void Start()
    {
        descriptionScreen = GameObject.Find("DescriptionScreen");
        descriptionText = GameObject.Find("DescriptionText");
        keywordButton.onClick.AddListener(ShowDescription);       
    }

    public void ShowDescription()
    // Shows the description stored in the button in pop-up screen.
    {
        string description = gameObject.GetComponent<KeyWord>().description;    
        descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = description;
        RectTransform pos = descriptionScreen.GetComponent<RectTransform>();
        pos.SetSiblingIndex(MapSceneManager.siblingIndex);

    }
    
}
