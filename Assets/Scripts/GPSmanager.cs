using System.Collections;
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

    public Text deviceCoordText;
    public Text logText;
    public Text userPosText;

    public GameObject compassSimple;
    public GameObject userLocationMarker;

    public bool isGpsOn = false;

    //User location

    private PanZoom panZoomScript;


    private float userPositionX;
    private float userPositionY;

    private float userTempWidth;
    private float userTempHeigth;

    public float userX;
    public float userY;

    public float markerX;
    public float markerY;

    //Map
    public RectTransform map;
    public float mapWidth;
    public float mapHeigth;

    //Values
    /*
    private float startOffsetX = -2200f;
    private float startOffsetY = -1575f;

    private float startOffsetGPSX = 22.211355f;
    private float startOffsetGPSY = 60.429646f;
    */
    private float startOffsetX;
    private float startOffsetY;

    private float startOffsetGPSX;
    private float startOffsetGPSY;

    private float widthUnit;
    private float heigthUnit;

    private float mapHeigthGps;
    private float mapWidthGps;
    /*
    private float mapHeigthGps = 0.018804f;
    private float mapWidthGps = 0.053123f;

    private float mapHeigth = 3150f;
    private float mapWidth = 4400f;
    */

    //Permission
    GameObject dialog = null;

    void Awake()
    {

        //Kartan keskus
        //deviceLatitude = 22.2379165f;
        //deviceLongitude = 60.439048f;

        //Juani
        //deviceLatitude = 22.254176f;
        //deviceLongitude = 60.440105f;

        panZoomScript = FindObjectOfType<PanZoom>();



        isGpsOn = false;

        //Ask permission for location
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }

        //Calls the GPS at start
        StartCoroutine(Start());

        //UserLocation();

    }

    void Update()
    {

        destinationLatitude = MarkerButton.destinationLatitude;
        destinationLongitude = MarkerButton.destinationLongitude;

        
        if (isGpsOn == true)
        {
            UserLocation();

        }
        
        //Debug.Log("GPS: Location: Lat: " + Input.location.lastData.latitude + " Lon: " + Input.location.lastData.longitude + " Alt: " + Input.location.lastData.altitude + " Horiz accur.: " + Input.location.lastData.horizontalAccuracy + " Timestamp: " + Input.location.lastData.timestamp);
        //logText.text = ("GPS: Location: Lat: " + Input.location.lastData.latitude + " Lon: " + Input.location.lastData.longitude + " Alt: " + Input.location.lastData.altitude + " Horiz accur.: " + Input.location.lastData.horizontalAccuracy + " Timestamp: " + Input.location.lastData.timestamp);


        //Debug.Log("HaettuLat " + destinationLatitude + " HaettuLong " + destinationLongitude);

        //Rotating compass
        float bearing = CalculateAngle(deviceLatitude, deviceLongitude, destinationLatitude, destinationLongitude);
        compassSimple.transform.rotation = Quaternion.Slerp(compassSimple.transform.rotation, Quaternion.Euler(0, 0, Input.compass.magneticHeading + bearing), 100f);
    }

    IEnumerator Start()
    {
        //logText.text = ("GPS: GPS under fetching");
        //userPosText.text = ("GPS: GPS under fetching");

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            logText.text = ("GPS: Device location disabled");
            Debug.Log("GPS: Device location disabled");
            yield break;
        }


        // Start service before querying location
        Input.location.Start(); //Default accuracy 10m
        logText.text = ("GPS: Input.location started");
        Debug.Log("GPS: Input.location started");

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            logText.text = ("GPS: Waiting location status");
            Debug.Log("GPS: Waiting location status");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            logText.text = ("GPS: Timed out");
            Debug.Log("GPS: Timed out");
            yield break;

        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            logText.text = ("GPS: Unable to determine device location");
            Debug.Log("GPS: Unable to determine device location");

            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("GPS: Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            deviceLatitude = Input.location.lastData.latitude;
            deviceLongitude = Input.location.lastData.longitude;
            deviceCoordText.text = ("deviceLatitude " + deviceLatitude + " deviceLongitude " + deviceLongitude);
            Debug.Log("deviceLatitude " + deviceLatitude + " deviceLongitude " + deviceLongitude);

            isGpsOn = true;

        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
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

    public void FetchGPS()
    {
        StartCoroutine(Start());
    }


    public void UserLocation()
    {
        mapHeigth = map.sizeDelta.y;
        mapWidth = map.sizeDelta.x;

        userX = deviceLatitude;
        userY = deviceLongitude;

        //userY = 60.43966f;
        //userX = 22.25441f;

        Debug.Log("User Position X: " + userX);
        Debug.Log("User Position Y: " + userY);


        float userLatConverted = ConvertUserLocationY(userY);
        float userLonConverted = ConvertUserLocationX(userX);

        //Debug.Log("userLonConverted: " + userLonConverted);
        //Debug.Log("userLatConverted: " + userLatConverted);

        //Debug.Log("mapHeigth gps " + mapHeigth);
        //Debug.Log("mapWidth gps " + mapWidth);

        userLocationMarker.transform.localPosition = new Vector3(userLonConverted, userLatConverted, 0);

        userPosText.text = ("User Position X: " + userPositionX + " User Position Y: " + userPositionY);

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
