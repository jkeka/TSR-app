using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ARSceneManager : MonoBehaviour
{
    //Screens
    public GameObject NavScreen;
    public GameObject ARScreen;
    public GameObject QuizScreen;


    void Start()
    {
        NavScreen.SetActive(true);
        ARScreen.SetActive(false);
        QuizScreen.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
