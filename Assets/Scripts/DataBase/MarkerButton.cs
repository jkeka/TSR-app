using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MarkerButton : MonoBehaviour

{  
    public Button button;
    public GameObject confScreen;
    
    private void Awake()
    {
        FindConfScreen();
    }
    private void Start()
    {
        button.onClick.AddListener(MarkerClick);            
    }
    public void MarkerClick()
        //Sets confirmationscreen visible and writes text to confirmation textbox. Also adds the location data to Coordinatedata class

    {      
        string locationName = gameObject.GetComponent<Coordinates>().locationName;
        var child = confScreen.transform.GetChild(3);
        child.GetComponent<Text>().text = "Navigate to " + locationName + "?";
        confScreen.SetActive(true);


        CoordinateData.locationName = gameObject.GetComponent<Coordinates>().locationName;
        CoordinateData.latitude = gameObject.GetComponent<Coordinates>().latitude;
        CoordinateData.longitude = gameObject.GetComponent<Coordinates>().longitude;

    }

    public void FindConfScreen() {
        // Finds the ConfirmationScreen gameobject and places it in the confScreen variable

                                                                                    //Matti:
                                                                                    //SUGGESTION: make a public static instance SINGLETON of the mapscenemanager
                                                                                    // and store all references in that, all other scripts can find the references from that singleton with 
                                                                                    //t.ex: confScreen=MapSceneManager.instance.confirmationScreen;

                                                                                    //Can be made cheaper with:
                                                                                    //confScreen = GameObject.Find("ConfirmationScreen");

        GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        foreach (var i in objects)
        {
            if (i.name == "ConfirmationScreen")
            {
                confScreen = i.gameObject;
                break;
            }
        }
    }

    /*public void showInformation()
    {
        if (destroy == true)
        {
            Destroy(gameObject.transform.GetChild(1).gameObject);
            destroy = false;
            return;
        }
       
        string locationName = gameObject.GetComponent<Coordinates>().locationName;
        string latitude = gameObject.GetComponent<Coordinates>().latitude;
        string longitude = gameObject.GetComponent<Coordinates>().longitude;

        Image img = Instantiate(image, new Vector3(x + 150, y, 0), Quaternion.identity);
        img.transform.SetParent(parent.transform, false);

        Text textOutput = Instantiate(textBox, new Vector3(x + 150, y, 0), Quaternion.identity);
        textOutput.transform.SetParent(img.transform, false);
        textOutput.text = "Name: " + locationName + "\n" + "Latitude: " + latitude + "\n" + "Longitude: " + longitude;

        destroy = true;
    }*/
}
