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
    public static MapSceneManager Instance;
    //References

    DatabaseReference reference;
    DataSnapshot snapshot;
    User currentUser;


    public GameObject confScreen;


    public RectTransform mapScreen;
    public RectTransform setScreen;
    public RectTransform schedScreen;
    public RectTransform langScreen;
    public RectTransform compassScreen;
    public RectTransform libraryScreen;

    //Buttons

    //Language
    public Button finnButton;
    public Button engButton;

    //Navigation

    public Button scheduleButton;
    public Button settingsButton;
    public Button mapButton;
    public Button langButton;
    public Button confYesButton;
    public Button confNoButton;
    public Button arButton;
    public Button libraryButton;
    public Button exitButton;

    private int mapSiblingIndex = 5;
    private int siblingIndex = 6;




    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

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
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Task faulted!");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Task success!");
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Value != null)
                    {
                        currentUser = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                        Debug.Log(snapshot.Value);
                        Debug.Log(currentUser.ToString());
                        OldUser();
                    }
                    else
                    {
                        Debug.Log("forwarding to create user");
                        CreateUser(deviceCode);
                    }


                }
                else
                {
                    Debug.Log("Something terrible happened during fetching data");
                }
            });
    }

    void Start()
    {


        langScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);



        //Language clicks
        finnButton.onClick.AddListener(FinnClick);
        engButton.onClick.AddListener(EngClick);

        //Navigation clicks
        scheduleButton.onClick.AddListener(ScheduleClick);
        mapButton.onClick.AddListener(MapClick);
        settingsButton.onClick.AddListener(SettingsClick);
        libraryButton.onClick.AddListener(LibraryClick);

        langButton.onClick.AddListener(LangClick);
        confYesButton.onClick.AddListener(YesClick);
        confNoButton.onClick.AddListener(NoClick);
        arButton.onClick.AddListener(ARClick);
        exitButton.onClick.AddListener(ExitClick);

        //menuText.text = "Language";

        GameObject.DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FinnClick()
    {
        Debug.Log("Set system to Finnish language!");
        currentUser.setLanguage("fi");
        mapScreen.SetSiblingIndex(siblingIndex);

        //langScreen.SetActive(false);
        //HubScreen.SetActive(true);

    }

    void EngClick()
    {
        Debug.Log("Set system to English language!");
        currentUser.setLanguage("en");
        mapScreen.SetSiblingIndex(siblingIndex);



    }

    void MapClick()
    {
        Debug.Log("Map clicked");
        mapScreen.SetSiblingIndex(siblingIndex);

    }

    void ScheduleClick()
    {
        Debug.Log("Schedule clicked");
        schedScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    void SettingsClick()
    {
        Debug.Log("Settings clicked");
        setScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    void LibraryClick()
    {
        Debug.Log("Library clicked");
        libraryScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    void LangClick()
    {
        Debug.Log("Language selection clicked");
        langScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    void YesClick()
    {
        Debug.Log("Confirmation yes, change to CompassScreen");
        Debug.Log("Fetch location data from database");
        compassScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);
        QuizDataHandler.LoadQuizz(CoordinateData.id, "fi");    
        //SceneManager.LoadScene("ARScene");
    }

    void NoClick()
    {
        Debug.Log("Confirmation answer no");
        confScreen.SetActive(false);
    }

    void ARClick()
    {
        SceneManager.LoadScene("ARScene");
    }

    void ExitClick()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }

}
   
