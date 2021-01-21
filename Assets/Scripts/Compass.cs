using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    

    public Vector3 destination; //testing

    private void FixedUpdate()
    {
        transform.LookAt(destination, Vector3.up);
    }
    
    private void GetDestination(string coordinates)
    {

    }
}
