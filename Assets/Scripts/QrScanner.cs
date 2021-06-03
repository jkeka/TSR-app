using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;
using ZXing;

public class QrScanner : MonoBehaviour
{
    // This class attaches listener to Qr button used for scanning the Qr code and runs the script when clicked

    // Variables that store information about the QR code
    public string currentQRcode;
    private string QrCode = string.Empty;

    // Boolean to check if application is currently scanning
    private bool scanning;

    // Webcam screen for scanning
    WebCamTexture webcamTexture;

    // Displays a texture for the UI
    public RawImage cameraImage;

    // Routine for camera to scan the Qr code
    Coroutine routine = null;  

    // References text below
    public TextMeshProUGUI qrtext;

    // References the Qr button
    public Button qrScanner;

    //References the Qr screen
    public RectTransform qrScreen;
    
      
    void Start()
    {
        qrtext.text = string.Empty;

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            
            
        } else {
            qrScanner.onClick.AddListener(OnScanClick);
            var renderer = cameraImage;
            webcamTexture = new WebCamTexture(512, 512);
            renderer.material.mainTexture = webcamTexture;
        }      
    }

    // Starts or stops scanning for Qr code
    public void OnScanClick()
    {
        if (scanning)
            StopScan();
        else
        {
            qrScanner.transform.GetChild(0).gameObject.SetActive(false);
            qrtext.text = string.Empty;
            QrCode = string.Empty;
            routine = StartCoroutine(GetQRCode());
            Debug.Log("Scanning QR-code!");
        }
    }

    // Scans for the Qr code
    IEnumerator GetQRCode()
    {
       
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        scanning = true;

        var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {
            if(qrScreen.GetSiblingIndex() != MapSceneManager.siblingIndex)
            {
                StopScan();
                break;
            }

            try
            {
                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result = barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result != null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        Debug.Log("DECODED TEXT FROM QR: " + QrCode);
                        currentQRcode = QrCode;

                        if (!User.visitedLocations.Contains(currentQRcode))
                        {
                            string name = User.AddVisitedLocation(currentQRcode);
                            qrtext.text = name;
                        }
                        else
                        {
                            qrtext.text = "Already Visited";
                        }
                            StopScan();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                StopScan();             
            }
            yield return null;
        }
    }

    // Stops the scan for Qr code
    private void StopScan()
    {
        if (scanning)
        {
            webcamTexture.Stop();
            scanning = false;
            StopCoroutine(routine);
            qrScanner.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("QR-scanning stopped!");
        }
    }
    
}
