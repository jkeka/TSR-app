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
    {      
        string locationName = this.gameObject.GetComponent<Coordinates>().locationName;
        var child = confScreen.transform.GetChild(3);
        child.GetComponent<Text>().text = "Navigate to " + locationName + "?";
        confScreen.SetActive(true);
    }

    public void FindConfScreen() {

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
