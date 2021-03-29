using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapZoom : MonoBehaviour
{
    //public GameObject map;

    public float currentScale;

    private float zoomMin = 1;
    private float zoomMax = 6;
    private float slowDownTime = 0.005f;

    public float difference;

    public Image map;

    void Start()
    {
        currentScale = 1;

        //scaleChange = new Vector2(1f, 1f);
    }
    /*
    // Update is called once per frame
    void Update()
    {
        

        
    }

    void Zoom(float difference)
    {
        //float multiplier = Mathf.Clamp(difference * slowDownTime, 1, 10);
        map.transform.localScale = scaleChange * difference;
        Debug.Log(map.transform.localScale);
    }

    */
    void Update()
    {


        //Two fingers on screen
        if (Input.touchCount == 2)
        {
            //logText.text = ("Two fingers on screen");
            Touch touchFirst = Input.GetTouch(0);
            Touch touchSecond = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = touchFirst.position - touchFirst.deltaPosition;
            Vector2 secondTouchPrevPos = touchSecond.position - touchSecond.deltaPosition;

            Debug.Log("touchFirstPos " + touchFirst.position);
            Debug.Log("touchSecondPos " + touchSecond.position);



            float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float currentMagnitude = (touchFirst.position - touchSecond.position).magnitude;


            Debug.Log("prevMagnitude " + prevMagnitude);
            Debug.Log("currentMagnitude " + currentMagnitude);


            difference = currentMagnitude - prevMagnitude;

            Zoom(difference * slowDownTime);

        }


        //Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        currentScale += increment;
        if (currentScale >= zoomMax)
        {
            currentScale = zoomMax;
        }
        else if (currentScale <= zoomMin)
        {
            currentScale = zoomMin;
        }
        map.rectTransform.localScale = new Vector2(currentScale, currentScale);
    }

}
