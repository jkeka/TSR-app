using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserRotation : MonoBehaviour
{
    public Gyroscope gyro;
    public Text gyroText;
    public GameObject userMarkerView;

    void Start()
    {
        //Set up and enable the gyroscope (check your device has one)
        gyro = Input.gyro;
        gyro.enabled = true;
        /*
        //If phone has gyroscope, show user view direction
        if (gyro. enabled == true)
        {
            userMarkerView.SetActive(true);
        }

        //If phone has not gyroscope, hide user view direction

        if (gyro.enabled == false)
        {
            userMarkerView.SetActive(false);

        }
        */
    }

    //This is a legacy function, check out the UI section for other ways to create your UI
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, gyro.attitude.eulerAngles.z);

        gyroText.text = ("Gyro attitude " + gyro.attitude.eulerAngles.y);

    }
}
