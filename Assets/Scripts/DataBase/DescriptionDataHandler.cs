using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class DescriptionDataHandler : MonoBehaviour
{
    // This class fetches description for ship from the database and formats it for reading

    // Contains description of ship cut into separate strings
    public static List<string> shipInfo;

    // Reference to Firebase database
    static DatabaseReference reference;
    
    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.GetReference("Descriptions");
    }

    // Attaches or removes listeners for the reference path
    public static void LoadDescription(string id, string language)
    {     
        reference.Child(id).Child(language).ValueChanged += DescriptionValueChanged;
        reference.Child(id).Child(language).ValueChanged -= DescriptionValueChanged;
    }

    //Listens to changes in the database and loads a new snapshot when data changes.
    static void DescriptionValueChanged(object sender, ValueChangedEventArgs args)
    
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        if (shipInfo != null)
        {
            shipInfo.Clear();
        }

        try
        {
            string[] separator = new string[] { "\n\n" };

            string s = snapshot.Child("description").Value.ToString();
            shipInfo = s.Split(separator, StringSplitOptions.None).ToList();
            string lastLine = s.Split(separator, StringSplitOptions.None).Last();
            shipInfo.Add(lastLine);
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
