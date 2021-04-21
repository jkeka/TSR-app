using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPassScript : MonoBehaviour
{
    
    private int[] testArray;
    public RectTransform virtualPassContent;
    public GameObject textPrefab;
    public List<GameObject> virtualPassLocations;
    public Button virtualPassButton;
    public List<string> testList = new List<string>();

    private void Start()
    {
        virtualPassButton.onClick.AddListener(LoadVirtualPass);
        
    }

    public void LoadVirtualPass()
    {
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

        if (virtualPassLocations != null)
        {
            ClearVirtualPass();
        }

        foreach (var item in User.GetVisitedLocations())
        {
           
            GameObject obj = Instantiate(textPrefab, virtualPassContent);
            Debug.Log("Created");
            obj.GetComponent<TMPro.TextMeshProUGUI>().text = item;
            virtualPassLocations.Add(obj);

            //GameObject textobject = ARSceneManager.instance.GetPooledObject();
            //textobject.GetComponent<TextMeshProUGUI>().text = item;
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


} 
