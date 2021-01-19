using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public string locationName;
    public string latitude;
    public string longitude;

    public Coordinates(string locationName, string latitude, string longitude)
    {
        this.locationName = locationName;
        this.latitude = latitude;
        this.longitude = longitude;
    }
}
