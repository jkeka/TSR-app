using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSceneManager : MonoBehaviour
{
    // Start is called before the first frame update


    //Screens
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
    public Button ScheduleButton;
    public Button SettingsButton;
    public Button SchedBackButton;
    public Button LangButton;
    public Button SetBackButton;
    public Button ConfYesButton;
    public Button ConfNoButton;

    //Destinations
    public Button ShipButton;


    void Start()
    {

        //Activates and deactivates screen at start
        LangScreen.SetActive(true);
        MapScreen.SetActive(false);
        SetScreen.SetActive(false);
        SchedScreen.SetActive(false);
        ConfScreen.SetActive(false);

        //Language clicks
        FinnButton.onClick.AddListener(FinnClick);
        EngButton.onClick.AddListener(EngClick);

        //Navigation clicks
        ScheduleButton.onClick.AddListener(ScheduleClick);
        SettingsButton.onClick.AddListener(SettingsClick);
        SchedBackButton.onClick.AddListener(SchedBackClick);
        SetBackButton.onClick.AddListener(SetBackClick);
        LangButton.onClick.AddListener(LangClick);
        ConfYesButton.onClick.AddListener(YesClick);
        ConfNoButton.onClick.AddListener(NoClick);

        //Destination clicks
        ShipButton.onClick.AddListener(ShipClick);


    }

    // Update is called once per frame
    void Update()
    {

    }
    void FinnClick()
    {
        Debug.Log("Set system to Finnish language!");
        LangScreen.SetActive(false);
        MapScreen.SetActive(true);
    }

    void EngClick()
    {
        Debug.Log("Set system to English language!");
        LangScreen.SetActive(false);
        MapScreen.SetActive(true);
    }

    void ScheduleClick()
    {
        Debug.Log("Schedule clicked");
        SchedScreen.SetActive(true);
    }

    void SettingsClick()
    {
        Debug.Log("Settings clicked");
        SetScreen.SetActive(true);
    }

    void SchedBackClick()
    {
        Debug.Log("Exit clicked, return to map screen");
        MapScreen.SetActive(true);
        LangScreen.SetActive(false);
        SetScreen.SetActive(false);
        SchedScreen.SetActive(false);
    }

    void SetBackClick()
    {
        Debug.Log("Exit clicked, return to map screen");
        MapScreen.SetActive(true);
        LangScreen.SetActive(false);
        SetScreen.SetActive(false);
        SchedScreen.SetActive(false);
    }

    void LangClick()
    {
        Debug.Log("Language selection clicked");
        LangScreen.SetActive(true);
        SetScreen.SetActive(false);
    }

    void ShipClick()
    {
        Debug.Log("Destination ship chosen, ask confirmation");
        ConfScreen.SetActive(true);
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
