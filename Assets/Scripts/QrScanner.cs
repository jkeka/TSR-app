using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QrScanner : MonoBehaviour
{
    WebCamTexture webcamTexture;
    Coroutine routine = null;
    string QrCode = string.Empty;
    //public AudioSource beepSound;
    public TextMeshProUGUI qrtext;
    public Button qrScanner;
    public RectTransform qrScreen;
    private bool scanning;

    /// <summary>
    /// current/last recorded qr code
    /// </summary>
    public string currentQRcode;
        
        void Start()
    {
        qrScanner.onClick.AddListener(OnScanClick);

        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(512, 512);
        renderer.material.mainTexture = webcamTexture;
    }

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
                            User.AddVisitedLocation(currentQRcode);
                            qrtext.text = QrCode;
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
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
                //qrtext.text = ex.Message;
                StopScan();             
            }
            yield return null;
        }
    }

    private void StopScan()
    {
        if (scanning)
        {
            webcamTexture.Stop();
            scanning = false;
            StopCoroutine(routine);
            transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("QR-scanning stopped!");
        }
    }
    public void OnScanClick()
    {
        //ARSceneManager.instance.qrButton.gameObject.SetActive(true);
        if (scanning)
            StopScan();
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            qrtext.text = string.Empty;
            QrCode = string.Empty;
            routine = StartCoroutine(GetQRCode());
            Debug.Log("Scanning QR-code!");
        }
    }




}
