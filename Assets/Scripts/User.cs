using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;



class User {
    public string deviceCode;
    public string language;
    DatabaseReference reference;

    public User() {}
    public  User(string deviceCode) {
        this.deviceCode = deviceCode;
        this.language = "default";
    }
    public override string ToString()
    {
        return deviceCode + ", " + language;
    }
    public void setLanguage(string lang) {
      Debug.Log("set lang");
      this.language = lang;
      reference = FirebaseDatabase.DefaultInstance.RootReference;
      string json = JsonUtility.ToJson(this);
      reference.Child("Users").Child(this.deviceCode).SetRawJsonValueAsync(json);
    }
    public string getLanguage() {
      return this.language;
    }
}