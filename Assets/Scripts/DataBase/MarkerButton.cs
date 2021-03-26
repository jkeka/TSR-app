using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class MarkerButton : MonoBehaviour

{  
    public Button button;
    public GameObject confScreen;

    public static float destinationLatitude;
    public static float destinationLongitude;

    private void Start()
    {
        confScreen = GameObject.Find("ConfirmationScreen");
        button.onClick.AddListener(MarkerClick);            
    }

    public void MarkerClick()
        //Sets confirmationscreen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class

    {      
        string locationName = gameObject.GetComponent<Coordinates>().locationName;
        var child = confScreen.transform.GetChild(0).transform.GetChild(0);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = "Navigate to " + locationName + "?";
        RectTransform Pos = confScreen.GetComponent<RectTransform>();
        Pos.SetSiblingIndex(6);
        confScreen.SetActive(true);
      
        CoordinateData.locationName = gameObject.GetComponent<Coordinates>().locationName;
        CoordinateData.latitude = gameObject.GetComponent<Coordinates>().latitude;
        CoordinateData.longitude = gameObject.GetComponent<Coordinates>().longitude;
        CoordinateData.id = gameObject.GetComponent<Coordinates>().id;
        CoordinateData.type = gameObject.GetComponent<Coordinates>().type;
        //Destination coordinates
        destinationLatitude = CoordinateData.latitude;
        destinationLongitude =CoordinateData.longitude;

    }

       
    /*public void showInformation()
    {
        if (destroy == true)
        {
            Destroy(gameObject.transform.GetChild(1).gameObject);
            destroy = false;
            return;
        }
       
        string locationName = gameObject.GetComponent<Coordinates>().locationName;
        string latitude = gameObject.GetComponent<Coordinates>().latitude;
        string longitude = gameObject.GetComponent<Coordinates>().longitude;

        Image img = Instantiate(image, new Vector3(x + 150, y, 0), Quaternion.identity);
        img.transform.SetParent(parent.transform, false);

        Text textOutput = Instantiate(textBox, new Vector3(x + 150, y, 0), Quaternion.identity);
        textOutput.transform.SetParent(img.transform, false);
        textOutput.text = "Name: " + locationName + "\n" + "Latitude: " + latitude + "\n" + "Longitude: " + longitude;

        destroy = true;
    }*/
}
