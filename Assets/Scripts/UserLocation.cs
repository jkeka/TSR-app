using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLocation : MonoBehaviour
{
    private PanZoom panZoomScript;
    private GPSmanager gpsManagerScript;


    public GameObject userLocationMarker;

    float widthUnit;
    float heigthUnit;

    float mapWidthGps;
    float mapHeigthGps;

    float startOffsetX;
    float startOffsetGPSX;

    float startOffsetY;
    float startOffsetGPSY;

    float userTempWidth;
    float userTempHeigth;

    float userX;
    float userY;

    float userPositionX;
    float userPositionY;

    // Top right 60.45733392077009, 22.278142732388787
    // Top left 60.45733392077009, 22.224010346708628

    //Bot right 60.4306495777899, 22.278142732388787
    //Bot left 60.4306495777899, 22.224010346708628

    
    // Start is called before the first frame update
    void Start()
    {

        panZoomScript = FindObjectOfType<PanZoom>();
        gpsManagerScript = GameObject.Find("GPSmanager").GetComponent<GPSmanager>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void userLocation()
    {

        //float widthUnit = 0.000012073f;
        widthUnit = mapWidthGps / panZoomScript.mapWidth;

        //float heigthUnit = 0.00000597f;
        heigthUnit = mapHeigthGps / panZoomScript.mapHeigth;

        //float mapWidthGps = 0.054132385680159f;
        mapWidthGps = 22.278142732388787f - 22.224010346708628f;

        //float mapHeigthGps = 0.02668434298019f;
        mapHeigthGps = 60.45733392077009f - 60.4306495777899f;

        //float startOffsetX = -2200f;
        startOffsetX = -(panZoomScript.mapWidth / 2);

        startOffsetGPSX = 22.224010346708628f;

        //float startOffsetY = -1575f;
        startOffsetY = -(panZoomScript.mapHeigth / 2);

        startOffsetGPSY = 60.4306495777899f;



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

    public float ConvertLocationX(float longitude)
    {

        float mapWidth = panZoomScript.mapWidth;

        float markerTempWidth = longitude - startOffsetGPSX;

        float markerPositionX = startOffsetX + (markerTempWidth / widthUnit);

        return markerPositionX;
    }

    public float ConvertLocationY(float latitude)
    {

        float mapHeigth = panZoomScript.mapHeigth;

        float markerTempHeigth = latitude - startOffsetGPSY;

        float markerPositionY = startOffsetY + (markerTempHeigth / heigthUnit);

        return markerPositionY;
    }
}
