using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class MapSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    //References
    public Text menuText;
    DatabaseReference reference;
    DataSnapshot snapshot;
    User currentUser;

    //Screens
    public GameObject HubScreen;
    public GameObject LangScreen;
    public GameObject MapScreen;
    public GameObject SetScreen;
    public GameObject SchedScreen;
    public GameObject ConfScreen;


    //Buttons

    //Language
    public Button FinnButton;
    public Button EngButton;

    //Navigation


    public Button HamburgerButton;
    public Button ScheduleButton;
    public Button SettingsButton;
    public Button MapButton;
    public Button SchedBackButton;
    public Button LangButton;
    public Button SetBackButton;
    public Button ConfYesButton;
    public Button ConfNoButton;

	void Awake()
	{
        string deviceCode = SystemInfo.deviceUniqueIdentifier; // Replace with any string to test the db
        print(deviceCode);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        CheckTheDatabaseForNewUser(deviceCode);
        
	}

    // Creates a new User if a device with particular device code wasn't found from DB
    void CreateUser(string deviceCode)
    {
        Debug.Log("Create User");
        User newUser = new User(deviceCode);
        string json = JsonUtility.ToJson(newUser);
        reference.Child("Users").Child(deviceCode).SetRawJsonValueAsync(json);
        currentUser = newUser;
    }


    void OldUser() 
    {
        Debug.Log("Old User");

    }

    void CheckTheDatabaseForNewUser(string deviceCode)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users").Child(deviceCode)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) 
                {
                    Debug.Log("Task faulted!");
                }
                else if (task.IsCompleted) 
                {
                    Debug.Log("Task success!");
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Value != null) {
                        currentUser = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                        Debug.Log(currentUser.ToString());
                        OldUser();
                    } else {
                        Debug.Log("forwarding to create user");
                        CreateUser(deviceCode);
                    }
                    
                    
                } else {
                    Debug.Log("Something terrible happened during fetching data");
                }
        });
    }

    void Start()
    {


        //Activates and deactivates screen at start
        LangScreen.SetActive(true);
        HubScreen.SetActive(false);
        MapScreen.SetActive(false);
        SetScreen.SetActive(false);
        SchedScreen.SetActive(false);
        ConfScreen.SetActive(false);
      

        //Language clicks
        FinnButton.onClick.AddListener(FinnClick);
        EngButton.onClick.AddListener(EngClick);

        //Navigation clicks
        HamburgerButton.onClick.AddListener(HambClick);
        ScheduleButton.onClick.AddListener(ScheduleClick);
        MapButton.onClick.AddListener(MapClick);
        SettingsButton.onClick.AddListener(SettingsClick);
        SchedBackButton.onClick.AddListener(SchedBackClick);
        SetBackButton.onClick.AddListener(SetBackClick);
        LangButton.onClick.AddListener(LangClick);
        ConfYesButton.onClick.AddListener(YesClick);
        ConfNoButton.onClick.AddListener(NoClick);

        //Destination clicks
        menuText.text = "Language";

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FinnClick()
    {
        Debug.Log("Set system to Finnish language!");
        currentUser.setLanguage("fi");

        LangScreen.SetActive(false);
        HubScreen.SetActive(true);
        
    }

    void EngClick()
    {
        Debug.Log("Set system to English language!");
        currentUser.setLanguage("en");

        LangScreen.SetActive(false);
        HubScreen.SetActive(true);
        
    }

    void HambClick()
    {
        Debug.Log("Hamburger clicked");
        HubScreen.SetActive(true);
        MapScreen.SetActive(false);
        SetScreen.SetActive(false);
        SchedScreen.SetActive(false);
        ConfScreen.SetActive(false);
        LangScreen.SetActive(false);
        menuText.text = "Menu";
    }

    void MapClick()
    {
        Debug.Log("Map clicked");
        MapScreen.SetActive(true);
        HubScreen.SetActive(false);
        menuText.text = "Map";
    }

    void ScheduleClick()
    {
        Debug.Log("Schedule clicked");
        SchedScreen.SetActive(true);
        menuText.text = "Schedule";
    }

    void SettingsClick()
    {
        Debug.Log("Settings clicked");
        SetScreen.SetActive(true);
        menuText.text = "Settings";
    }

    void SchedBackClick()
    {
        Debug.Log("Exit clicked, return to map screen");
        SchedScreen.SetActive(false);
        menuText.text = "Map";
    }

    void SetBackClick()
    {
        Debug.Log("Exit clicked, return to map screen");
        MapScreen.SetActive(true);
        LangScreen.SetActive(false);
        SetScreen.SetActive(false);
        SchedScreen.SetActive(false);
        menuText.text = "Map";
    }

    void LangClick()
    {
        Debug.Log("Language selection clicked");
        LangScreen.SetActive(true);
        SetScreen.SetActive(false);
        menuText.text = "Languages";
    }

    void YesClick()
    {
        Debug.Log("Confirmation yes, change to ARScene");
        Debug.Log("Fetch location data from database");
        SceneManager.LoadScene("ARScene");
    }

    void NoClick()
    {
        Debug.Log("Confirmation answer no");
        ConfScreen.SetActive(false);
    }

}
