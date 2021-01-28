using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSmanager : MonoBehaviour
{
    public float latitude;
    public float longitude;

    private Text latitudeText;
    private Text longitudeText;
    private Text logText;


    void Awake()
    {
        //Calls the GPS at start
        StartCoroutine(Start());
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
        Input.location.Start(10, 10); //Default accuracy 10m
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
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            Debug.Log("Latitude " + latitude + " Longitude " + longitude);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
    

}
