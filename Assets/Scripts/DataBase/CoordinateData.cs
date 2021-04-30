using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoordinateData
{
    public static string locationName;
    public static float latitude;
    public static float longitude;
    public static string id;
    public static string type;

    public override string ToString()
    {
        string s =
            "id : " + id + "\n" +
            "locationName: " + locationName + "\n" +
            "latitude : " + latitude + "\n" +
            "longitude : " + longitude + "\n";

        return s;
    }
}

