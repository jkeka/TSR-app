using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public string locationName;
    public string latitude;
    public string longitude;
    public string id;

    public Coordinates(string id, string locationName, string latitude, string longitude)
    {
        this.id = id;
        this.locationName = locationName;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public override string ToString()
    {
         string s =
            "id: " + id + "\n" +
            "locationName: " + locationName + "\n" +
            "latitude: " + latitude + "\n" +
            "longitude: " + longitude;

        return s;
    }
}
