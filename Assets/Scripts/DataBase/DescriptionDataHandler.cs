using System;
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
        /*if (CoordinateData.type != "ship")
        {
            return;
        }*/

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

        if (snapshot.Value == null)
        {
            return;
        }

        Description.info.Clear();

        foreach (DataSnapshot i in snapshot.Children)
        {
            Description.info.Add(i.Key, i.Value.ToString());  
            
        }

        Debug.Log(Description.ReturnString());
        /*
        ShipDescription.id = snapshot.Key;
        ShipDescription.description = snapshot.Child("description").ToString();
        ShipDescription.owner = snapshot.Child("owner").ToString();
        ShipDescription.builder = snapshot.Child("builder").ToString();
        ShipDescription.launched = snapshot.Child("launched").ToString();
        ShipDescription.length = snapshot.Child("lenght").ToString();
        ShipDescription.height = snapshot.Child("height").ToString();
        ShipDescription.depth = snapshot.Child("depth").ToString();
        ShipDescription.draft = snapshot.Child("draft").ToString();
        ShipDescription.tonnage = snapshot.Child("tonnage").ToString();
        ShipDescription.speed = snapshot.Child("speed").ToString();
        ShipDescription.shipType = snapshot.Child("shipType").ToString();
        ShipDescription.status = snapshot.Child("status").ToString();
        */
    }
}
