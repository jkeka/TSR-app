﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;

using Newtonsoft.Json;


public class EventDataHandler : MonoBehaviour

{
    public Button eventButton;
    public GameObject scheduleScreen;
    public TMPro.TMP_Dropdown dropdown;
    public static List<Button> scheduleList = new List<Button>();

    DatabaseReference reference;


    private void Start()
    {
     reference = FirebaseDatabase.DefaultInstance.RootReference;
     LoadScheduleData();
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

            CreateEvent(
               i.Child("id").Value.ToString(),
               i.Child("venueId").Value.ToString(),
               i.Child("startTime").Value.ToString(),
               i.Child("endTime").Value.ToString(),
               i.Child("translations").GetRawJsonValue());
        }

        scheduleList.Sort((x, y) => x.GetComponent<Event>().startTime.CompareTo(y.GetComponent<Event>().startTime));
        CheckEventDate();

    }

    public void CreateEvent(string id, string venueId, string startTime, string endTime, string translations)
    // Creates button for schedule menu which shows event name, start- and endtime.
    {
        Button schedule = Instantiate(eventButton);

        schedule.GetComponent<Event>().id = id;
        schedule.GetComponent<Event>().venueId = venueId;
        schedule.GetComponent<Event>().startTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(startTime) / 1000).LocalDateTime;
        schedule.GetComponent<Event>().endTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(endTime) / 1000).LocalDateTime;
        schedule.GetComponent<Event>().translations = translations;

        var start = schedule.GetComponent<Event>().startTime.ToShortTimeString();
        var end = schedule.GetComponent<Event>().endTime.ToShortTimeString();

        var translation = SetEventLanguage(translations);

        var child = schedule.transform.GetChild(0);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = translation["event"] + "  " + start + " - " + end;

        schedule.transform.SetParent(scheduleScreen.transform, false);
        scheduleList.Add(schedule);

    }

    public Dictionary<string, string> SetEventLanguage(string translations)
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

    public void CheckEventDate()
    // Checks if the event date matches the selected dropdown date.
    {
        var day = dropdown.options[dropdown.value].text;

        foreach (Button button in scheduleList)
        {
            Debug.Log("Hello");
            var eventDate = button.GetComponent<Event>().startTime.Day + "." + button.GetComponent<Event>().startTime.Month;

            

            if (eventDate == day)
            {
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }

    }
}
