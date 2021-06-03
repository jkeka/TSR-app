using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class MarkerButton : MonoBehaviour
{
    // This class attaches listener to marker buttons used for navigation in map screen and runs the script when clicked

    // Changes to marker button size depending on camera zoom
    private float buttonSize = Mathf.Clamp(1f, 0.4f, 1.5f);
    private float cameraSizeMultiplier;

    //???
    public static float destinationLatitude;
    public static float destinationLongitude;
    
    // References this button
    public Button button;

    // References confirmation screen in the MainCanvas
    public GameObject confScreen;

    // References MapSceneManager script
    private MapSceneManager mapSceneManagerScript;

    //References table used in string localization
    private LocalizedString localizedString = new LocalizedString() { TableReference = "Translations", TableEntryReference = "CONFIRMATION_TEXT"};

    private void Start()
    {     
        mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();
        confScreen = mapSceneManagerScript.confScreen;
        button.onClick.AddListener(MarkerClick);            
    }

    private void Update()
    {
        cameraSizeMultiplier = (Camera.main.orthographicSize * 0.0028f);
        transform.localScale = new Vector3(buttonSize * cameraSizeMultiplier, buttonSize * cameraSizeMultiplier, buttonSize * cameraSizeMultiplier);     
    }

    //Sets confirmationscreen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class
    public void MarkerClick()
       
    {
        mapSceneManagerScript.screens.SetActive(true);

        for (int i = 0; i < mapSceneManagerScript.screenObjects.Count; i++)
        {
            mapSceneManagerScript.screenObjects[i].SetActive(false);
        }

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
