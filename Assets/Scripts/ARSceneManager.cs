using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ARSceneManager : MonoBehaviour
{

    public Button backButton;
    public static ARSceneManager instance;

    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        backButton.onClick.AddListener(ArSceneBackClick);
    }


    public void ArSceneBackClick()
    {
        Debug.Log("returning from AR scene");
        Debug.Log("return to Map screen");
        SceneManager.LoadScene("MapScene");
    }
}


