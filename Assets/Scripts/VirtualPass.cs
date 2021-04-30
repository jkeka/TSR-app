using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPass : MonoBehaviour
{
     
    public Button button;
    public GameObject confScreen;
    private MapSceneManager mapSceneManagerScript;
    //private PanZoom panZoomScript;
    private float buttonSize = Mathf.Clamp(1f, 0.4f, 1.5f);
    private float cameraSizeMultiplier;


    public static float destinationLatitude;
    public static float destinationLongitude;

    private void Start()
    {
        confScreen = GameObject.Find("ConfirmationScreen");
        //mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();
        //panZoomScript = FindObjectOfType<PanZoom>();


        button.onClick.AddListener(ButtonClick);
    }

    private void Update()
    {
        //cameraSizeMultiplier = (Camera.main.orthographicSize * 0.0028f);
        //transform.localScale = new Vector3(buttonSize * cameraSizeMultiplier, buttonSize * cameraSizeMultiplier, buttonSize * cameraSizeMultiplier);
        //Debug.Log("Camera size from marker button script " + cameraSizeMultiplier);

    }

    public void ButtonClick()
    //Sets confirmationscreen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class

    {
        /*mapSceneManagerScript.screens.SetActive(true);
        for (int i = 0; i < mapSceneManagerScript.screenObjects.Count; i++)
        {
            mapSceneManagerScript.screenObjects[i].SetActive(false);
        }*/


        string locationName = gameObject.GetComponent<Coordinates>().locationName;
        var child = confScreen.transform.GetChild(0).transform.GetChild(0);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = "Matkataanko kohteeseen " + locationName + "?";
        RectTransform Pos = confScreen.GetComponent<RectTransform>();
        Pos.SetSiblingIndex(MapSceneManager.siblingIndex);
        confScreen.SetActive(true);

        CoordinateData.locationName = gameObject.GetComponent<Coordinates>().locationName;
        CoordinateData.latitude = gameObject.GetComponent<Coordinates>().latitude;
        CoordinateData.longitude = gameObject.GetComponent<Coordinates>().longitude;
        CoordinateData.id = gameObject.GetComponent<Coordinates>().id;
        CoordinateData.type = gameObject.GetComponent<Coordinates>().type;
        //Destination coordinates
        destinationLatitude = CoordinateData.latitude;
        destinationLongitude = CoordinateData.longitude;

    }
}
