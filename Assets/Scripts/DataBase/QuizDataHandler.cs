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
               Debug.Log("No quiz found");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;

               Quiz.id = id;
               Quiz.quizText = snapshot.GetRawJsonValue();
               Debug.Log(Quiz.quizText);            
           }
       });      
    }   
}
