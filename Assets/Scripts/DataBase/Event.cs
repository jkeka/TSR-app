using System;
using UnityEngine;

public class Event : MonoBehaviour
{
    // This class stores database data for events

    // Data for event. VenueId is the id of venue where the event takes place
    public string id;
    public string venueId;
    public string eventName;
    public string eventDescription;

    // Start- and endtime of event
    public DateTime startTime;
    public DateTime endTime;

    public Event(string id, string venueId, DateTime startTime, DateTime endTime, string eventName, string eventDescription)
    {
        this.id = id;
        this.venueId = venueId;
        this.startTime = startTime;
        this.endTime = endTime;
        this.eventName = eventName;
        this.eventDescription = eventDescription;
    }

    public override string ToString()
    {
        string s =
            "id: " + id + "\n" +
            "venueId: " + venueId + "\n" +
            "startTime: " + startTime + "\n" +
            "endTime: " + endTime + "\n" +
            "event name: " + eventName + "\n" +
            "event description: " + eventDescription;

        return s;
    }
}
