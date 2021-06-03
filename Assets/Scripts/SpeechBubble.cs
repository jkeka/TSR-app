using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class SpeechBubble : MonoBehaviour
{
    // This class attaches listener to speech bubble in info screen and runs the script when clicked

    // Counter for times the speech bubble has been clicked
    public static int count = 0;

    // References the speech bubble
    public Button bubble;

    // References Qr button in the info screen
    public Button qrButton;

    // References text gameobject of speech bubble
    Transform bubbleChild;

    // References the size and position of info screen
    public RectTransform infoScreen;

    // Coroutine to reset the speechBubble
    Coroutine routine = null;

    // Reference to table used in string localization
    LocalizedString localizedString = new LocalizedString() {TableReference = "Translations"};
    
    // Variables related to sailor interaction
    public delegate void AnimTrigger(int i);
    public AnimTrigger QV;
    public bool test;

    // Start is called before the first frame update
    void Start()
    {
   
        bubbleChild = bubble.transform.GetChild(0);
        bubble.onClick.AddListener(ClickTextBubble);
    }

    // Changes bubble text content when clicked. Also triggers the sailor animation
    public void ClickTextBubble()
    
    {
        try
        {
            if (test)
            {
                if (QV != null)
                    QV(2);
            }
            else
            {
                if (QV != null)
                    QV(2);
           
                if (count == DescriptionDataHandler.shipInfo.Count - 1)
                {
                    bubble.onClick.RemoveListener(ClickTextBubble);
                    count = 1;
                    
                    qrButton.gameObject.SetActive(true);
                    DescriptionDataHandler.shipInfo.Clear();
                    Debug.Log(DescriptionDataHandler.shipInfo);
                    routine = StartCoroutine(ResetBubble());
                    return;
                }

                bubbleChild.GetComponent<TMPro.TextMeshProUGUI>().text = DescriptionDataHandler.shipInfo[count];
                count++;
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            if (QV != null)
                QV(2);

            Debug.Log("Following error in the speechbubble: " + e.Message);
            localizedString.TableEntryReference = "INFO_ERROR";
            bubbleChild.GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
        catch (NullReferenceException e)
        {
            if (QV != null)
                QV(2);

            Debug.Log("Following error in the speechbubble: " + e.Message);
            localizedString.TableEntryReference = "INFO_ERROR";
            bubbleChild.GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
        catch (Exception e)
        {
            if (QV != null)
                QV(2);

            Debug.Log("Following error in the speechbubble: " + e.Message);
            localizedString.TableEntryReference = "INFO_ERROR";
            bubbleChild.GetComponent<TMPro.TextMeshProUGUI>().text = localizedString.GetLocalizedString();
        }
    }

    // Resets button listener and text after quitting info screen.
    IEnumerator ResetBubble()
    {
       
        while(infoScreen.GetSiblingIndex() == MapSceneManager.siblingIndex)
        {
            yield return new WaitForSecondsRealtime(1);
        }

        bubble.onClick.RemoveAllListeners();
        bubble.onClick.AddListener(ClickTextBubble);
        qrButton.gameObject.SetActive(false);
        Debug.Log("speechbubble reset!");
        StopCoroutine(routine);               
    }
}
