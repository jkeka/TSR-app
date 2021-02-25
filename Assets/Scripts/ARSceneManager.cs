using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ARSceneManager : MonoBehaviour
{

    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(ArSceneBackClick);
    }


    void ArSceneBackClick()
    {
        Debug.Log("returning from AR scene");
        Debug.Log("return to Map screen");
        SceneManager.LoadScene("MapScene");
    }
}


