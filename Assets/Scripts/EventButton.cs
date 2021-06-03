using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class EventButton : MonoBehaviour
{
    //This class attaches listener to event buttons in schedule screen and runs the script when clicked

    // References this button
    public Button button;

    // References gameobject confirmation screen
    public GameObject confScreen;

    // References MapSceneManager script
    private MapSceneManager mapSceneManagerScript;

    //References table used in string localization
    private LocalizedString localizedString = new LocalizedString() { TableReference = "Translations", TableEntryReference = "CONFIRMATION_TEXT" };

    // Start is called before the first frame update
    void Start()
    {
        mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();
        confScreen = mapSceneManagerScript.confScreen;
        button.onClick.AddListener(EventClick);
    }

    //Sets confirmation screen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class
    public void EventClick()   
    {
        string venueID = gameObject.GetComponent<Event>().venueId;
            
        foreach (Button button in LocationDataHandler.markerList)
        {                   
            if (venueID == button.GetComponent<Coordinates>().id)
            {
                string description = gameObject.GetComponent<Event>().eventDescription;
                string locationName = button.GetComponent<Coordinates>().locationName;
                
                confScreen.SetActive(true);
                var child = confScreen.transform.GetChild(0).transform.GetChild(0);

                child.GetComponent<TMPro.TextMeshProUGUI>().text = description + "\n\n" + localizedString.GetLocalizedString() + " " + locationName + "?";
                RectTransform Pos = confScreen.GetComponent<RectTransform>();
                Pos.SetSiblingIndex(MapSceneManager.siblingIndex);

                CoordinateData.locationName = button.GetComponent<Coordinates>().locationName;
                CoordinateData.latitude = button.GetComponent<Coordinates>().latitude;
                CoordinateData.longitude = button.GetComponent<Coordinates>().longitude;
                CoordinateData.id = button.GetComponent<Coordinates>().id;
                CoordinateData.type = button.GetComponent<Coordinates>().type;

                Debug.Log("Event button clicked!");

                return;
            }
        }
    }
}
