using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using UnityEngine.UI;

using Newtonsoft.Json;


public class MapSceneDatabase : MonoBehaviour
{
    public Button locationMarker;
    public Button scheduleButton;
    public GameObject mapScreen;
    public GameObject scheduleScreen;
    List<Button> markerList = new List<Button>();
    List<Button> scheduleList = new List<Button>();
    DatabaseReference reference;


    void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //CheckDependencies();

    }

    void Start()
    {
        LoadMarkers();
        LoadScheduleData();
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

    public void LoadScheduleData()
    // Loads schedule data from the database.
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
        DataSnapshot snapshot = args.Snapshot;

        if (scheduleList != null)
        {
            DestroySchedule();
        }

        foreach (DataSnapshot i in snapshot.Children)
        {

            CreateSchedule(
               i.Child("id").Value.ToString(),
               i.Child("venueId").Value.ToString(),
               i.Child("startTime").Value.ToString(),
               i.Child("endTime").Value.ToString(),
               i.Child("translations").GetRawJsonValue());
        }

    }

    public void CreateSchedule(string id, string venueId, string startTime, string endTime, string translations)
    // Creates button for schedule menu which shows event name, start- and endtime.
    {
        Button schedule = Instantiate(scheduleButton);

        schedule.GetComponent<Schedule>().id = id;
        schedule.GetComponent<Schedule>().venueId = venueId;
        schedule.GetComponent<Schedule>().startTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(startTime) / 1000).LocalDateTime;
        schedule.GetComponent<Schedule>().endTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(endTime) / 1000).LocalDateTime;
        schedule.GetComponent<Schedule>().translations = translations;

        var start = schedule.GetComponent<Schedule>().startTime.ToShortTimeString();
        var end = schedule.GetComponent<Schedule>().endTime.ToShortTimeString();

        var translation = SetScheduleLanguage(translations);

        var child = schedule.transform.GetChild(0);
        child.GetComponent<Text>().text = translation["event"];

        child = schedule.transform.GetChild(1);
        child.GetComponent<Text>().text = start + " - " + end;

        scheduleList.Add(schedule);

        //schedule.transform.SetParent(scheduleScreen.transform, false);

    }

    public Dictionary<string, string> SetScheduleLanguage(string translations)
    // Sets schedule language based on user settings.
    {
        Dictionary<string, Dictionary<string, string>> languages =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(Convert.ToString(translations));

        string language = "fi";  // Needs to get the language from settings

        foreach (var item in languages)
        {
            if (language == item.Key)
            {
                var translation = item.Value;
                return translation;
            }
        }
        return null;
    }

    public void DestroySchedule()
    // Destroys existing schedule button on the schedule screen any time the schedule data changes in the database.
    {

        for (int i = 0; i < scheduleList.Count; i++)
        {
            Destroy(scheduleList[i].gameObject);
        }

        scheduleList.Clear();
    }

}