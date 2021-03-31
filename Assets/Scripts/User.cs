using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class DBUser {
    public string deviceCode;
    public string language;
    public List<string> visitedLocations;

    // for new user
    public DBUser(string device) {
        this.deviceCode = device;
        this.language = "default";
        this.visitedLocations = new List<string>();
    }

    // for an old user
    public DBUser(string device, string lang, List<string> locsVisited) {
        this.deviceCode = device;
        this.language = lang;
        this.visitedLocations = locsVisited;
    }
    public string ToString() {
        string locations = "";
        visitedLocations.ForEach(item => locations += item);
        return this.deviceCode + ", " + this.language + ", " + locations;
    }
}

public static class User {
    public static string deviceCode;
    public static string language;
    public static DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    public static List<string> visitedLocations;

    public static string ToString()
    {
        string locations = "";
        visitedLocations.ForEach(item => locations += item);
        return deviceCode + ", " + language + ", " + locations;
    }
    public static void SetLanguage(string lang) {
        Debug.Log("Setting language");
        language = lang;
        DBUser tmp = new DBUser(deviceCode, lang, visitedLocations); 
        string json = JsonUtility.ToJson(tmp);
        reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json);
    }
    public static string GetLanguage() {
        return language;
    }

    public static void InitializeUser(string deviceCodeNow) // old CheckTheDatabaseForNewUser
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users").Child(deviceCodeNow)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Task faulted!");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Task success!");
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Value != null)
                    {
                        DBUser tmpUser = JsonUtility.FromJson<DBUser>(snapshot.GetRawJsonValue());
                        Debug.Log(snapshot.Value);
                        deviceCode = deviceCodeNow;
                        OldUser(tmpUser);
                    }
                    else
                    {
                        Debug.Log("forwarding to create user");
                        CreateUser(deviceCodeNow);
                    }
                }
                else
                {
                    Debug.Log("Something terrible happened during fetching data");
                }
        });
    }

    // Creates a new User if a device with particular device code isn't found from DB
    public static void CreateUser(string deviceCode)
    {
        Debug.Log("Create User");
        deviceCode = deviceCode;
        DBUser tmp = new DBUser(deviceCode);
        string json = JsonUtility.ToJson(tmp);
        Debug.Log(json);
        reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json);
    }

    public static void OldUser(DBUser tmpUser)
    {
        Debug.Log("Old User");
        Debug.Log(tmpUser.ToString());
        language = tmpUser.language;
        visitedLocations = tmpUser.visitedLocations;
    }

    public static void AddVisitedLocation(string newLocation)
    {
        Debug.Log("Adding Location");
        visitedLocations.Add(newLocation);
        DBUser tmp = new DBUser(deviceCode, language, visitedLocations);
        string json = JsonUtility.ToJson(tmp);
        reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json);
    }

    public static List<string> GetVisitedLocations()
    {
        return visitedLocations;
    }
}