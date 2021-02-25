using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public enum Space {two, three };
    public Space dimension;
    public Transform dTrans;        //move gameobject to target ship's location in relation to this gameobject
    private float latitude;
    private float longitude;


    //get location
    public GPSmanager gpsManager;

    //public static Compass Instance;
    //implement phone gyroscope

    private void Start()
    {
       // if (Instance == null)
        //    Instance = this;
       // else
       // Destroy(gameObject);

    }
    private void FixedUpdate()
    {
        dTrans.position = new Vector3(longitude - gpsManager.longitude, 0, latitude - gpsManager.latitude);

        switch (dimension)
        {
            case Space.two:
                
                Vector3 posvec=new Vector3(dTrans.position.x,dTrans.position.z,0);

                float angle = Mathf.Tan(dTrans.position.z / dTrans.position.x);

                //transform.rotation=Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, posvec);

                break;
            case Space.three:
                transform.LookAt(dTrans.position, Vector3.up);

                break;
        
        }

    }
    /// <summary>
    /// Tells the Compass the coordinates of the target
    /// </summary>
    /// <param name="_latitude"></param>
    /// <param name="_longitude"></param>
    private void GetDestination(float _latitude, float _longitude)
    {
        latitude =_latitude;
        longitude =_longitude;
    }
}
