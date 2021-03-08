using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationConversion : MonoBehaviour
{
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

    //Sigyn
    private float JoutsenX = 22.237032f;
    private float JoutsenY = 60.436393f;

    // Start is called before the first frame update
    void Start()
    {
        widthUnit =  mapWidthGps / mapWidth;
        heigthUnit = mapHeigthGps / mapHeigth;


        //Joutsen location
        tempWidth = JoutsenX - startOffsetGPSX;
        tempHeigth = JoutsenY - startOffsetGPSY;
        /*
        Debug.Log("startOffsetX: " + startOffsetGPSX);
        Debug.Log("startOffsetY: " + startOffsetGPSY);

        Debug.Log("tempWidth: " + sigynX);
        Debug.Log("tempHeigth: " + sigynY);

        Debug.Log("widthUnit: " + widthUnit);
        Debug.Log("heigthUnit: " + heigthUnit);
        */
        positionX = startOffsetX + (tempWidth / widthUnit);
        positionY = startOffsetY + (tempHeigth / heigthUnit);

        Debug.Log("Position X: " + positionX);
        Debug.Log("Position Y: " + positionY);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(positionX, positionY, 0);

    }
}
