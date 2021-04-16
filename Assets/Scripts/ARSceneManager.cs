using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ARSceneManager : MonoBehaviour
{

    public Button backButton;
    public Button qrButton;
    public Button otherButton;

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

        qrScannerScreen.SetActive(!qrScannerScreen.activeSelf);
        otherButton.gameObject.SetActive(!qrScannerScreen.activeSelf);
       // qrButton.gameObject.SetActive(!qrScannerScreen.activeSelf);
    }
    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        qrButton.onClick.RemoveAllListeners();
    }
}


