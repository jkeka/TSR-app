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
    // Loads quiz data from the database.
    {

        FirebaseDatabase.DefaultInstance
            .GetReference("Quiz").Child(id).Child(language)
            .ValueChanged += QuizzValueChanged;

        FirebaseDatabase.DefaultInstance
           .GetReference("Quiz").Child(id).Child(language)
           .ValueChanged -= QuizzValueChanged;
    }

    static void QuizzValueChanged(object sender, ValueChangedEventArgs args)
    //Listens to changes in the database and loads a new snapshot when data changes.
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        Quiz.id = snapshot.Key;
        Quiz.quizText = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(snapshot.GetRawJsonValue());
    } 
}
