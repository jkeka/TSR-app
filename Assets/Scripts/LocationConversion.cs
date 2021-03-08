using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationConversion : MonoBehaviour
{
    public GameObject userLocationMarker;

    public GameObject joutsenLocationMarker;


    private float userPositionX;
    private float userPositionY;

    private float userTempWidth;
    private float userTempHeigth;

    public float userX;
    public float userY;

    //Values
    private float positionX;
    private float positionY;

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

    private float tempWidth;
    private float tempHeigth;

    //Example locations

    //Sigyn
    private float sigynX = 22.241855f;
    private float sigynY = 60.438521f;
    

    //Joutsen
    private float JoutsenX = 22.237032f;
    private float JoutsenY = 60.436393f;


    //Reference to GPS manager
    private GPSmanager gpsManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        gpsManagerScript = GameObject.Find("GPSmanager").GetComponent<GPSmanager>();

        widthUnit =  mapWidthGps / mapWidth;
        heigthUnit = mapHeigthGps / mapHeigth;


        //Joutsen location
        tempWidth = JoutsenX - startOffsetGPSX;
        tempHeigth = JoutsenY - startOffsetGPSY;

        positionX = startOffsetX + (tempWidth / widthUnit);
        positionY = startOffsetY + (tempHeigth / heigthUnit);

        Debug.Log("Position X: " + positionX);
        Debug.Log("Position Y: " + positionY);



    }

    // Update is called once per frame
    void Update()
    {
        joutsenLocationMarker.transform.localPosition = new Vector3(positionX, positionY, 0);

        //UserLocation





    }


    public void userLocation()
    {

        //User
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
}
