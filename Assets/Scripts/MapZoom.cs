using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoom : MonoBehaviour
{
    public GameObject map;

    private Vector2 scaleChange;

    private float zoomOutMin = 1;
    private float zoomOutMax = 8;
    private float slowDownTime = 0.005f;

    void Start()
    {
        scaleChange = new Vector2(1f, 1f);
    }

    // Update is called once per frame
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


            float difference = currentMagnitude - prevMagnitude;
            Zoom(difference);

        }
        
    }

    void Zoom(float difference)
    {
        //float multiplier = Mathf.Clamp(difference * slowDownTime, 1, 10);
        map.transform.localScale = scaleChange * difference;
        Debug.Log(map.transform.localScale);
    }
}
