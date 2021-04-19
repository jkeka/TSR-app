using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserRotation : MonoBehaviour
{
    Gyroscope m_Gyro;
    public Text gyroText;

    void Start()
    {
        //Set up and enable the gyroscope (check your device has one)
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;
    }

    //This is a legacy function, check out the UI section for other ways to create your UI
    void Update()
    {
        //gameObject.transform.rotation = m_Gyro.attitude;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, m_Gyro.attitude.eulerAngles.z);

        gyroText.text = ("Gyro attitude " + m_Gyro.attitude.eulerAngles.y);
        //gyroText.text = ("Gyro attitude " + -Input.compass.magneticHeading);
        /*
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + m_Gyro.rotationRate);
        GUI.Label(new Rect(500, 350, 200, 40), "Gyro attitude" + m_Gyro.attitude);
        GUI.Label(new Rect(500, 400, 200, 40), "Gyro enabled : " + m_Gyro.enabled);
    */
    }
}
