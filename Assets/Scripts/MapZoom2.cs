using UnityEngine;
using UnityEngine.UI;

public class MapZoom2 : MonoBehaviour
{
    [SerializeField] float startSize = 1;
    [SerializeField] float minSize = 0.75f;
    [SerializeField] float maxSize = 1;

    [SerializeField] private float zoomRate = 5;

    private float scrollWheel;

    Vector3 difference;
    Vector3 mousePos;

    public Image map;

    private void Start()
    {

    }

    void Update()
    {
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        //ChangeZoom(Input.GetAxis("Mouse ScrollWheel"));
        Debug.Log("fsdfs" + scrollWheel);

        /*
        if (scrollWheel != 0)
        {
            Debug.Log("VITUN KULL");
        }
        */
    }

    void ChangeZoom(float scrollWheel)
    {


        float rate = 1 + zoomRate * Time.unscaledDeltaTime;
        if (scrollWheel > 0 && map.transform.localScale.y > minSize)
        {
            mousePos = Input.mousePosition;
            SetZoom(Mathf.Clamp(map.rectTransform.localScale.y / rate, minSize, maxSize));
            difference = map.rectTransform.position - mousePos;
            map.transform.position = mousePos + (difference * 0.9F);
        }
        else if (scrollWheel < 0 && map.rectTransform.localScale.y < maxSize)
        {
            mousePos = Input.mousePosition;
            SetZoom(Mathf.Clamp(map.rectTransform.localScale.y * rate, minSize, maxSize));
            difference = map.rectTransform.position - mousePos;
            map.rectTransform.position = mousePos + (difference * 1.11F);
        }

    }

    void SetZoom(float targetSize)
    {
        map.rectTransform.localScale = new Vector3(targetSize, targetSize, 1);
        Debug.Log("TOIMIIKO");

    }
}
