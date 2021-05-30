using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LibraryScript : MonoBehaviour
{
    public TextAsset purjelaivat;
    public TextAsset purjehdus;
    public TextAsset joutsen;

    public Dictionary<string, string> yearsDict = new Dictionary<string, string>();

    public List<Button> glossaryList = new List<Button>();
    public static List<Button> letterList = new List<Button>();
    public Button suuretPurjeLaivat;
    public Button purjehdusTunnissa;
    public Button suomenJoutsen;
    public Button keywordButton;
    public Button letterMarker;
    public Button letterButton;
    public Transform keywordPos;
    public Transform letterButtonPos;
    public GameObject glossaryScreen;
    public GameObject libraryScreen;

    
    void Start()
    {
        suuretPurjeLaivat.onClick.AddListener(delegate { LoadGlossary(purjelaivat.text); });
        purjehdusTunnissa.onClick.AddListener(delegate { LoadGlossary(purjehdus.text); });
        suomenJoutsen.onClick.AddListener(delegate { LoadGlossary(joutsen.text); });
    }

    private void LoadGlossary(string file)
    // Loads the selected glossary data on screen.
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

    public void ClearGlossary()
    // Clears glossary of the previous data.
    {

        for (int i = 0; i < glossaryList.Count; i++)
        {
            Destroy(glossaryList[i].gameObject);
        }

        glossaryList.Clear();
        letterList.Clear();
    }
}
