using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using Firebase.Database;

using static User;

public class MapSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MapSceneManager Instance;
  
    public bool isFirstTime;

    public GameObject sailor;

    public GameObject instructionCanvas;
    public GameObject bottomBar;

    public GameObject confScreen;
    public GameObject errorMessage;

    public GameObject qrText;
   
    public RectTransform mapScreen;
    public RectTransform setScreen;
    public RectTransform schedScreen;
    public RectTransform langScreen;
    public RectTransform compassScreen;
    public RectTransform libraryScreen;
    public RectTransform glossaryScreen;
    public RectTransform infoScreen;
    public RectTransform qrScannerScreen;
    public RectTransform virtualPassScreen;
   
    public List<GameObject> screenObjects = new List<GameObject>();

    public GameObject screens;
    //public List<GameObject> screensList = new List<GameObject>();

    //Buttons

    //Language
    public Button finnButton;
    public Button engButton;
    public Button sweButton;

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
    public Button qrButton;
    public Button virtualPassButton;
    public Button speechBubble;
    

    private int mapSiblingIndex = 5;
    public static int siblingIndex = 11;




    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        // FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);  //Disables the cache for data

        /*StartCoroutine(CheckConnection(isConnected =>
        {
            string deviceCode = SystemInfo.deviceUniqueIdentifier; // Replace with any string to test the db
            User.InitializeUser(deviceCode);

        }
        ));*/

    }

    void Start()
    {
        isFirstTime = true;

        sailor.SetActive(false);


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
        sweButton.onClick.AddListener(SweClick);

        //Navigation clicks
        scheduleButton.onClick.AddListener(ScheduleClick);
        mapButton.onClick.AddListener(MapClick);
        settingsButton.onClick.AddListener(SettingsClick);
        libraryButton.onClick.AddListener(LibraryClick);
        //passButton.onClick.AddListener(VirtualPassBarClick);

        langButton.onClick.AddListener(LangClick);
        confYesButton.onClick.AddListener(YesClick);
        confNoButton.onClick.AddListener(NoClick);
        arButton.onClick.AddListener(infoClick);
        exitButton.onClick.AddListener(ExitClick);
        closeButton.onClick.AddListener(CloseClick);
        instructionButton.onClick.AddListener(InstructionClick);
        qrButton.onClick.AddListener(QRScreenClick);
        virtualPassButton.onClick.AddListener(VirtualPassClick);
        
        //menuText.text = "Language";

        //GameObject.DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update()
    {
        //Always deactivate 3D sailor if infoScreen is not chosen
        if (infoScreen.GetSiblingIndex() != siblingIndex)
        {
            sailor.SetActive(false);
        }

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
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        
        mapScreen.SetSiblingIndex(siblingIndex);

        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        EventDataHandler.ChangeLanguage();

        //langScreen.SetActive(false);
        //HubScreen.SetActive(true);
    }

    void EngClick()
    {
        screens.SetActive(false);

        Debug.Log("Set system to English language!");
        User.SetLanguage("en");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
       
        mapScreen.SetSiblingIndex(siblingIndex);

        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        EventDataHandler.ChangeLanguage();
    }

    void SweClick()
    {
        screens.SetActive(false);

        Debug.Log("Set system to Swedish language!");
        User.SetLanguage("se");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];

        mapScreen.SetSiblingIndex(siblingIndex);

        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        EventDataHandler.ChangeLanguage();
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
            DescriptionDataHandler.LoadDescription(CoordinateData.id, User.GetLanguage());
        }       

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(true);
        }

        //speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Description.shipInfo[0];
    }

    void NoClick()
    {
        Debug.Log("Confirmation answer no");
        //schedScreen.SetSiblingIndex(siblingIndex);

        if(virtualPassScreen.GetSiblingIndex() == siblingIndex - 1)
        {
            virtualPassScreen.SetSiblingIndex(siblingIndex);
        }
        else
        {
            screens.SetActive(false);
            for (int i = 0; i < screenObjects.Count; i++)
            {
                screenObjects[i].SetActive(true);
            }
        }
    }

    public void infoClick()
    {
        LocalizedString localizedString = new LocalizedString() {TableReference = "Translations" };
                          
        if (CoordinateData.type == "ship")
        {
            sailor.SetActive(true);
            infoScreen.SetSiblingIndex(siblingIndex);
            mapScreen.SetSiblingIndex(mapSiblingIndex);
            Debug.Log("Moved to infoScreen");
        }
        else
        {
            MapClick();
            return;
        }
        try
        {
            if (SpeechBubble.count == 0)
            {
                localizedString.TableEntryReference = "INFO_START";              
                speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
            }
            else
            {
                speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Description.shipInfo[0];
                SpeechBubble.count = 1;
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.Log(e);
            localizedString.TableEntryReference = "INFO_ERROR";
            speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
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

    void QRScreenClick()
    {
        Debug.Log("Moved to QR Screen");
        qrScannerScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);
        qrButton.gameObject.SetActive(false);

        //qrText.GetComponent<TMPro.TextMeshProUGUI>().text = "Laivan nimi";
    }

    void VirtualPassClick()
    {
        screens.SetActive(true);
        //passBackBútton.SetActive(true);
        Debug.Log("Moved to Virtual Pass");
        virtualPassScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }

    /*void VirtualPassBarClick()
    {
        screens.SetActive(true);
        passBackBútton.SetActive(false);

        Debug.Log("Moved to Virtual Pass");
        virtualPassScreen.SetSiblingIndex(siblingIndex);
        mapScreen.SetSiblingIndex(mapSiblingIndex);

    }*/

}
   
