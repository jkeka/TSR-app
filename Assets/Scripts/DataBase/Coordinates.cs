using UnityEngine;

public class Coordinates : MonoBehaviour
{
    // This class stores database data for marker information

    // Data for marker
    public string locationName;
    public float latitude;
    public float longitude;
    public string id;
    public string type;

    public Coordinates(string id, string locationName, float latitude, float longitude, string type)
    {
        this.id = id;
        this.locationName = locationName;
        this.latitude = latitude;
        this.longitude = longitude;
        this.type = type;
    }

    public override string ToString()
    {
        string s =
            "id: " + id + "\n" +
            "locationName: " + locationName + "\n" +
            "latitude: " + latitude + "\n" +
            "longitude: " + longitude + "\n" +
            "type: " + type;

        return s;
    }
}
