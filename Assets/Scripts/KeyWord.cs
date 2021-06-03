using UnityEngine;
using UnityEngine.UI;

public class KeyWord : MonoBehaviour
{
    // This class attaches listener to keyword buttons in glossary screen and runs the script when clicked

    // Stores the description got from the glossary
    public string description;

    // References this button
    public Button keywordButton;

    // References description screen
    public RectTransform descriptionScreen;

    // References text gameobject in description screen
    public GameObject descriptionText;

    // References MapsSceneManager script
    private MapSceneManager mapSceneManagerScript;
  
    void Start()
    {
        mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();
        descriptionScreen = mapSceneManagerScript.descriptionScreen;
        descriptionText = mapSceneManagerScript.descriptionText;
        keywordButton.onClick.AddListener(ShowDescription);       
    }

    // Shows the description stored in the button in pop-up screen.
    public void ShowDescription()
    {
        descriptionScreen.gameObject.SetActive(true);
        RectTransform pos = descriptionScreen.GetComponent<RectTransform>();
        pos.SetSiblingIndex(MapSceneManager.siblingIndex);

        string description = gameObject.GetComponent<KeyWord>().description;    
        descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = description;
    }
    
}
