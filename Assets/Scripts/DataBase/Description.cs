using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description 
{
    public static Dictionary<string, string> info = new Dictionary<string, string>();

    public static string ReturnString()
    {
        string a = "name: " + CoordinateData.locationName + "\n";

        foreach (KeyValuePair<string, string> entry in info)
        {
            string b = entry.Key + ": " + entry.Value + "\n";
            a = a + b;
        }
        return a;      
    }

    /*
    public static string id;
    public static string description;
    public static string owner;
    public static string builder;
    public static string launched;
    public static string length;
    public static string height;
    public static string depth;
    public static string draft;
    public static string tonnage;
    public static string speed;
    public static string shipType;
    public static string status;
    */

}
