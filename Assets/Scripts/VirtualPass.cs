using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class VirtualPass : MonoBehaviour
{
    //This class attaches listener to virtual pass button in virtual pass screen and runs the script when clicked

    // References to this button
    public Button button;
    
    // References to the confirmation screen
    public GameObject confScreen;

    //References to the MapsSceneManager script
    private MapSceneManager mapSceneManagerScript;

    // Reference to table used in string localization
    private LocalizedString localizedString = new LocalizedString() { TableReference = "Translations", TableEntryReference = "CONFIRMATION_TEXT" };

    //???
    public static float destinationLatitude;
    public static float destinationLongitude;

    private void Start()
    {
        mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();
        confScreen = mapSceneManagerScript.confScreen;
        button.onClick.AddListener(ButtonClick);
    }

    //Sets confirmationscreen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class
    public void ButtonClick()
    
    {
        string locationName = gameObject.GetComponent<Coordinates>().locationName;
        var child = confScreen.transform.GetChild(0).transform.GetChild(0);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString() + " " + locationName + "?";
        RectTransform Pos = confScreen.GetComponent<RectTransform>();
        Pos.SetSiblingIndex(MapSceneManager.siblingIndex);
        confScreen.SetActive(true);

        CoordinateData.locationName = gameObject.GetComponent<Coordinates>().locationName;
        CoordinateData.latitude = gameObject.GetComponent<Coordinates>().latitude;
        CoordinateData.longitude = gameObject.GetComponent<Coordinates>().longitude;
        CoordinateData.id = gameObject.GetComponent<Coordinates>().id;
        CoordinateData.type = gameObject.GetComponent<Coordinates>().type;
        
        destinationLatitude = CoordinateData.latitude;
        destinationLongitude = CoordinateData.longitude;
    }
}
