﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPSmanager : MonoBehaviour
{
    //Device coordinates
    public float deviceLatitude;
    public float deviceLongitude;

    public float destinationLatitude;
    public float destinationLongitude;

    private int userLocationTimes = 0;

    public Text deviceCoordText;
    public Text logText;
    public Text userPosText;
    public Text debugText;
    public Text bearingText;

    public GameObject destinationPointer;
    public GameObject compassSimple;
    public GameObject compassBottom;
    public GameObject userLocationMarker;

    public bool isGpsOn = false;

    //User location

    private PanZoom panZoomScript;


    private float userPositionX;
    private float userPositionY;

    private float userTempWidth;
    private float userTempHeigth;

    private float latitudeTolerance = 0.00039f;
    private float longitudeTolerance = 0.00012f;

    public float userX;
    public float userY;

    public float markerX;
    public float markerY;

    //Map
    public RectTransform map;
    public float mapWidth;
    public float mapHeigth;


    private float startOffsetX;
    private float startOffsetY;

    private float startOffsetGPSX;
    private float startOffsetGPSY;

    private float widthUnit;
    private float heigthUnit;

    private float mapHeigthGps;
    private float mapWidthGps;

    //Gyroscope
    private UserRotation userRotationScript;
    Compass compass;

    //Permission
    GameObject dialog = null;

    private MapSceneManager mapSceneManagerScript;


    void Awake()
    {
        /*
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        */

        deviceLatitude = 99.9999f;
        deviceLongitude = 99.9999f;

        panZoomScript = FindObjectOfType<PanZoom>();
        userRotationScript = FindObjectOfType<UserRotation>();
        mapSceneManagerScript = FindObjectOfType<MapSceneManager>();

        //Compass
        Input.compass.enabled = true;

        isGpsOn = false;
        /*
        //Ask permission for location
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }
        */
        //Call gps
        StartCoroutine(GetLocation());



    }


    void Update()
    {

        destinationLatitude = MarkerButton.destinationLatitude;
        destinationLongitude = MarkerButton.destinationLongitude;


        //Rotating compass
        float magneticHeading = Input.compass.magneticHeading;
        float bearing = CalculateAngle(deviceLatitude, deviceLongitude, destinationLatitude, destinationLongitude);

        //Compass always points to north
        compassSimple.transform.rotation = Quaternion.Slerp(compassSimple.transform.rotation, Quaternion.Euler(0, 0, magneticHeading), 0.05f);

        //Pointer to destination
        destinationPointer.transform.rotation = Quaternion.Slerp(destinationPointer.transform.rotation, Quaternion.Euler(0, 0, magneticHeading + bearing), 0.05f);

        bearingText.text = ("magneticHeading: " + magneticHeading);


        //-----------------------------
        
        //If user get near destination
        if (deviceLatitude > (destinationLatitude - latitudeTolerance) && deviceLatitude < (destinationLatitude + latitudeTolerance))
        {
            if (deviceLongitude > (destinationLongitude - longitudeTolerance) && deviceLongitude < (destinationLongitude + longitudeTolerance))
            {
                logText.text = "Paamaara saavutettu";
                mapSceneManagerScript.infoClick();

            }
        }
        


        //Bruno tarkka 60.44118, 22.24897

        //Vasen ala 60.44112, 22.24883
        //Vasen yla 60.44124, 22.24883
        //Oikea yla 60.44124, 22.24922
        //Oikea ala 60.44112, 22.24922

        //22.1234... on länsi-itä-suunta eli latitude
        //60.1234... on pohjois-etelä-suunta eli longitude


    }
    IEnumerator GetLocation()
    {
        //logText.text = ("GPS: GPS under fetching");
        //userPosText.text = ("GPS: GPS under fetching");

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            //Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            logText.text = ("GPS: Device location disabled");
            //Debug.Log("GPS: Device location disabled");
            //yield return new WaitForSeconds(3);
            yield break;
        }


        // Start service before querying location
        Input.location.Start(5f, 5f); //Accuracy and update distance
        logText.text = ("GPS: Input.location started");
        //Debug.Log("GPS: Input.location started");

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            logText.text = ("GPS: Waiting location status");
            //Debug.Log("GPS: Waiting location status");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            logText.text = ("GPS: Timed out");
            //Debug.Log("GPS: Timed out");
            yield break;

        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            logText.text = ("GPS: Unable to determine device location");
            //Debug.Log("GPS: Unable to determine device location");

            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            //Debug.Log("GPS: Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            deviceLatitude = Input.location.lastData.latitude;
            deviceLongitude = Input.location.lastData.longitude;
            deviceCoordText.text = ("deviceLatitude " + deviceLatitude + " deviceLongitude " + deviceLongitude);
            //Debug.Log("deviceLatitude " + deviceLatitude + " deviceLongitude " + deviceLongitude);

            UserLocation();

        }

        StartCoroutine(GetLocation());

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }


    private float CalculateAngle(float deviceLatitude, float deviceLongitude, float destinationLatitude, float destinationLongitude)
    {

        //Convert to radians
        deviceLatitude = deviceLatitude * Mathf.Deg2Rad;
        deviceLongitude = deviceLongitude * Mathf.Deg2Rad;

        destinationLatitude = destinationLatitude * Mathf.Deg2Rad;
        destinationLongitude = destinationLongitude * Mathf.Deg2Rad;
        
        //Calculate angle
        float dLon = (destinationLongitude - deviceLongitude);
        float y = Mathf.Sin(dLon) * Mathf.Cos(destinationLatitude);
        float x = (Mathf.Cos(deviceLatitude) * Mathf.Sin(destinationLatitude)) - (Mathf.Sin(deviceLatitude) * Mathf.Cos(destinationLatitude) * Mathf.Cos(dLon));

        float brng = Mathf.Atan2(y, x);
        brng = Mathf.Rad2Deg * brng;
        brng = (brng + 360) % 360;
        brng = 360 - brng;
        return brng;
    }



    public void UserLocation()
    {
        userLocationTimes++;

        debugText.text = ("UserLocation() called " + userLocationTimes + " times.");
        mapHeigth = map.sizeDelta.y;
        mapWidth = map.sizeDelta.x;

        userY = deviceLatitude;
        userX = deviceLongitude;

        //userY = 60.43966f;
        //userX = 22.25441f;

        Debug.Log("User Position X: " + userX);
        Debug.Log("User Position Y: " + userY);

        logText.text = ("UserLocation(): deviceLatitude " + userX + " deviceLongitude " + userY);


        float userLatConverted = ConvertUserLocationY(userY);
        float userLonConverted = ConvertUserLocationX(userX);

        //Debug.Log("userLonConverted: " + userLonConverted);
        //Debug.Log("userLatConverted: " + userLatConverted);

        //Debug.Log("mapHeigth gps " + mapHeigth);
        //Debug.Log("mapWidth gps " + mapWidth);

        userLocationMarker.transform.localPosition = new Vector3(userLonConverted, userLatConverted, 0);

        userPosText.text = ("Converted user Position X: " + userLonConverted + " User Position Y: " + userLatConverted);

        // Top right 60.45733392077009, 22.278142732388787
        // Top left 60.45733392077009, 22.224010346708628

        //Bot right 60.4306495777899, 22.278142732388787
        //Bot left 60.4306495777899, 22.224010346708628


    }
    public float ConvertUserLocationX(float longitude)
    {

        //float startOffsetX = -2200f;
        float startOffsetX = -(mapWidth / 2);

        //Debug.Log("startOffsetX: " + startOffsetX);
        //Debug.Log("mapWidth gps: " + mapWidth);


        float startOffsetGPSX = 22.22401f;

        //float mapWidthGps = 0.054132385680159f;
        float mapWidthGps = 22.27814f - 22.22401f;


        //float widthUnit = 0.000012073f;
        float widthUnit = mapWidthGps / mapWidth;

        //Debug.Log("widthUnit: " + widthUnit);


        float userTempWidth = longitude - startOffsetGPSX;

        //Debug.Log("userTempWidth: " + userTempWidth);


        float userPositionX = startOffsetX + (userTempWidth / widthUnit);

        //Debug.Log("userPositionX: " + userPositionX);

        return userPositionX;
    }

    public float ConvertUserLocationY(float latitude)
    {

        //float startOffsetY = -1575f;
        float startOffsetY = -(mapHeigth / 2);

        float startOffsetGPSY = 60.43064f;

        //float mapHeigthGps = 0.02668434298019f;
        float mapHeigthGps = 60.45733f - 60.43064f;


        //float heigthUnit = 0.00000597f;
        float heigthUnit = mapHeigthGps / mapHeigth;

        float userTempHeigth = latitude - startOffsetGPSY;

        float userPositionY = startOffsetY + (userTempHeigth / heigthUnit);

        //Debug.Log("userPositionY: " + userPositionY);


        return userPositionY;
    }



}