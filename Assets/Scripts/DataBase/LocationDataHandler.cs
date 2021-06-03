using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;


public class LocationDataHandler : MonoBehaviour
{
    public Button shipMarker;
    public Button infoMarker;
    public Button healthMarker;
    public GameObject mapScreen;
    public static List<Button> markerList = new List<Button>();

    private PanZoom panZoomScript;

    DatabaseReference reference;


    private void Start()
    {
        panZoomScript = FindObjectOfType<PanZoom>();

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
            catch(ArgumentException e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
            catch(FormatException e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
            catch(Exception e)
            {
                Debug.Log("Following error when creating a location marker: " + e.Message);
            }
        }
    }

    public void CreateMarker(string name, string latitude, string longitude, string type, string id)
    // Creates markers on on the map screen based on the database coordinates. Attaches the location name and coordinates fetched.
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

        //float startOffsetX = -2200f;
        float startOffsetX = -(panZoomScript.mapWidth / 2);

        float startOffsetGPSX = 22.22401f;

        //float mapWidthGps = 0.054132385680159f;
        float mapWidthGps = 22.27814f - 22.22401f;

        float mapWidth = panZoomScript.mapWidth;

        //float widthUnit = 0.000012073f;
        float widthUnit = mapWidthGps / panZoomScript.mapWidth;


        float markerTempWidth = longitude - startOffsetGPSX;

        float markerPositionX = startOffsetX + (markerTempWidth / widthUnit);

        return markerPositionX;
    }

    public float ConvertLocationY(float latitude)
    {

        //float startOffsetY = -1575f;
        float startOffsetY = -(panZoomScript.mapHeigth / 2);

        float startOffsetGPSY = 60.43064f;

        // Top right 60.45733392077009, 22.278142732388787
        // Top left 60.45733392077009, 22.224010346708628

        //Bot right 60.4306495777899, 22.278142732388787
        //Bot left 60.4306495777899, 22.224010346708628


        //float mapHeigthGps = 0.02668434298019f;
        float mapHeigthGps = 60.45733f - 60.43064f;

        float mapHeigth = panZoomScript.mapHeigth;

        //float heigthUnit = 0.00000597f;
        float heigthUnit = mapHeigthGps / panZoomScript.mapHeigth;

        float markerTempHeigth = latitude - startOffsetGPSY;

        float markerPositionY = startOffsetY + (markerTempHeigth / heigthUnit);

        return markerPositionY;
    }
}
