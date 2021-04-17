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

    private bool isGpsOn = false;

    //User location

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

        //Ask permission for location
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }

        //Calls the GPS at start
        StartCoroutine(Start());


    }

    void Update()
    {

        destinationLatitude = MarkerButton.destinationLatitude;
        destinationLongitude = MarkerButton.destinationLongitude;

        Debug.Log("GPS: Location: Lat: " + Input.location.lastData.latitude + " Lon: " + Input.location.lastData.longitude + " Alt: " + Input.location.lastData.altitude + " Horiz accur.: " + Input.location.lastData.horizontalAccuracy + " Timestamp: " + Input.location.lastData.timestamp);
        logText.text = ("GPS: Location: Lat: " + Input.location.lastData.latitude + " Lon: " + Input.location.lastData.longitude + " Alt: " + Input.location.lastData.altitude + " Horiz accur.: " + Input.location.lastData.horizontalAccuracy + " Timestamp: " + Input.location.lastData.timestamp);


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

            if (isGpsOn == true)
            {
                InvokeRepeating("UserLocation", 0, 3);
            }
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

        //User
        widthUnit = mapWidthGps / mapWidth;
        heigthUnit = mapHeigthGps / mapHeigth;


        userY = deviceLatitude;
        userX = deviceLongitude;

        userTempWidth = userX - startOffsetGPSX;
        userTempHeigth = userY - startOffsetGPSY;

        userPositionX = startOffsetX + (userTempWidth / widthUnit);
        userPositionY = startOffsetY + (userTempHeigth / heigthUnit);

        //Debug.Log("Device latitude: " + deviceLatitude);
        //Debug.Log("Device longitude: " + deviceLongitude);

        //Debug.Log("User Position X: " + userPositionX);
        //Debug.Log("User Position Y: " + userPositionY);

        Debug.Log("Fetching location");


        userLocationMarker.transform.localPosition = new Vector3(userPositionX, userPositionY, 0);

        //userPosText.text = ("User Position X: " + userPositionX + "            User Position Y: " + userPositionY);

    }

}
