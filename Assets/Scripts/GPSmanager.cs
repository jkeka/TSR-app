using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSmanager : MonoBehaviour
{
    //Device coordinates
    public float deviceLatitude;
    public float deviceLongitude;

    //Destination coordinates
    public float destinationLatitude;
    public float destinationLongitude;

    private Text deviceLatitudeText;
    private Text deviceLongitudeText;
    private Text logText;

    public GameObject compassSimple;

    private MapSceneDatabase mapSceneDatabaseScript;

    void Awake()
    {
        //Calls the GPS at start
        StartCoroutine(Start());

        mapSceneDatabaseScript = GameObject.Find("MapSceneDatabase").GetComponent<MapSceneDatabase>();

    }

    void Update()
    {
        //Rotating compass
        float bearing = CalculateAngle(deviceLatitude, deviceLongitude, destinationLatitude, destinationLongitude);
        compassSimple.transform.rotation = Quaternion.Slerp(compassSimple.transform.rotation, Quaternion.Euler(0, 0, Input.compass.magneticHeading + bearing), 100f);
    }

    IEnumerator Start()
    {
        Debug.Log("GPS: GPS started");

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS: Device location disabled");
            yield break;
        }


        // Start service before querying location
        Input.location.Start(1, 1); //Default accuracy 10m
        Debug.Log("GPS: Input.location started");

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("GPS: Waiting location status");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("GPS: Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("GPS: Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("GPS: Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            deviceLatitude = Input.location.lastData.latitude;
            deviceLongitude = Input.location.lastData.longitude;
            Debug.Log("deviceLatitude " + deviceLatitude + " deviceLongitude " + deviceLongitude);
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

}
