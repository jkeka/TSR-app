using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VirtualPassScript : MonoBehaviour
{
    
    private int[] testArray;
    public void OnEnable()
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
        //}

        foreach (var item in User.GetVisitedLocations())
        {
            GameObject textobject = ARSceneManager.instance.GetPooledObject();
            textobject.GetComponent<TextMeshProUGUI>().text = item;
        }
    }
    public void OnDisable()
    {
        foreach (var item in ARSceneManager.instance.pooledTextPrefabs)
        {
            item.SetActive(false);
        }
    }

} 
