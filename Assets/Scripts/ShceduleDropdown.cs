using UnityEngine;
using UnityEngine.UI;

public class ShceduleDropdown : MonoBehaviour

{
    //This class attaches listener to dropdown in schedule screen and runs the script when drodown value changes

    // References the dropdown
    public TMPro.TMP_Dropdown dropdown;

    // References the position of schedule screen
    public Transform scheduleScreen;

    // Start is called before the first frame update
    void Start()
    {       
    dropdown.onValueChanged.AddListener(delegate { CheckDate(); });  
    }

    // Checks if the event date matches the selected dropdown date.
    public void CheckDate()
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
