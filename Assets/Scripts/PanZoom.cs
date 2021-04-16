﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomMin = 100;
    public float zoomMax = 480;
    private float zoomSlow = 0.01f;

    public RectTransform map;
    public float mapWidth;
    public float mapHeigth;

    public float cameraSize;

    public float cameraWidth;
    public float cameraHeight;

    public float cameraPositionX;
    public float cameraPositionY;

    public float cameraBoundXRight;
    public float cameraBoundXLeft;
    public float cameraBoundYTop;
    public float cameraBoundYBot;

    // Start is called before the first frame update
    void Start()
    {
        zoomMin = 100;
        zoomMax = 480;

    }

    // Update is called once per frame
    void Update()
    {

        //Zooming and moving the camera

        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float curretnMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = curretnMagnitude - prevMagnitude;

            Zoom(difference);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));

        void Zoom(float increment)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMin, zoomMax);
        }

        //Boundaries

        cameraSize = Camera.main.orthographicSize;

        cameraHeight = cameraSize * 2;
        cameraWidth = cameraHeight * 0.5625f;

        mapHeigth = map.sizeDelta.y / 2;
        mapWidth = map.sizeDelta.x / 2;

        cameraPositionX = transform.position.x;
        cameraPositionY = transform.position.y;

        cameraBoundYTop = cameraPositionY + (cameraHeight / 2);
        cameraBoundYBot = (-cameraPositionY - (-cameraHeight / 2)) * -1;

        cameraBoundXRight = cameraPositionX + (cameraWidth / 2);
        cameraBoundXLeft = (-cameraPositionX - (-cameraWidth / 2)) * -1;


        if (cameraBoundYTop > mapHeigth)
        {
            //Debug.Log("OutOfBounds Y top");
            //Camera.main.orthographicSize = cameraSize - 1;
            transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        }
        else if ((cameraBoundYBot < -mapHeigth))
        {
            //Debug.Log("OutOfBounds Y bot");
            //Camera.main.orthographicSize = cameraSize - 1;
            transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);

        }

        if (cameraBoundXRight > mapWidth)
        {
            //Debug.Log("OutOfBounds X right");
            //Camera.main.orthographicSize = cameraSize - 1;
            transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);

        }
        else if ((cameraBoundXLeft < -mapWidth))
        {
            //Debug.Log("OutOfBounds x left");
            //Camera.main.orthographicSize = cameraSize - 1;
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);

        }

    }
}
