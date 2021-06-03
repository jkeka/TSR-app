using System;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Firebase.Database;

public class MapSceneManager : MonoBehaviour
{
    // This class operates the functioning of MapScene UI and most of the static buttons

    // Number of sibling indexes the screens-object in the MainCanvas has
    public static int siblingIndex = 10;

    // Checks if player is starting the app first time. Point is to activate the Instructions at the beginning. Currently set always to true
    public bool isFirstTime;

    // Reference to gameobject MapSceneDatabase that contains database scripts
    public GameObject mapSceneDataBase;

    // References to gameobjects in the MainCanvas    
    public GameObject sailor;
    public GameObject screens;
    public GameObject instructionCanvas;
    public GameObject mapCanvas;
    public GameObject bottomBar;
    public GameObject confScreen;
    public GameObject errorMessage;
    public GameObject descriptionText;
    public GameObject qrText;

    // References to size and position of screens in the MainCanvas
    public RectTransform setScreen;
    public RectTransform schedScreen;
    public RectTransform langScreen;
    public RectTransform compassScreen;
    public RectTransform libraryScreen;
    public RectTransform glossaryScreen;
    public RectTransform infoScreen;
    public RectTransform qrScannerScreen;
    public RectTransform virtualPassScreen;
    public RectTransform descriptionScreen;

    // References to language buttons in the language screen
    public Button finnButton;
    public Button engButton;
    public Button sweButton;

    // References to buttons used for navigation in the app
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
    
    // Reference to speech bubble where ship information is displayed
    public Button speechBubble;

    //List for screens in the MainCanvas
    public List<GameObject> screenObjects = new List<GameObject>();

    // Reference to EventDataHandler script
    private EventDataHandler eventDataHandler;

