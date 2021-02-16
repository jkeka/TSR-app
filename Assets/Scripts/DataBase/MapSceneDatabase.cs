using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using Firebase.Extensions;

public class MapSceneDatabase : MonoBehaviour
{
    public Button locationMarker;
    public Button scheduleButton;
    public GameObject mapScreen;
    List<Button> markerList = new List<Button>();
    DatabaseReference reference;
    DataSnapshot snapshot;


    void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //CheckDependencies();

    }

    void Start()
    {
        LoadMarkers();
        //LoadScheduleData();
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
        snapshot = args.Snapshot;

        if (markerList != null)
        {
            DestroyMarkers();
        }

        foreach (DataSnapshot i in snapshot.Children)
        {
            CreateMarker(i.Key, i.Child("Latitude").Value.ToString(), i.Child("Longitude").Value.ToString(),
				i.Child("type").Value.ToString());

        }
    }

    public void CreateMarker(string name, string latitude, string longitude, string type)
    // Creates markers on on the map screen based on the database coordinates. Attaches the location name and coordinates fetched.
    {

        Button marker = Instantiate(locationMarker, new Vector3(float.Parse(latitude), float.Parse(longitude), 0), Quaternion.identity) as Button;

        marker.GetComponent<Coordinates>().locationName = name;
        marker.GetComponent<Coordinates>().latitude = latitude;
        marker.GetComponent<Coordinates>().longitude = longitude;
		
		if (type.Equals("venue")) {
			marker.GetComponent<Image>().color = Color.green;
		} else if (type.Equals("booth")) {
			marker.GetComponent<Image>().color = Color.yellow;
		}

        marker.transform.SetParent(mapScreen.transform, false);

        markerList.Add(marker);


    }

    public void DestroyMarkers()
     // Destroys existing markers on the map screen any time the location data changes in the database.
    {

        for(int i = 0; i < markerList.Count; i++)
        {
            Destroy(markerList[i].gameObject);
        }

        markerList.Clear();

    }

    public void LoadScheduleData()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Schedule")
            .Child("events")
            .ValueChanged += ScheduleValueChanged;
    }

    void ScheduleValueChanged(object sender, ValueChangedEventArgs args)
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

            CreateSchedule(
               i.Child("id").Value.ToString(),
               i.Child("startTime").Value.ToString(),
               i.Child("endTime").Value.ToString(),
               i.Child("latitude").Value.ToString(),
               i.Child("longitude").Value.ToString(),
               i.Child("address").Value.ToString(),
               i.Child("translations").GetRawJsonValue());

        }

    }

    public void CreateSchedule(string id, string startTime, string endTime, string latitude, string longitude, string address, string translations)
    {
        Button schedule = Instantiate(scheduleButton);

        schedule.GetComponent<Schedule>().id = id;
        schedule.GetComponent<Schedule>().startTime = startTime;
        schedule.GetComponent<Schedule>().endTime = endTime;
        schedule.GetComponent<Schedule>().latitude = latitude;
        schedule.GetComponent<Schedule>().longitude = longitude;
        schedule.GetComponent<Schedule>().address = address;
        //schedule.GetComponent<Schedule>().translations = translations

    }
}
