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


}
