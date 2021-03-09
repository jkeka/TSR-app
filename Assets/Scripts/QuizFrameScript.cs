using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizFrameScript : MonoBehaviour
{
    private Button[] buttonArray;

    /// <summary>
    /// placeholder: (Shipname,score)
    /// </summary>
    public Dictionary<string, int> shipScores = new Dictionary<string, int>();


    private void Start()
    {
        buttonArray = new Button[3];

        for (int i = 0; i <=2; i++)
        {
            buttonArray[i] = transform.GetChild(i + 1).GetComponent<Button>();
        }
        transform.GetChild(4).GetComponent<Button>().onClick.AddListener(OnNextQuestionClick);
        
        
        //SetQuestion("press two", 1, "one", "two", "three");
    }
    /// <summary>
    /// q text, correct answer's index 0-2, three answer texts, Example:
    /// SetQuestion("what's 2+2?", 1, "it's three", "it's four", "it's five");
    /// </summary>
    /// <param name="_question"></param>
    /// <param name="_correctIndex"></param>
    /// <param name="_answerOne"></param>
    /// <param name="_answerTwo"></param>
    /// <param name="_answerThree"></param>
    public void SetQuestion(string _question,int _correctIndex,string _answerOne, string _answerTwo, string _answerThree)
    {
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = _question;
        if (buttonArray.Length != 0)
        {
            buttonArray[0].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = _answerOne;
            buttonArray[1].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = _answerTwo;
            buttonArray[2].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = _answerThree;
        }
        else
            Debug.Log("ButtonArray.Length=0");

        for (int i = 0; i <= 2; i++)
        {
            if (i == _correctIndex)
                buttonArray[_correctIndex].onClick.AddListener(OnCorrectAnswer);
            else
                buttonArray[i].onClick.AddListener(OnWrongAnswer);
        }
    }


    private void OnCorrectAnswer()
    {
        Debug.Log("correct!");
        //calls for a new fetching of quiz data and reloads the new set of questions
        AddPoints();
    }
    private void OnWrongAnswer()
    {
        Debug.Log("wrong!");
    }
    private void OnNextQuestionClick()
    {
        Debug.Log("Next question clicked");
        //calls for a new fetching of quiz data and reloads the new set of questions
    }







    private void AddPoints()
    {
        //test
        //if (shipScores.ContainsKey("testString"))
        //    shipScores["testString"] += 1;
        //else
        //    shipScores.Add("testString", 1);
        //Debug.Log("testString" + shipScores["testString"]);


        if (shipScores.ContainsKey(CoordinateData.locationName))
        {
            shipScores[CoordinateData.locationName] += 1;
        }
        else
            shipScores.Add(CoordinateData.locationName, 1);

        Debug.Log(CoordinateData.locationName + shipScores[CoordinateData.locationName]);
    }
}
