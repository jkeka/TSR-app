using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static User;

public class MapSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MapSceneManager Instance;


    public GameObject confScreen;


    public RectTransform mapScreen;
    public RectTransform setScreen;
    public RectTransform schedScreen;
    public RectTransform langScreen;
    public RectTransform compassScreen;
    public RectTransform libraryScreen;
    public RectTransform glossaryScreen;

    public GameObject screens;
    //public List<GameObject> screensList = new List<GameObject>();

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
    public Button closeButton;

    private int mapSiblingIndex = 5;
    private int siblingIndex = 8;




    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;


        string deviceCode = SystemInfo.deviceUniqueIdentifier; // Replace with any string to test the db
        User.InitializeUser(deviceCode);
    }

    void Start()
    {

        screens.SetActive(true);

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
        closeButton.onClick.AddListener(CloseClick);

        //menuText.text = "Language";

        GameObject.DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FinnClick()
    {
        screens.SetActive(false);

        Debug.Log("Set system to Finnish language!");
        User.SetLanguage("fi");
        
        mapScreen.SetSiblingIndex(siblingIndex);

        //langScreen.SetActive(false);
        //HubScreen.SetActive(true);
    }

    void EngClick()
    {
        screens.SetActive(false);

        Debug.Log("Set system to English language!");
        User.SetLanguage("en");
        mapScreen.SetSiblingIndex(siblingIndex);
    }

    void MapClick()
    {
        screens.SetActive(false);

        Debug.Log("Map clicked");
        mapScreen.SetSiblingIndex(siblingIndex);

    }

    void ScheduleClick()
    {
        screens.SetActive(true);

        Debug.Log("Schedule clicked");
        schedScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    void SettingsClick()
    {
        screens.SetActive(true);

        Debug.Log("Settings clicked");
        setScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    void LibraryClick()
    {
        screens.SetActive(true);

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
        DescriptionDataHandler.LoadDescription(CoordinateData.id, "fi");
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

    public void ExitClick()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }

    void CloseClick()
    {
        Debug.Log("Description closed");
        glossaryScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);
    }

}
   
