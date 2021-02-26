using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShceduleDropdown : MonoBehaviour

{
    public TMPro.TMP_Dropdown dropdown;
    public Transform scheduleScreen;
    List<Button> activeList = new List<Button>();
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
               
        foreach (Button button in MapSceneDatabase.scheduleList)
        {
            var eventDate = button.GetComponent<Schedule>().startTime.Day + "." + button.GetComponent<Schedule>().startTime.Month;

            if (eventDate == day)
            {
                button.gameObject.SetActive(true);
                activeList.Add(button);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
        SetEventPosition(activeList);
    }

    public void SetEventPosition(List<Button> activeList)
    // Sets event position in the schedule.
    {
        int index = 0;
        Vector3 defaultPos = new Vector3();
        foreach (Button button in activeList)
        {
       
            if (index != 0)
            {
                button.transform.position = defaultPos + new Vector3(0, index * -40, 0);
            }
            else
            {
                defaultPos = button.transform.position;
            }

            index++;
        }
        activeList.Clear();
    }
}
