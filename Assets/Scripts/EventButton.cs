using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class EventButton : MonoBehaviour
{
    public Button button;
    public GameObject confScreen;
    private LocalizedString localizedString = new LocalizedString() { TableReference = "Translations", TableEntryReference = "CONFIRMATION_TEXT" };

    // Start is called before the first frame update
    void Start()
    {
        confScreen = GameObject.Find("ConfirmationScreen");
        button.onClick.AddListener(EventClick);
    }

    public void EventClick()
    //Sets confirmationscreen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class

    {
        string venueID = gameObject.GetComponent<Event>().venueId;
            
        foreach (Button button in LocationDataHandler.markerList)
        {
            Debug.Log(venueID + " " + button.GetComponent<Coordinates>().id);
                     
            if (venueID == button.GetComponent<Coordinates>().id)
            {
                string description = gameObject.GetComponent<Event>().eventDescription;
                string locationName = button.GetComponent<Coordinates>().locationName;
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
