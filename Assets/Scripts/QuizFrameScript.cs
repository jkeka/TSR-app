using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizFrameScript : MonoBehaviour
{
    private Button[] buttonArray;
    public Button nextQuestion;
    public Button answer;
    private int index = 0;
    private int numberOfQuestions = 3;
    private Color deepBlue = new Color32(33, 43, 83, 255);
    private Color paleBlue = new Color32(81, 92, 124, 255);


    public GameObject summaryScreen;


    Image[] imgArray;
   
    private void Start()
    {
        buttonArray = new Button[3];

        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i] = transform.GetChild(i + 1).GetComponent<Button>();
        }

        nextQuestion = gameObject.transform.GetChild(4).GetComponent<Button>();

        GetQuizValues();

        if(CoordinateData.locationName!=null)
        QuizResults.shipScores.Add(CoordinateData.locationName, 0);
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

    private void GetQuizValues()
        // Gets target quiz values from quizText dictionary
    {
        int count = 0;

        try
        {
            foreach (var question in Quiz.quizText)
            {
                if (count == index)
                {
                    string q = question.Value["q"];
                    string a1 = question.Value["a1"];
                    string a2 = question.Value["a2"];
                    string a3 = question.Value["a3"];
                    int r = int.Parse(question.Value["r"]) - 1;

                    index++;
                    SetQuestion(q, r, a1, a2, a3);
                    return;

                }
                count++;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }

        //index++;                                                                 //test       
        //SetQuestion("kyssä", 1, "väärä", "oikea", "väärä");                     //test
    }

    public void SetQuestion(string _question, int _correctIndex, string _answerOne, string _answerTwo, string _answerThree)
        // Sets quizz values in UI and adds listener to the option buttons
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


        for (int i = 0; i < buttonArray.Length; i++)
        {
            int j = i;
            buttonArray[i].onClick.AddListener(delegate { checkAnswer(j, _correctIndex); });

        }
        
    }

    private void checkAnswer(int answer, int correctAnswer)
        // Checks if the answer user clicked is correct. Changes circle colours to show correct answer and makes nextQuestion button clickable.
    {
        imgArray  = new Image[2];
        imgArray[0] = buttonArray[answer].transform.GetChild(2).GetComponent<Image>();
        imgArray[1] = buttonArray[correctAnswer].transform.GetChild(2).GetComponent<Image>();


        Color ansCol =imgArray[0].color;
        Color corCol = imgArray[1].color; 
        
        if (answer == correctAnswer)
        {
            Debug.Log("correct!");
            ansCol = Color.green;
            ansCol.a = 0.4f;

            AddPoints();
        }
        else
        {
            Debug.Log("wrong!");

            corCol = Color.green;
            corCol.a = 0.4f;

            //imgArray[1].color = corCol;
            ChangeColor(1, corCol);
            ansCol = Color.red;
            ansCol.a = 0.4f;
        }
        ChangeColor(0, ansCol);
        //imgArray[0].color = ansCol;

        foreach (Button b in buttonArray)
        {
            b.onClick.RemoveAllListeners();
        }

        nextQuestion.onClick.AddListener(OnNextQuestionClick);
        nextQuestion.GetComponent<Image>().color = deepBlue;

    }

    void ChangeColor(int _i, Color _col)
    {
        if(imgArray.Length!=0)
        imgArray[_i].color = _col;
    }

    private void OnNextQuestionClick()
    //Calls for a new fetching of quiz data and reloads the new set of questions
    {
        Color resetCol = Color.white;
        resetCol.a = 0;
        ChangeColor(0, resetCol);
        ChangeColor(1, resetCol);

        foreach (Button b in buttonArray)
        {
            b.transform.GetChild(1).GetComponent<Image>().color = deepBlue;
        }

        GetQuizValues();
        nextQuestion.onClick.RemoveListener(OnNextQuestionClick);
        nextQuestion.GetComponent<Image>().color = paleBlue;

        Debug.Log(numberOfQuestions);

        if (index == numberOfQuestions)
        {
            nextQuestion.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Quiz";
            nextQuestion.onClick.AddListener(ShowSummary);
        }
        Debug.Log("Next question clicked");

    }
    private void ShowSummary()
    {
        nextQuestion.onClick.RemoveAllListeners();
        nextQuestion.onClick.AddListener(ARSceneManager.instance.ArSceneBackClick);

        for (int i = 0; i <= 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        
        summaryScreen.SetActive(true);

        //if (QuizResults.shipScores.ContainsKey("testString"))                                                                                               //test
           // summaryScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QuizResults.shipScores["testString"].ToString() + "/" + index;              //test

        if (QuizResults.shipScores.ContainsKey(CoordinateData.locationName))
            summaryScreen.transform.GetChild(0).GetComponent<TextMeshPro>().text= QuizResults.shipScores[CoordinateData.locationName].ToString() +"/"+ index;
            summaryScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You received a 4% discount code to the restaurant Rantakerttu";
    }

    private void AddPoints()
    {
        ////test
        //if (QuizResults.shipScores.ContainsKey("testString"))
        //    QuizResults.shipScores["testString"] += 1;
        //else
        //    QuizResults.shipScores.Add("testString", 1);
        //Debug.Log("testString" + QuizResults.shipScores["testString"]);

        if (CoordinateData.locationName != null)
        {
            if (QuizResults.shipScores.ContainsKey(CoordinateData.locationName))
            {
                QuizResults.shipScores[CoordinateData.locationName] += 1;
                Debug.Log(CoordinateData.locationName + QuizResults.shipScores[CoordinateData.locationName]);

            }
            else
                QuizResults.shipScores.Add(CoordinateData.locationName, 1);
        }
        else
            Debug.Log("no location name available");
    }


}
