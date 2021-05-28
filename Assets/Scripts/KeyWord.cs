using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyWord : MonoBehaviour
{
    public string description;
    public Button keywordButton;
    public RectTransform descriptionScreen;
    public GameObject descriptionText;
    private MapSceneManager mapSceneManagerScript;
  
    void Start()
    {
        mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();
        descriptionScreen = mapSceneManagerScript.descriptionScreen;
        descriptionText = mapSceneManagerScript.descriptionText;
        keywordButton.onClick.AddListener(ShowDescription);       
    }

    public void ShowDescription()
    // Shows the description stored in the button in pop-up screen.
    {
        descriptionScreen.gameObject.SetActive(true);
        RectTransform pos = descriptionScreen.GetComponent<RectTransform>();
        pos.SetSiblingIndex(MapSceneManager.siblingIndex);

        string description = gameObject.GetComponent<KeyWord>().description;    
        descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = description;
    }
    
}
