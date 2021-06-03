using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class LocationDataHandler : MonoBehaviour
{
    //This class fetches location data from database and creates markers on the map screen based on that data

    // Marker buttons on the map
    public Button shipMarker;
    public Button infoMarker;
    public Button healthMarker;

    // Reference to gameobject where markers are put
    public GameObject mapScreen;

    // List where references to marker buttons are stored
    public static List<Button> markerList = new List<Button>();

    // Reference to Panzoom script
    private PanZoom panZoomScript;

    // Reference to Firebase database
    private DatabaseReference reference;

    private void Start()
    {
        panZoomScript = FindObjectOfType<PanZoom>();
        reference = FirebaseDatabase.DefaultInstance.GetReference("Location");
        reference.ValueChanged += MarkerValueChanged;
    }

    //Listens to changes in the database and loads a new snapshot when data changes.
    void MarkerValueChanged(object sender, ValueChangedEventArgs args)

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
            try
            {
                CreateMarker(
                    i.Key,
                    i.Child("Latitude").Value.ToString(),
                    i.Child("Longitude").Value.ToString(),
                    i.Child("type").Value.ToString(),
                    i.Child("id").Value.ToString());
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
            catch (ArgumentException e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
            catch (FormatException e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
        }
    }

    // Creates markers on on the map screen based on the database coordinates. Attaches the to the marker values fetched.
    public void CreateMarker(string name, string latitude, string longitude, string type, string id)

    {
        float floatLat = float.Parse(latitude, CultureInfo.InvariantCulture);
        float floatLon = float.Parse(longitude, CultureInfo.InvariantCulture);

        float roundLat = Mathf.Round(floatLat * 1000f) / 1000f;
        float roundLon = Mathf.Round(floatLon * 1000f) / 1000f;

        Debug.Log("FLoatLat " + floatLat);
        Debug.Log("FLoatLon " + floatLon);

        float latConverted = ConvertLocationY(floatLat);
        float lonConverted = ConvertLocationX(floatLon);

        Button marker = null;

        if (type.Equals("ship"))
        {
            marker = Instantiate(shipMarker, new Vector3(lonConverted, latConverted, 0), Quaternion.identity);
        }

        else
        {
            marker = Instantiate(infoMarker, new Vector3(lonConverted, latConverted, 0), Quaternion.identity);
        }

        marker.GetComponent<Coordinates>().locationName = name;
        marker.GetComponent<Coordinates>().latitude = floatLat;
        marker.GetComponent<Coordinates>().longitude = floatLon;
        marker.GetComponent<Coordinates>().id = id;
        marker.GetComponent<Coordinates>().type = type;

        marker.transform.SetParent(mapScreen.transform, false);

        markerList.Add(marker);

        float roundLatConv = Mathf.Round(latConverted * 1000f) / 1000f;
        float roundLonConv = Mathf.Round(lonConverted * 1000f) / 1000f;
    }

    // Destroys existing markers on the map screen any time the location data changes in the database.
    public void DestroyMarkers()
    {

        for (int i = 0; i < markerList.Count; i++)
        {
            Destroy(markerList[i].gameObject);
        }

        markerList.Clear();

    }

    // Converts the longitude of the marker to X coordinates on the map
    public float ConvertLocationX(float longitude)
    {

        float startOffsetX = -(panZoomScript.mapWidth / 2);

        float startOffsetGPSX = 22.22401f;

        float mapWidthGps = 22.27814f - 22.22401f;

        float mapWidth = panZoomScript.mapWidth;

        float widthUnit = mapWidthGps / panZoomScript.mapWidth;

        float markerTempWidth = longitude - startOffsetGPSX;

        float markerPositionX = startOffsetX + (markerTempWidth / widthUnit);

        return markerPositionX;
    }

    // Converts the latitude of the marker to Y coordinates on the map
    public float ConvertLocationY(float latitude)
    {
        float startOffsetY = -(panZoomScript.mapHeigth / 2);

        float startOffsetGPSY = 60.43064f;

        float mapHeigthGps = 60.45733f - 60.43064f;

        float mapHeigth = panZoomScript.mapHeigth;

        float heigthUnit = mapHeigthGps / panZoomScript.mapHeigth;

        float markerTempHeigth = latitude - startOffsetGPSY;

        float markerPositionY = startOffsetY + (markerTempHeigth / heigthUnit);

        return markerPositionY;
    }
}
