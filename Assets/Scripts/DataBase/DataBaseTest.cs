using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;


public class DataBaseTest : MonoBehaviour
{
    public Text outputText;
    public InputField textInput;
    DatabaseReference reference;
    //string text = "";
    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame

    private void Update()
    {
      
    }

    public void SaveData()
    {
        reference.Child("Tests").Push().SetValueAsync(textInput.text.ToString());
    }


    public void LoadData()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("Testi")
        .ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        outputText.text = args.Snapshot.GetValue(true).ToString();
        // Do something with the data in args.Snapshot
    }

}
