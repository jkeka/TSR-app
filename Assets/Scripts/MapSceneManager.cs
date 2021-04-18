using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

using Firebase.Database;

using static User;

public class MapSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MapSceneManager Instance;

    public bool isFirstTime;

    public GameObject instructionCanvas;
    public GameObject bottomBar;

    public GameObject confScreen;
    public GameObject errorMessage;
   
    public RectTransform mapScreen;
    public RectTransform setScreen;
    public RectTransform schedScreen;
    public RectTransform langScreen;
    public RectTransform compassScreen;
    public RectTransform libraryScreen;
    public RectTransform glossaryScreen;

    public List<GameObject> screenObjects = new List<GameObject>();

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
    public Button instructionButton;

    private int mapSiblingIndex = 5;
    public static int siblingIndex = 8;




    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);  //Disables the cache for data

        StartCoroutine(CheckConnection(isConnected =>
        {
            string deviceCode = SystemInfo.deviceUniqueIdentifier; // Replace with any string to test the db
            User.InitializeUser(deviceCode);

        }
        ));

    }

    void Start()
    {
        isFirstTime = true;

        bottomBar.SetActive(false);
        instructionCanvas.SetActive(false);
        screens.SetActive(true);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(true);
        }

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
        instructionButton.onClick.AddListener(InstructionClick);

        //menuText.text = "Language";

        GameObject.DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator CheckConnection(UnityAction<bool> action)
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
        action(true);
    }

    void FinnClick()
    {
        screens.SetActive(false);

        Debug.Log("Set system to Finnish language!");
        User.SetLanguage("fi");
        
        mapScreen.SetSiblingIndex(siblingIndex);

        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        


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

        if (CoordinateData.type == "ship")
        {
            DescriptionDataHandler.LoadDescription(CoordinateData.id, "fi");
        }       

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(true);
        }
    }

    void NoClick()
    {
        Debug.Log("Confirmation answer no");
        schedScreen.SetSiblingIndex(siblingIndex);

        screens.SetActive(false);
        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(true);
        }

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

    void InstructionClick()
    {
        Debug.Log("Instruction clicked");
        instructionCanvas.SetActive(true);
    }

}
   
