using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;


public class LocationDataHandler : MonoBehaviour 
{
    public Button locationMarker;
    public GameObject mapScreen;
    public static List<Button> markerList = new List<Button>();

    DatabaseReference reference;


    private void Start()
    {

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        LoadMarkers();
    }

    public void LoadMarkers()
    // Loads location data form the database.
    {

        FirebaseDatabase.DefaultInstance
            .GetReference("Location")
            .ValueChanged += MarkerValueChanged;
    }

    void MarkerValueChanged(object sender, ValueChangedEventArgs args)
    //Listens to changes in the database and loads a new snapshot when data changes.
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        if (markerList != null)
        {
            DestroyMarkers();
        }

        foreach (DataSnapshot i in snapshot.Children)
        {
            CreateMarker(i.Key, i.Child("Latitude").Value.ToString(), i.Child("Longitude").Value.ToString(),
                i.Child("type").Value.ToString(), i.Child("id").Value.ToString());

        }
    }

    public void CreateMarker(string name, string latitude, string longitude, string type, string id)
    // Creates markers on on the map screen based on the database coordinates. Attaches the location name and coordinates fetched.
    {
        
        float floatLat = float.Parse(latitude, CultureInfo.InvariantCulture);
        float floatLon = float.Parse(longitude, CultureInfo.InvariantCulture);

        float roundLat = Mathf.Round(floatLat * 1000f) / 1000f;
        float roundLon = Mathf.Round(floatLon * 1000f) / 1000f;

        float latConverted = ConvertLocationY(floatLat);
        float lonConverted = ConvertLocationX(floatLon);

        Button marker = Instantiate(locationMarker, new Vector3(lonConverted, latConverted, 0), Quaternion.identity) as Button;

        marker.GetComponent<Coordinates>().locationName = name;
        marker.GetComponent<Coordinates>().latitude = floatLat;
        marker.GetComponent<Coordinates>().longitude = floatLon;
        marker.GetComponent<Coordinates>().id = id;
        marker.GetComponent<Coordinates>().type = type;


        if (type.Equals("venue"))
        {
            marker.GetComponent<Image>().color = Color.green;
        }
        else if (type.Equals("booth"))
        {
            marker.GetComponent<Image>().color = Color.yellow;
        }

        marker.transform.SetParent(mapScreen.transform, false);

        markerList.Add(marker);

        float roundLatConv = Mathf.Round(latConverted * 1000f) / 1000f;
        float roundLonConv = Mathf.Round(lonConverted * 1000f) / 1000f;
    }



    public void DestroyMarkers()
    // Destroys existing markers on the map screen any time the location data changes in the database.
    {

        for (int i = 0; i < markerList.Count; i++)
        {
            Destroy(markerList[i].gameObject);
        }

        markerList.Clear();

    }

    public float ConvertLocationX(float longitude)
    {

        float startOffsetX = -2200f;

        float startOffsetGPSX = 22.211355f;

        //float mapWidthGps = 0.053123f;

        //float mapWidth = 4400f;

        float widthUnit = 0.000012073f;
            //mapWidthGps / mapWidth;

        float markerTempWidth = longitude - startOffsetGPSX;

        float markerPositionX = startOffsetX + (markerTempWidth / widthUnit);

        return markerPositionX;
    }

    public float ConvertLocationY(float latitude)
    {

        float startOffsetY = -1575f;

        float startOffsetGPSY = 60.429646f;

        //float mapHeigthGps = 0.018804f;

        //float mapHeigth = 3150f;

        float heigthUnit = 0.00000597f;
            //mapHeigthGps / mapHeigth;

        float markerTempHeigth = latitude - startOffsetGPSY;

        float markerPositionY = startOffsetY + (markerTempHeigth / heigthUnit);

        return markerPositionY;
    }
}
