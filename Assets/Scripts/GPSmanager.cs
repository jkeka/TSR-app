using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSmanager : MonoBehaviour
{
    public float latitude;
    public float longitude;

    public Text latitudeText;
    public Text longitudeText;
    public Text logText;
    //public Text locationText;

    /*
    public GameObject sigyn;
    public GameObject joutsen;

    
    void Awake()
    {
        sigyn.SetActive(false);
        joutsen.SetActive(false);
    }
    */

    void Update()
    {

        //Location checks
        //Arvot kasvaa pohjoiseen ja itään
        //Ehtolauseisiin aina suurempi arvo oikealle
        //Latitude = Pohjois-Etelä
        //Longitude = Länsi-itä

        /*
        //Joutsen
        if (latitude > 60.43945 && latitude < 60.44071)
        {
            if (longitude > 22.25252 && longitude < 22.25467)
            {
                locationText.text = "Location: Tiilentekija";
                joutsen.SetActive(true);
            }
        }
        else
        {
            joutsen.SetActive(false);

        }
        */
    }

    IEnumerator Start()
    {
        logText.text = "Log: StartGPS started";

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            logText.text = "Log: Device location disabled";
            yield break;
        }


        // Start service before querying location
        Input.location.Start(10, 10); //Default accuracy 10m
        logText.text = "Log: Input.location started";

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            logText.text = "Log: Waiting location status";
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            logText.text = "Log: Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            logText.text = "Log: Unable to determine device location";
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            latitudeText.text = latitude.ToString();
            longitudeText.text = longitude.ToString();
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
    
    //Buttons
    public void FetchGPS()
    {
        StartCoroutine(Start());
    }

    public void Quit()
    {

        Application.Quit();
    }
}
