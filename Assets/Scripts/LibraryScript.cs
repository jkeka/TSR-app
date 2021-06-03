using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LibraryScript : MonoBehaviour
{
    // This script operates the building of the glossary for the glossary screen

    // References to JSON files containing the glossaries
    public TextAsset purjelaivat;
    public TextAsset purjehdus;
    public TextAsset joutsen;

    // Stores references to created keywords and letters
    public List<Button> glossaryList = new List<Button>();

    // Stores references to created letters for use in LetterButton script
    public static List<Button> letterList = new List<Button>();
    
    // References to buttons in the library screen
    public Button suuretPurjeLaivat;
    public Button purjehdusTunnissa;
    public Button suomenJoutsen;
    
    // References to glossary buttons: keywords, letters and letter markers
    public Button keywordButton;
    public Button letterMarker;
    public Button letterButton;
    
    //References to Positions of keyword and letter buttons in the glossary screen
    public Transform keywordPos;
    public Transform letterButtonPos;
    
    // References to glossary and library screen
    public GameObject glossaryScreen;
    public GameObject libraryScreen;

    
    void Start()
    {
        suuretPurjeLaivat.onClick.AddListener(delegate { LoadGlossary(purjelaivat.text); });
        purjehdusTunnissa.onClick.AddListener(delegate { LoadGlossary(purjehdus.text); });
        suomenJoutsen.onClick.AddListener(delegate { LoadGlossary(joutsen.text); });
    }

    // Loads the selected glossary data to the glossary screen
    public void LoadGlossary(string file)
    {

        if (glossaryList != null)
        {
            ClearGlossary();
        }

        RectTransform pos = glossaryScreen.GetComponent<RectTransform>();
        libraryScreen.SetActive(false);
        glossaryScreen.SetActive(true);
        pos.SetSiblingIndex(11);
        Dictionary<string, string> glossary = JsonConvert.DeserializeObject<Dictionary<string, string>>(file);

        string previousKey = "*";
        string pattern = "^[0-9]";
        
        foreach (KeyValuePair<string, string> entry in glossary)
        {
            Match match = Regex.Match(entry.Key, pattern, RegexOptions.None);
    
            if (!entry.Key.ToUpper().StartsWith(previousKey) && !match.Success)
            {
               
                Button letterBut = Instantiate(letterButton, letterButtonPos);
                glossaryList.Add(letterBut);

                letterBut.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = entry.Key[0].ToString().ToUpper();

                Button letter = Instantiate(letterMarker, keywordPos);
                letter.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = entry.Key[0].ToString().ToUpper();
                previousKey = entry.Key[0].ToString().ToUpper();
                glossaryList.Add(letter);
                letterList.Add(letter);
            }

            Button keyWord = Instantiate(keywordButton, keywordPos);
            keyWord.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = entry.Key;
            keyWord.GetComponent<KeyWord>().description = entry.Value;
            glossaryList.Add(keyWord);
        }

    }

    // Clears glossary of the previous data
    public void ClearGlossary()
    {

        for (int i = 0; i < glossaryList.Count; i++)
        {
            Destroy(glossaryList[i].gameObject);
        }

        glossaryList.Clear();
        letterList.Clear();
    }
}
