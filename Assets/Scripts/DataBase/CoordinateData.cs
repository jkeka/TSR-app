
public class CoordinateData
{
    // This class stores marker information to be used globally for navigation and description fetching

    // Data from selected marker is stored in these variables
    public static string locationName = null;
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
            "longitude : " + longitude + "\n" +
            "type : " + type;

        return s;
    }
}

