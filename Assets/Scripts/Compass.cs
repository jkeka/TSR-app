using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public enum Space {two, three };
    public Space dimension;
    public Transform dTrans;        //move gameobject to target ship's location in relation to this gameobject



    //get location
    public GPSmanager gpsManager;


    //implement phone gyroscope

    private void Start()
    {


    }
    private void FixedUpdate()
    {
        switch (dimension)
        {
            case Space.two:
                
                Vector3 posvec=new Vector3(dTrans.position.x,dTrans.position.z,0);

                float angle = Mathf.Tan(dTrans.position.z / dTrans.position.x);

                transform.rotation=Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, posvec);

                break;
            case Space.three:
                transform.LookAt(dTrans.position, Vector3.up);

                break;
        
        }
    }
    
    private void GetDestination(string _latitude, string _longitude)
    {
        float latitude = float.Parse(_latitude);
        float longitude = float.Parse(_longitude);
        dTrans.position = new Vector3(longitude-gpsManager.longitude, 0, latitude - gpsManager.latitude);
    }
}
