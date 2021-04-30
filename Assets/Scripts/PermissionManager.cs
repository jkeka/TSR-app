using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{

    public GameObject permissionScreen;
    /*
    // Start is called before the first frame update
    void Awake()
    {
        permissionScreen.SetActive(true);

        StartCoroutine(CheckPermissions());





    }
 
    IEnumerator CheckPermissions()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) || !Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            AskPermissions();

        }

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            CloseScreen();
            yield break;
        }
        else
        {
            Application.Quit();
        }
    }


    void AskPermissions()
    {
        //Camera permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);

        }
        //Location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);

        }
    }

    void CloseScreen()
    {
        permissionScreen.SetActive(false);

    }
    */
}