    void Awake()
    {     
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);  //Disables the cache for data
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
            screenObjects[i].SetActive(false);
        }

        langScreen.gameObject.SetActive(true); 

        langScreen.SetSiblingIndex(siblingIndex);
      
        finnButton.onClick.AddListener(FinnClick);
        engButton.onClick.AddListener(EngClick);
        sweButton.onClick.AddListener(SweClick);

        scheduleButton.onClick.AddListener(ScheduleClick);
        mapButton.onClick.AddListener(MapClick);
        settingsButton.onClick.AddListener(SettingsClick);
        libraryButton.onClick.AddListener(LibraryClick);
        langButton.onClick.AddListener(LangClick);
        confYesButton.onClick.AddListener(YesClick);
        confNoButton.onClick.AddListener(NoClick);
        arButton.onClick.AddListener(infoClick);
        exitButton.onClick.AddListener(ExitClick);
        closeButton.onClick.AddListener(CloseClick);
        instructionButton.onClick.AddListener(InstructionClick);
        qrButton.onClick.AddListener(QRScreenClick);
        virtualPassButton.onClick.AddListener(VirtualPassClick);

        eventDataHandler = mapSceneDataBase.GetComponent<EventDataHandler>();
        AndroidNotificationCenter.RegisterNotificationChannel(Notifications.defaultChannel);
     
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

    // Selects to finnish language to be used in the app
    void FinnClick()
    {
        screens.SetActive(false);
        mapCanvas.SetActive(true);

        Debug.Log("Set system to Finnish language!");
        User.SetLanguage("fi");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];

        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        eventDataHandler.LoadScheduleData();
    }

    // Selects to english language to be used in the app
    void EngClick()
    {
        screens.SetActive(false);
        mapCanvas.SetActive(true);

        Debug.Log("Set system to English language!");
        User.SetLanguage("en");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
     
        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        eventDataHandler.LoadScheduleData();
    }

    // Selects to swedish language to be used in the app
    void SweClick()
    {
        screens.SetActive(false);
        mapCanvas.SetActive(true);

        Debug.Log("Set system to Swedish language!");
        User.SetLanguage("se");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];

        if (isFirstTime == true)
        {
            instructionCanvas.SetActive(true);

        }

        bottomBar.SetActive(true);
        eventDataHandler.LoadScheduleData();
    }

    // Moves user to the map screen
    void MapClick()
    {
        screens.SetActive(false);
        mapCanvas.SetActive(true);

        Debug.Log("Map clicked");      

    }

    // Moves user to the schedule screen
    void ScheduleClick()
    {
        screens.SetActive(true);
        mapCanvas.SetActive(false);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        schedScreen.gameObject.SetActive(true);

        Debug.Log("Schedule clicked");
        schedScreen.SetSiblingIndex(siblingIndex);
    }

    // Moves user to the settings screen
    void SettingsClick()
    {
        screens.SetActive(true);
        mapCanvas.SetActive(false);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        setScreen.gameObject.SetActive(true);

        Debug.Log("Settings clicked");
        setScreen.SetSiblingIndex(siblingIndex);
    }

    // Moves user to the library screen
    void LibraryClick()
    {
        screens.SetActive(true);
        mapCanvas.SetActive(false);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        libraryScreen.gameObject.SetActive(true);

        Debug.Log("Library clicked");
        libraryScreen.SetSiblingIndex(siblingIndex);
    }

    // Moves user to the language screen
    void LangClick()
    {
        screens.SetActive(true);
        mapCanvas.SetActive(false);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        langScreen.gameObject.SetActive(true);

        Debug.Log("Language selection clicked");
        langScreen.SetSiblingIndex(siblingIndex);
       

    }

    // Moves user to the virtual pass screen
    void VirtualPassClick()
    {
        screens.SetActive(true);
        mapCanvas.SetActive(false);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        virtualPassScreen.gameObject.SetActive(true);

        Debug.Log("Moved to Virtual Pass");
        virtualPassScreen.SetSiblingIndex(siblingIndex);
    }

    // Moves user from confirmation screen to compass screen, loads description if target is ship
    void YesClick()
    {
        Debug.Log("Confirmation yes, change to CompassScreen");
        Debug.Log("Fetch location data from database");
        compassScreen.SetSiblingIndex(siblingIndex);


        if (CoordinateData.type == "ship")
        {
            DescriptionDataHandler.LoadDescription(CoordinateData.id, User.GetLanguage());
        }

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        compassScreen.gameObject.SetActive(true);

    }

    // Moves user back from confirmation screen to the previous screen
    void NoClick()
    {
        Debug.Log("Confirmation answer no");

        if(virtualPassScreen.GetSiblingIndex() == siblingIndex - 1)
        {
            virtualPassScreen.SetSiblingIndex(siblingIndex);
        }
        else if (schedScreen.GetSiblingIndex() == siblingIndex - 1)
        {
            schedScreen.SetSiblingIndex(siblingIndex);
        }
        else
        {
            screens.SetActive(false);
            mapCanvas.SetActive(true);          
        }
    }

    // Moves user to the infoscreen if target is ship and sets sailor introduction. Otherwise moves back to map screen and sends notification of arrival
    public void infoClick()
    {
        LocalizedString localizedString = new LocalizedString() {TableReference = "Translations" };
                          
        if (CoordinateData.type == "ship")
        {
            for (int i = 0; i < screenObjects.Count; i++)
            {
                screenObjects[i].SetActive(false);
            }

            infoScreen.gameObject.SetActive(true);
            sailor.SetActive(true);
            infoScreen.SetSiblingIndex(siblingIndex); 
            Debug.Log("Moved to infoScreen");
        }
        else
        {
            MapClick();
            Notifications.SetNotificationLanguage();         
            AndroidNotificationCenter.SendNotification(Notifications.arrivalNotification, "channel_id");
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
                speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = DescriptionDataHandler.shipInfo[0];
                SpeechBubble.count = 1;
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.Log("Following error in the speechbubble: " + e.Message);
            localizedString.TableEntryReference = "INFO_ERROR";
            speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Following error in the speechbubble: " + e.Message);
            localizedString.TableEntryReference = "INFO_ERROR";
            speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
        catch (Exception e)
        {
            Debug.Log("Following error in the speechbubble: " + e.Message);
            localizedString.TableEntryReference = "INFO_ERROR";
            speechBubble.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
    }

    // Exits the application
    public void ExitClick()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }

    // Closes the description screen
    void CloseClick()
    {
        Debug.Log("Description closed");

        descriptionScreen.gameObject.SetActive(false);
        glossaryScreen.SetSiblingIndex(siblingIndex);
        
    }

    // Shows the instructions
    void InstructionClick()
    {
        Debug.Log("Instruction clicked");
        instructionCanvas.SetActive(true);
    }

    // Moves user to QR screen for scanning
    void QRScreenClick()
    {
        Debug.Log("Moved to QR Screen");

        for (int i = 0; i < screenObjects.Count; i++)
        {
            screenObjects[i].SetActive(false);
        }

        qrScannerScreen.gameObject.SetActive(true);
        qrScannerScreen.SetSiblingIndex(siblingIndex);
        qrButton.gameObject.SetActive(false);
    }

}
   
