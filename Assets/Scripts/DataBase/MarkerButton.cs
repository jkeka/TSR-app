using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerButton : MonoBehaviour
{
    public Text textBox;
    public Button button;
    public Image image;
    public GameObject parent;

    private bool destroy = false;
    private float x;
    private float y;

    private void Start()
    {
        x = gameObject.transform.localPosition.x;
        y = gameObject.transform.localPosition.y;

        button.onClick.AddListener(showInformation);
    }

    public void showInformation()
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
    }
}
