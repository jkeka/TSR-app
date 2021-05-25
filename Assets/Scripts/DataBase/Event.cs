using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public string id;
    public string venueId;
    public DateTime startTime;
    public DateTime endTime;
    public string eventName;
    public string eventDescription;
    
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
            "event description: " + eventName;

        return s;
    }

}
