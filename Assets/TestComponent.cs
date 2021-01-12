using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

class Tester {
    public string Testi;
}

public class TestComponent : MonoBehaviour
{
    public Text textObject;
    
    public static int playerScore;
    public static string playerName;
    
    // Start is called before the first frame update
    private void Start()
    {
        textObject = GameObject.Find("Canvas/testText").GetComponent<Text>();
        textObject.text = "muuuttuu";
    }

    public void OnRetrieve()
    {
        RetrieveFromDatabase();
    }
    
    private void RetrieveFromDatabase()
    {
        
        RestClient.Get<Tester>("https://test-project1-d9370-default-rtdb.firebaseio.com/.json").Then(response =>
            {
                this.textObject.text = response.Testi;
            });
        
    }
    
}