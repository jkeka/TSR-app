using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;

public class DescriptionDataHandler : MonoBehaviour
{
    
    public static void LoadDescription(string id, string language)
    // Loads Ship Description data from the database.
    {
       
        FirebaseDatabase.DefaultInstance
            .GetReference("Descriptions").Child(id).Child(language)
            .ValueChanged += DescriptionValueChanged;

        FirebaseDatabase.DefaultInstance
           .GetReference("Descriptions").Child(id).Child(language)
           .ValueChanged -= DescriptionValueChanged;
    }

    static void DescriptionValueChanged(object sender, ValueChangedEventArgs args)
    //Listens to changes in the database and loads a new snapshot when data changes.
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        if (Description.shipInfo != null)
        {
            Description.shipInfo.Clear();
        }

        try
        {
            string[] separator = new string[] { "\n\n" };

            string s = snapshot.Child("description").Value.ToString();
            Description.shipInfo = s.Split(separator, StringSplitOptions.None).ToList();
            string lastLine = s.Split(separator, StringSplitOptions.None).Last();
            Description.shipInfo.Add(lastLine);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Following error when loading description: " + e.Message);
        }
        catch (ArgumentException e)
        {
            Debug.Log("Following error when creating a location marker: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("Following error when loading description: " + e.Message);
        }

    }
}
