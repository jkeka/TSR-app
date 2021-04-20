using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ARSceneManager : MonoBehaviour
{

    public Button backButton;
    public Button qrButton;
    public Button virtualPassButton;

    public static ARSceneManager instance;
    public GameObject qrScannerScreen;
    public GameObject virtualPassScreen;
    public GameObject textPrefab;
    public List<GameObject> pooledTextPrefabs;
    public int poolSize;
    public bool shouldExpand;
  
  
    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        backButton.onClick.AddListener(ArSceneBackClick);
        qrButton.onClick.AddListener(QRscannerToggle);
        virtualPassButton.onClick.AddListener(VirtualPassToggle);

        pooledTextPrefabs = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            float y =142-(32 * i);
            GameObject obj = Instantiate(textPrefab);
            obj.SetActive(false);
            pooledTextPrefabs.Add(obj);
            obj.transform.SetParent(virtualPassScreen.transform.GetChild(0).GetChild(0).GetChild(0));
            obj.GetComponent<RectTransform>().anchoredPosition =new Vector3(2.21f,y,0);

        }
    }
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledTextPrefabs.Count; i++)
        {
            if (!pooledTextPrefabs[i].activeInHierarchy)
            {
                pooledTextPrefabs[i].gameObject.SetActive(true);
                return pooledTextPrefabs[i];
            }
        }
        if (shouldExpand)
        {
            GameObject obj = Instantiate(textPrefab);
            obj.SetActive(false);
            pooledTextPrefabs.Add(obj);
            return obj;
        }
        else
        return null;
    }

    public void ArSceneBackClick()
    {
        Debug.Log("returning from AR scene");
        Debug.Log("return to Map screen");
        SceneManager.LoadScene("MapScene");
    }

    public void QRscannerToggle()
    {
        string text;
        qrScannerScreen.SetActive(!qrScannerScreen.activeSelf);
        if (qrScannerScreen.activeSelf)
            text = "Back";
        else
            text = "QR-scan";

            qrButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

        virtualPassButton.gameObject.SetActive(!qrScannerScreen.activeSelf);

       // qrButton.gameObject.SetActive(!qrScannerScreen.activeSelf);
    }


    public void VirtualPassToggle()
    {
        virtualPassScreen.SetActive(!virtualPassScreen.activeSelf);
        string text;
        if (virtualPassScreen.activeSelf)
            text = "Back";
        else
            text = "Virtual Pass";

        virtualPassButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

        qrButton.gameObject.SetActive(!virtualPassScreen.activeSelf);

    }


    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        qrButton.onClick.RemoveAllListeners();
    }
}


