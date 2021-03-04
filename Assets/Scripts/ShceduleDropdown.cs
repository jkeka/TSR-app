using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShceduleDropdown : MonoBehaviour

{
    public TMPro.TMP_Dropdown dropdown;
    public Transform scheduleScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    dropdown.onValueChanged.AddListener(delegate {
            CheckDate();      
        });
   
    }

    public void CheckDate()
     // Checks if the event date matches the selected dropdown date.
    {
        var day = dropdown.options[dropdown.value].text;
               
        foreach (Button button in EventDataHandler.scheduleList)
        {
            var eventDate = button.GetComponent<Event>().startTime.Day + "." + button.GetComponent<Event>().startTime.Month;

            if (eventDate == day)
            {
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
