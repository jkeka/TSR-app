using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

/**
* This file includes 3 different classes:
* - DBUser: used to add/fetch user data from Firebase DB
* - Location: used to add/fetch VisitedLocations of the user
* - User: used as an interface to control device based user's actions
*       in database
* 
*/

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

    // used for debugging
    public override string ToString() {
        string locations = "";
        visitedLocations.ForEach(item => locations += item);
        return this.deviceCode + ", " + this.language + ", " + locations;
    }
}


public class Location {
    public string id;
    public string name;
    public Location(string newId, string name) {
        this.id = newId;
        this.name = name;
    }
}

// Used to sort Location arrays
public class Locs : IEnumerable
{
    private Location[] _location;
    public Locs(Location[] pArray)
    {
        _location = new Location[pArray.Length];

        for (int i = 0; i < pArray.Length; i++)
        {
            _location[i] = pArray[i];
        }
    }

// Implementation for the GetEnumerator method.
    IEnumerator IEnumerable.GetEnumerator()
    {
       return (IEnumerator) GetEnumerator();
    }

    public LocsEnum GetEnumerator()
    {
        return new LocsEnum(_location);
    }
}

public class LocsEnum : IEnumerator
{
    public Location[] _location;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public LocsEnum(Location[] list)
    {
        _location = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _location.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public Location Current
    {
        get
        {
            try
            {
                return _location[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

/** 


Functions:
ToString() - returns deviceCode, set language and locations visited in a string
SetLanguage(string language) - sets the language with variables like 'fi', 'en'
GetLanguage() - returns language
InitializeUser(string deviceCode) 
  - logs/auths in to Firebase with the main user's username/password
  - checks if the user exists (CheckTheDatabaseForNewUser) in the database and if so, 
        gets the data, otherwise creates a new user to DB
AddVisitedLocation(string newLocation) - adds a location to VisitedLocations (List<string>)
  - returns a string of the added location (ship)
  - if the location is not found from db returns a string describing failure
GetVisitedLocations() - return visitedLocations as List<string> 
*/

public static class User {
    public static string deviceCode;
    public static string language;
    public static DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    public static List<string> visitedLocations;
    public static List<Location> locationArray = new List<Location>();
	public static bool readyToUse = false;
	public static bool initializing = false;
    

    public static void InitializeUser(string deviceCodeNow)
    {
		initializing = true;
        GetLocations();
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync("mainUser@hotmail.com", "FrustratinglyLongPassword%!7").ContinueWith(task => {
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
                        Debug.Log(snapshot.GetRawJsonValue());
                        Debug.Log(snapshot.Child("visitedLocations").GetRawJsonValue());

                        DBUser tmpUser = JsonUtility.FromJson<DBUser>(snapshot.GetRawJsonValue());
                        Debug.Log(tmpUser.deviceCode);
                        Debug.Log(tmpUser.language);
                        Debug.Log(tmpUser.visitedLocations.Count);
                        foreach( var x in tmpUser.visitedLocations) {
                            Debug.Log( x.ToString());
                        }
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
				readyToUse = true;
            }
        });
        
    }

    static void OldUser(DBUser tmpUser)
    {
        Debug.Log("Old User");
        Debug.Log(tmpUser.ToString());
        language = tmpUser.language;
        visitedLocations = tmpUser.visitedLocations;
		readyToUse = true;
    }

    public static string AddVisitedLocation(string newLocation)
    {   
        Debug.Log("Adding Location");
        foreach (string loc in visitedLocations) {
            if (loc.Equals(newLocation)) {
                Debug.Log("Location already visited");
                return "Location already visited";
            }
        }
        foreach (Location loc in locationArray) {
            if (loc.id == newLocation) {
                Debug.Log("Location id matches!");
                visitedLocations.Add(newLocation);
                DBUser tmp = new DBUser(deviceCode, language, visitedLocations);
                string json = JsonUtility.ToJson(tmp);

                reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json).ContinueWith((task) => { 
                    if(!task.IsFaulted && !task.IsCanceled) 
                    {
                        Debug.Log("Completed adding the location.");
                    } 
                        else 
                    {
                        Debug.Log("Adding location faulted.");
                        Debug.Log(task.Exception);
                    }

                });
                return loc.name;
            }
        }
        return "Location not found from database";
    }

    public static void GetLocations()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Location")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Value != null)
                    {
                        foreach (DataSnapshot ds in snapshot.Children) {
                            Location x = JsonUtility.FromJson<Location>(ds.GetRawJsonValue());
                            locationArray.Add(x);
                        }
                    }
                }
                else
                {
                    Debug.Log("Something terrible happened during fetching location data");
                }
        });
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
