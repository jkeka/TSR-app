using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Glossary : MonoBehaviour
{
    private string[] paths = {"Assets/Data/joutsen.json", "Assets/Data/sailing.json" };
    public Button suuretPurjeLaivat;
    public Button purjehdusTunnissa;
    // Start is called before the first frame update
    void Start()
    {       
        suuretPurjeLaivat.onClick.AddListener(delegate { LoadGlossary(paths[0]); });
        purjehdusTunnissa.onClick.AddListener(delegate { LoadGlossary(paths[1]); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadGlossary(string path)
    // Loads the selected glossary data on screen
    {

        string s = "";

        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Dictionary<string, string> glossary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        reader.Close();

        foreach (KeyValuePair<string, string> entry in glossary)
        {
            s = s + entry.Key + ": " + entry.Value + "\n";
        }

        Debug.Log(s);
    }   
}
