﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using Firebase.Database;

using static User;

public class LoadingSceneManager : MonoBehaviour
{
    bool connectionEstablished = false;
    bool permissionsGranted = false;
    public GameObject errorMessage;
   
    // Start is called before the first frame update

    void Start()
    {
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);

        StartCoroutine(CheckConnection());
        CheckPermissions();

    }

    // Update is called once per frame
    void Update()
    {
        if (connectionEstablished == true && permissionsGranted == true)
        {
            
            if (!User.initializing) {
                Debug.Log("Initializing user");
                string deviceCode = SystemInfo.deviceUniqueIdentifier; // Replace with any string to test the db
                User.InitializeUser(deviceCode);
            }
            if (User.readyToUse) {
                Debug.Log("User ready, loading mapscene");
                SceneManager.LoadScene("MapScene");
            }
        }

    }

    IEnumerator CheckConnection()
    // Checks if connection is available. If not, displays a warning screen. Only creates user once connection is established.
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        //request.timeout = 2;
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            errorMessage.SetActive(true);
            errorMessage.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Error! Check internet connection!";
            Debug.Log("Error. Check internet connection!");

            bool connected = false;

            while (connected == false)
            {
                yield return new WaitForSecondsRealtime(2);
                request = new UnityWebRequest("http://google.com");
                yield return request.SendWebRequest();

                if (!request.isNetworkError)
                {
                    connected = true;
                    errorMessage.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Internet connection established!";
                    errorMessage.GetComponent<Image>().color = new Color32(71, 219, 69, 200);
                    yield return new WaitForSecondsRealtime(2);
                    errorMessage.SetActive(false);
                }
            }
        }
        Debug.Log("Connected to internet!");
        connectionEstablished = true;
    }

    public void CheckPermissions()
    // Checks permission for Android phone about camera and GPS Location
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) || !Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            StartCoroutine(AskPermissions());

        }

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            //CloseScreen();
            permissionsGranted = true;
            Debug.Log("All permissions granted!");
            
        }
            
    }

    IEnumerator AskPermissions()
    // Asks permission from Android phone to use camera and GPS Location 
    {
              
        //Camera permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            Debug.Log("Asking permission for camera");
        }

        yield return new WaitForSecondsRealtime(0.5f);

        //Location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            //Permission.RequestUserPermission(Permission.CoarseLocation);
            Debug.Log("Asking permission for GPS");

        }
        
        yield return new WaitForSecondsRealtime(0.5f);

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            permissionsGranted = true;
            Debug.Log("All permissions granted!");
        }
        else
        {
            Application.Quit();
        }
    } 
}
