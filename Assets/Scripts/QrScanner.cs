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
    string QrCode = string.Empty;
    //public AudioSource beepSound;
    public TextMeshProUGUI qrtext;
    private bool scanning;

    /// <summary>
    /// current/last recorded qr code
    /// </summary>
    public string currentQRcode;
        
        void Start()
    {
        ARSceneManager.instance.qrButton.onClick.AddListener(StopScan);

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
                        qrtext.text = QrCode;
                        currentQRcode = QrCode;
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
            StopCoroutine(GetQRCode());
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }
    public void OnScanClick()
    {
        if (scanning)
            StopScan();
        else
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            qrtext.text = string.Empty;
            QrCode = string.Empty;
            StartCoroutine(GetQRCode());
        }
    }




}
