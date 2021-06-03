using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPassScript : MonoBehaviour
{
    // This class operates the virtual pass screen when clicked
    
    // The location where virtual pass locations are created
    public RectTransform virtualPassContent;
    
    // Stores references of created virtual pass locations
    public List<Button> virtualPassLocations;
    
    // References the virtual pass screen in MainCanvas
    public GameObject virtualPassScreen;

    // References the virtual pass location prefab that stores information about target
    public Button virtualPassLocation;

    // References  the virtual pass button in the menu bar
    public Button virtualPassButton;
       
    private void Start()
    {
        virtualPassButton.onClick.AddListener(LoadVirtualPass);    
    }

    // Loads the virtual pass locations to the virtual pass screen
    public void LoadVirtualPass()
    {
              
        if (virtualPassLocations != null)
        {
            ClearVirtualPass();
        }

        foreach (var coordinate in LocationDataHandler.markerList)
        {
            if( coordinate.GetComponent<Coordinates>().type == "ship")
            {
                string shipName = coordinate.GetComponent<Coordinates>().locationName;

                Button button = Instantiate(virtualPassLocation, virtualPassContent);   
                button.transform.localScale = new Vector3(1, 1, 1);
                button.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
                button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = shipName;
                virtualPassLocations.Add(button);

                button.GetComponent<Coordinates>().locationName = coordinate.GetComponent<Coordinates>().locationName;
                button.GetComponent<Coordinates>().latitude = coordinate.GetComponent<Coordinates>().latitude;
                button.GetComponent<Coordinates>().longitude = coordinate.GetComponent<Coordinates>().longitude;
                button.GetComponent<Coordinates>().id = coordinate.GetComponent<Coordinates>().id;
                button.GetComponent<Coordinates>().type = coordinate.GetComponent<Coordinates>().type;

                foreach (var location in User.GetVisitedLocations())
                {
                    if (button.GetComponent<Coordinates>().id == location)
                    {
                        button.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }                   
        }
    }

    // Deletes the virtual pass locations from the virtual pass screen
    public void ClearVirtualPass()
    {
        for (int i = 0; i < virtualPassLocations.Count; i++)
        {
            Destroy(virtualPassLocations[i].gameObject);
        }

        virtualPassLocations.Clear();
    }
}
