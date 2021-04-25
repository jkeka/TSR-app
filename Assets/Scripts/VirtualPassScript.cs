using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPassScript : MonoBehaviour
{
    
    private int[] testArray;
    public RectTransform virtualPassContent;
    public List<Button> virtualPassLocations;
    public GameObject virtualPassScreen;
    public Button virtualPassLocation;
    public Button virtualPassButton;
    
    private void Start()
    {
        virtualPassButton.onClick.AddListener(LoadVirtualPass);
       
    }

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
                    if (shipName == location)
                    {
                        button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                    }
                }
            }                   
        }
    }

    public void ClearVirtualPass()
    {
        for (int i = 0; i < virtualPassLocations.Count; i++)
        {
            Destroy(virtualPassLocations[i].gameObject);
        }

        virtualPassLocations.Clear();
    }


    //GameObject textobject = ARSceneManager.instance.GetPooledObject();
    //textobject.GetComponent<TextMeshProUGUI>().text = item;

    //testArray = new int[10];
    //for (int i = 0; i < testArray.Length; i++)
    //{
    //    testArray[i] = i;
    //}
    //foreach (var item in testArray)
    //{

    //    GameObject textobject = ARSceneManager.instance.GetPooledObject();
    //    textobject.GetComponent<TextMeshProUGUI>().text = item.ToString();
    //    textobject.transform.SetParent(gameObject.transform.GetChild(0).GetChild(0).GetChild(0));
    //}User.GetVisitedLocations()


}
