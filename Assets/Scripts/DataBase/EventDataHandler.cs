using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Newtonsoft.Json;


public class EventDataHandler : MonoBehaviour
{
    // This class fetches event data from database and creates events on the schedule screen based from that data

    // Variable to store information about used language
    private static string language = null;

    // Reference to prefab button for the schedule screen
    public Button eventButton;

    //Reference to the gameobject where the created events are put
    public GameObject events;

    // Reference to dropdown on the schedule screen
    public TMPro.TMP_Dropdown dropdown;

    // List where references to event buttons are stored
    public static List<Button> scheduleList = new List<Button>();

    // Reference to Firebase database
    DatabaseReference reference;


    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.GetReference("Schedule").Child("events");
    }

    // Attaches or removes listeners for the reference path
    public void LoadScheduleData()

    {
        reference.ValueChanged -= ScheduleValueChanged;
        reference.ValueChanged += ScheduleValueChanged;
    }

    // Listens to changes in the database and loads a new snapshot when data changes. 
    void ScheduleValueChanged(object sender, ValueChangedEventArgs args)

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
            try
            {
                CreateEvent(
                   i.Child("id").Value.ToString(),
                   i.Child("venueId").Value.ToString(),
                   i.Child("startTime").Value.ToString(),
                   i.Child("endTime").Value.ToString(),
                   i.Child("translations").GetRawJsonValue());
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Following error when creating an event: " + e.Message);
            }
            catch (ArgumentException e)
            {
                Debug.Log("Following error when creating a location marker:" + e.Message);
            }
            catch (FormatException e)
            {
                Debug.Log("Following error when creating a location marker:" + e.Message);
            }
            catch (Exception e)
            {
                Debug.Log("Following error when creating an event:" + e.Message);
            }
        }

        scheduleList.Sort((x, y) => x.GetComponent<Event>().startTime.CompareTo(y.GetComponent<Event>().startTime));
        CheckEventDate();

    }

    // Creates button for schedule menu which shows event name, start- and endtime.
    public void CreateEvent(string id, string venueId, string startTime, string endTime, string translations)

    {
        Button schedule = Instantiate(eventButton);

        schedule.GetComponent<Event>().id = id;
        schedule.GetComponent<Event>().venueId = venueId;
        schedule.GetComponent<Event>().startTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(startTime) / 1000).LocalDateTime;
        schedule.GetComponent<Event>().endTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(endTime) / 1000).LocalDateTime;

        var start = schedule.GetComponent<Event>().startTime.ToShortTimeString();
        var end = schedule.GetComponent<Event>().endTime.ToShortTimeString();

        SetEventLanguage(schedule, translations);

        var child = schedule.transform.GetChild(0);
        child.GetComponent<TMPro.TextMeshProUGUI>().text = schedule.GetComponent<Event>().eventName + "\n" + start + " - " + end;

        schedule.transform.SetParent(events.transform, false);
        scheduleList.Add(schedule);

    }

    // Sets schedule language based on user settings.
    public void SetEventLanguage(Button schedule, string translations)

    {
        Dictionary<string, Dictionary<string, string>> languages =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(Convert.ToString(translations));

        if (User.GetLanguage() != null)
        {
            language = User.GetLanguage();
        }
        else if (Application.systemLanguage == SystemLanguage.Finnish)
        {
            language = "fi";
        }
        else if (Application.systemLanguage == SystemLanguage.Swedish)
        {
            language = "se";
        }
        else
        {
            language = "en";
        }

        foreach (var item in languages)
        {
            if (language == item.Key)
            {
                var translation = item.Value;
                schedule.GetComponent<Event>().eventName = translation["event"];
                schedule.GetComponent<Event>().eventDescription = translation["desc"];
            }
        }
    }

    // Destroys existing schedule button on the schedule screen any time the schedule data changes in the database.
    public void DestroySchedule()
    {

        for (int i = 0; i < scheduleList.Count; i++)
        {
            Destroy(scheduleList[i].gameObject);
        }

        scheduleList.Clear();
    }

    // Checks if the event date matches the selected dropdown date.
    public void CheckEventDate()
    {
        var day = dropdown.options[dropdown.value].text;

        foreach (Button button in scheduleList)
        {
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
