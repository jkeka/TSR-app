using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ARSceneManager : MonoBehaviour
{

    public Button backButton;
    public Button qrButton;
    public static ARSceneManager instance;
    public GameObject qrScannerScreen;

    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        backButton.onClick.AddListener(ArSceneBackClick);
        qrButton.onClick.AddListener(QRscannerToggle);
    }


    public void ArSceneBackClick()
    {
        Debug.Log("returning from AR scene");
        Debug.Log("return to Map screen");
        SceneManager.LoadScene("MapScene");
    }

    public void QRscannerToggle()
    {
        Debug.Log(qrScannerScreen.activeSelf+" Ashelf");
        Debug.Log(qrScannerScreen.activeInHierarchy + "hieromarcy");
        qrScannerScreen.SetActive(!qrScannerScreen.activeSelf);
    }
    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        qrButton.onClick.RemoveAllListeners();
    }
}


