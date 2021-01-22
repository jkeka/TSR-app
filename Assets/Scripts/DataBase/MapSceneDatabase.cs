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


    void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        CheckDependencies();
        
    }

    void Start()
    {     
        LoadMarkers();
    }

    void Update()
    {

    }

    public void CheckDependencies()
        // Checks dependencies of Firebase Unity SDK. Android Devices require latest version of Google Play Services.
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Dependencies are fine");
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void LoadMarkers()
        // Loads markers on the MapScreen.
    {

        FirebaseDatabase.DefaultInstance
            .GetReference("Ships")
            .ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
      //Listens to changes in the database and loads a new snapshot when data changes.
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
        // Creates markers on on the map screen based on the database coordinates. Attaches the location name and coordinates fetched.
    {
        GameObject marker = Instantiate(locationMarker, new Vector3(float.Parse(latitude), float.Parse(longitude), 0), Quaternion.identity) as GameObject;
        
        marker.GetComponent<Coordinates>().locationName = name;
        marker.GetComponent<Coordinates>().latitude = latitude;
        marker.GetComponent<Coordinates>().longitude = longitude;
        
        marker.transform.SetParent(mapScreen.transform, false);

        
    }
}
