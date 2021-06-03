using UnityEngine;
using UnityEngine.UI;

public class UserRotation : MonoBehaviour   
{
    // This class operates the rotation of the user on the map

    // Gyroscope of the phone
    public Gyroscope gyro;

    // Text for debugging purposes
    public Text gyroText;

    // Shows the user direction on the map
    public GameObject userMarkerView;

    void Start()
    {
        //Set up and enable the gyroscope (check your device has one)
        gyro = Input.gyro;
        gyro.enabled = true;
        
    }

    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, gyro.attitude.eulerAngles.z);

        gyroText.text = ("Gyro attitude " + gyro.attitude.eulerAngles.y);

    }
}
