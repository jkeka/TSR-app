using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class MapSceneDatabase : MonoBehaviour
{
    public GameObject locationMarker;
    public GameObject mapScreen;
    DatabaseReference reference;
    DataSnapshot snapshot;
    
    
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        LoadMarkers();
    }

    void Update()
    {

    }

    public void LoadMarkers()
    {

        FirebaseDatabase.DefaultInstance
            .GetReference("Ships")
            .ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        snapshot = args.Snapshot;
        foreach (DataSnapshot i in snapshot.Children)
        {
            CreateMarker(i.Key, i.Child("Latitude").Value.ToString(), i.Child("Longitude").Value.ToString());

        }
    }

    public void CreateMarker(string name, string latitude, string longitude)
    {
        GameObject marker = Instantiate(locationMarker, new Vector3(float.Parse(latitude), float.Parse(longitude), 0), Quaternion.identity) as GameObject;
        
        marker.GetComponent<Coordinates>().locationName = name;
        marker.GetComponent<Coordinates>().latitude = latitude;
        marker.GetComponent<Coordinates>().longitude = longitude;
        
        marker.transform.SetParent(mapScreen.transform, false);

        
    }
}
