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
    public override string ToString() {
        string locations = "";
        visitedLocations.ForEach(item => locations += item);
        return this.deviceCode + ", " + this.language + ", " + locations;
    }
}

/** 
Functions:
ToString() - returns deviceCode, set language and locations visited in a string
SetLanguage(string language) - sets the language with variables like 'fi', 'en'
GetLanguage() - returns language
InitializeUser(string deviceCode) 
  - checks if the user exists in the database and if so, gets the data, otherwise creates a new user to DB
AddVisitedLocation(string newLocation) - adds a location to VisitedLocations (List<string>)
GetVisitedLocations() - return visitedLocations as List<string> 
*/

public static class User {
    public static string deviceCode;
    public static string language;
    public static DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    public static List<string> visitedLocations;

    

    public static void InitializeUser(string deviceCodeNow)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            CheckTheDatabaseForNewUser(deviceCodeNow);
        });
        
    }

    public static void CheckTheDatabaseForNewUser(string deviceCodeNow) 
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
    static void CreateUser(string newDeviceCode)
    {
        Debug.Log("Create User");
        DBUser tmp = new DBUser(newDeviceCode);
        string json = JsonUtility.ToJson(tmp);
        deviceCode = newDeviceCode;
        visitedLocations = new List<string>();
        Debug.Log(json);
        reference.Child("Users").Child(newDeviceCode).SetRawJsonValueAsync(json).ContinueWith((task) => { 
            if(task.IsFaulted) {
                Debug.Log("Creating user faulted.");
                Debug.Log(task.Exception);
            }

            if(task.IsCanceled) {
                Debug.Log("Cancelled.");
            }

            if(!task.IsFaulted && !task.IsCanceled) {
                Debug.Log("Completed creating the user.");
            }
        });
        
    }

    static void OldUser(DBUser tmpUser)
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
        reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json).ContinueWith((task) => { 
            if(task.IsFaulted) {
                Debug.Log("Adding location faulted.");
                Debug.Log(task.Exception);
            }

            if(task.IsCanceled) {
                Debug.Log("Cancelled.");
            }

            if(!task.IsFaulted && !task.IsCanceled) {
                Debug.Log("Completed adding the location.");
            }
        });;
    }

    public static List<string> GetVisitedLocations()
    {
        return visitedLocations;
    }
    
    public new static string ToString()
    {
        string locations = "";
        visitedLocations.ForEach(item => locations += item);
        return deviceCode + ", " + language + ", " + locations;
    }
    public static void SetLanguage(string lang) {
        Debug.Log("Setting language");
        language = lang;
        DBUser tmp = new DBUser(deviceCode, lang, visitedLocations); 
        Debug.Log(deviceCode + lang + visitedLocations);
        Debug.Log(tmp.ToString());
        string json = JsonUtility.ToJson(tmp);
        reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json).ContinueWith((task) => { 
            if(task.IsFaulted) {
                Debug.Log("Setting language faulted.");
                Debug.Log(task.Exception);
            }

            if(task.IsCanceled) {
                Debug.Log("Cancelled.");
            }

            if(!task.IsFaulted && !task.IsCanceled) {
                Debug.Log("Completed setting language.");
            }
        });;
    }
    public static string GetLanguage() {
        return language;
    }
}
