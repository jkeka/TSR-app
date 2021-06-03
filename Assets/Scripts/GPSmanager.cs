using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPSmanager : MonoBehaviour
{
    // This class operates the GPS-based navigation system of the application

    // Device coordinates
    public float deviceLatitude;
    public float deviceLongitude;

    // Destination coordinates
    public float destinationLatitude;
    public float destinationLongitude;

    //User coordinates
    private float latitudeTolerance = 0.00039f;
    private float longitudeTolerance = 0.00012f;
    public float userX;
    public float userY;

    // Counter for Userlocation() function
    private int userLocationTimes = 0;

    // Check if GPS is on
    public bool isGpsOn = false;

    //Map size and position
    public RectTransform map;
    public float mapWidth;
    public float mapHeigth;

    // Reference to MapSceneManager script
    private MapSceneManager mapSceneManagerScript;

    // Text objects for debugging purposes
    public Text deviceCoordText;
    public Text logText;
    public Text userPosText;
    public Text debugText;
    public Text bearingText;

    // Compass parts
    public GameObject destinationPointer;
    public GameObject compassSimple;
    public GameObject compassBottom;

    // User location marker on the map
    public GameObject userLocationMarker;

    void Awake()
    {
        
        deviceLatitude = 99.9999f;
        deviceLongitude = 99.9999f;

        Input.compass.enabled = true;

        isGpsOn = false;
     
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
      
        //If user get near destination
        if (deviceLatitude > (destinationLatitude - latitudeTolerance) && deviceLatitude < (destinationLatitude + latitudeTolerance))
        {
            if (deviceLongitude > (destinationLongitude - longitudeTolerance) && deviceLongitude < (destinationLongitude + longitudeTolerance))
            {
                logText.text = "Paamaara saavutettu";
                mapSceneManagerScript.infoClick();

            }
        }
        
    }

    // Gets GPS location from phone
    IEnumerator GetLocation()
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            logText.text = ("GPS: Device location disabled");
            yield break;
        }


        // Start service before querying location
        Input.location.Start(5f, 5f); //Accuracy and update distance
        logText.text = ("GPS: Input.location started");

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            logText.text = ("GPS: Waiting location status");
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

            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            deviceLatitude = Input.location.lastData.latitude;
            deviceLongitude = Input.location.lastData.longitude;
            deviceCoordText.text = ("deviceLatitude " + deviceLatitude + " deviceLongitude " + deviceLongitude);
      
            UserLocation();

        }

        StartCoroutine(GetLocation());

    }

    // Calculates the angle between compass and destination
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


    // Moves the position of user marker on map
    public void UserLocation()
    {
        userLocationTimes++;

        debugText.text = ("UserLocation() called " + userLocationTimes + " times.");
        mapHeigth = map.sizeDelta.y;
        mapWidth = map.sizeDelta.x;

        userY = deviceLatitude;
        userX = deviceLongitude;

        Debug.Log("User Position X: " + userX);
        Debug.Log("User Position Y: " + userY);

        logText.text = ("UserLocation(): deviceLatitude " + userX + " deviceLongitude " + userY);

        float userLatConverted = ConvertUserLocationY(userY);
        float userLonConverted = ConvertUserLocationX(userX);

        userLocationMarker.transform.localPosition = new Vector3(userLonConverted, userLatConverted, 0);

        userPosText.text = ("Converted user Position X: " + userLonConverted + " User Position Y: " + userLatConverted);

    }

    // Converts the user longitude to X coordinates
    public float ConvertUserLocationX(float longitude)
    {

        float startOffsetX = -(mapWidth / 2);

        float startOffsetGPSX = 22.22401f;
      
        float mapWidthGps = 22.27814f - 22.22401f;
       
        float widthUnit = mapWidthGps / mapWidth;

        float userTempWidth = longitude - startOffsetGPSX;

        float userPositionX = startOffsetX + (userTempWidth / widthUnit);

        return userPositionX;
    }

    // Converts the user latitude to X coordinates
    public float ConvertUserLocationY(float latitude)
    {

        float startOffsetY = -(mapHeigth / 2);

        float startOffsetGPSY = 60.43064f;

        float mapHeigthGps = 60.45733f - 60.43064f;

        float heigthUnit = mapHeigthGps / mapHeigth;

        float userTempHeigth = latitude - startOffsetGPSY;

        float userPositionY = startOffsetY + (userTempHeigth / heigthUnit);

        return userPositionY;
    }

}
