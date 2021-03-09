using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationConversion : MonoBehaviour
{
    public Button userLocationMarker;
    public GameObject confScreen;

    /*
    private float userPositionX;
    private float userPositionY;

    private float userTempWidth;
    private float userTempHeigth;

    public float userX;
    public float userY;

    public float markerX;
    public float markerY;

    //Values

    private float startOffsetX = -2200f;
    private float startOffsetY = -1575f;

    private float startOffsetGPSX = 22.211355f;
    private float startOffsetGPSY = 60.429646f;

    private float widthUnit;
    private float heigthUnit;

    private float mapHeigthGps = 0.018804f;
    private float mapWidthGps = 0.053123f;

    private float mapHeigth = 3150f;
    private float mapWidth = 4400f;
    */
    //Reference to GPS manager
    private GPSmanager gpsManagerScript;

    void Start()
    {
        gpsManagerScript = GameObject.Find("GPSmanager").GetComponent<GPSmanager>();
        /*
        widthUnit =  mapWidthGps / mapWidth;
        heigthUnit = mapHeigthGps / mapHeigth;
        */
        confScreen = GameObject.Find("ConfirmationScreen");

        userLocationMarker.onClick.AddListener(ConfScreen);


    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public void userLocation()
    {

        //User
        widthUnit = mapWidthGps / mapWidth;
        heigthUnit = mapHeigthGps / mapHeigth;


        userX = gpsManagerScript.deviceLatitude;
        userY = gpsManagerScript.deviceLongitude;

        userTempWidth = userX - startOffsetGPSX;
        userTempHeigth = userY - startOffsetGPSY;

        userPositionX = startOffsetX + (userTempWidth / widthUnit);
        userPositionY = startOffsetY + (userTempHeigth / heigthUnit);

        Debug.Log("Device latitude: " + gpsManagerScript.deviceLatitude);
        Debug.Log("Device longitude: " + gpsManagerScript.deviceLongitude);

        Debug.Log("User Position X: " + userPositionX);
        Debug.Log("User Position Y: " + userPositionY);

        userLocationMarker.transform.localPosition = new Vector3(userPositionX, userPositionY, 0);

        gpsManagerScript.userPosText.text = ("User Position X: " + userPositionX + "            User Position Y: " + userPositionY);

    }

    */

    public void ConfScreen()
    {
        RectTransform Pos = confScreen.GetComponent<RectTransform>();
        Pos.SetSiblingIndex(5);
        confScreen.SetActive(true);
    }

}
