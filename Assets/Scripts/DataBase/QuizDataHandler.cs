using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;

using Newtonsoft.Json;

public class QuizDataHandler : MonoBehaviour
{
    DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public static void LoadQuizz(string id, string language)
        //Loads target quiz from the database.
    {
        FirebaseDatabase.DefaultInstance
       .GetReference("Quiz").Child(id).Child(language)
       .GetValueAsync().ContinueWith(task =>
       {
           if (task.IsFaulted)
           {
               Debug.Log("Something went wrong when fetching quizz");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;

               Quiz.id = id;
               Quiz.quizText = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(snapshot.GetRawJsonValue());

           }
       });

    }
/*
    public static void LoadQuizz(string id, string language)
    // Loads location data form the database.
    {

        FirebaseDatabase.DefaultInstance
            .GetReference("Quizz").Child(id).Child(language)
            .ValueChanged += QuizzValueChanged;

       FirebaseDatabase.DefaultInstance
           .GetReference("Quizz").Child(id).Child(language)
           .ValueChanged -= QuizzValueChanged;
    }

    public static void QuizzValueChanged(object sender, ValueChangedEventArgs args)
    //Listens to changes in the database and loads a new snapshot when data changes.
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        Quiz.id = snapshot.Key;

        string json = snapshot.GetRawJsonValue();

        //var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        foreach (var question in snapshot.Children)
        {


        }
    } 
*/
}
