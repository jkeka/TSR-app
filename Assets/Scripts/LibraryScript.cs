using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LibraryScript : MonoBehaviour
{
    public TextAsset joutsen;
    public TextAsset sailing;

    public Dictionary<string, string> yearsDict = new Dictionary<string, string>();

    public List<Button> glossaryList = new List<Button>();
    public static List<Button> letterList = new List<Button>();
    public Button suuretPurjeLaivat;
    public Button purjehdusTunnissa;
    public Button keywordButton;
    public Button letterMarker;
    public Button letterButton;
    public Transform keywordPos;
    public Transform letterButtonPos;
    public GameObject glossaryScreen;

    void Start()
    {
        suuretPurjeLaivat.onClick.AddListener(delegate { LoadGlossary(joutsen.text); });
        purjehdusTunnissa.onClick.AddListener(delegate { LoadGlossary(sailing.text); });
    }

    private void LoadGlossary(string file)
    // Loads the selected glossary data on screen.
    {

        if (glossaryList != null)
        {
            ClearGlossary();
        }

        RectTransform pos = glossaryScreen.GetComponent<RectTransform>();
        pos.SetSiblingIndex(11);
        Dictionary<string, string> glossary = JsonConvert.DeserializeObject<Dictionary<string, string>>(file);
       
        string previousKey = "*";
        bool years = false;
        
        foreach (KeyValuePair<string, string> entry in glossary)
        {
            if (entry.Key.StartsWith("1"))
            {
                if (years == false)
                {
                    Button letter = Instantiate(letterMarker, keywordPos);
                    letter.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "YEARS OF NOTE";
                    glossaryList.Add(letter);
                    years = true;
                }                                 
            }

            else if (!entry.Key.ToUpper().StartsWith(previousKey))
            {
                Button letterBut = Instantiate(letterButton, letterButtonPos);
                //letterBut.transform.localPosition = new Vector3(x, y, 0);
                //y = y - 110;
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
