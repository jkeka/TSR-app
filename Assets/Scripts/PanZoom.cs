using UnityEngine;

public class PanZoom : MonoBehaviour
{
    // This class operates the zooming function of the map screen

    // Checks if user is zooming
    bool IsZooming = false;

    // Zooming position and speed
    Vector3 touchStart;
    public float zoomMin = 50;
    public float zoomMax = 480;
    private float zoomSlow = 0.4f;

    // Map size and position
    public RectTransform map;
    public float mapWidth;
    public float mapHeigth;
    public float mapHeightBound;
    public float mapWidthBound;

    // Camera size and position
    public float cameraSize;
    public float cameraWidth;
    public float cameraHeight;
    public float cameraPositionX;
    public float cameraPositionY;
    public float cameraBoundXRight;
    public float cameraBoundXLeft;
    public float cameraBoundYTop;
    public float cameraBoundYBot;

    void Start()
    {
       
        mapHeigth = map.sizeDelta.y;
        mapWidth = map.sizeDelta.x;

        zoomMin = 50;
        zoomMax = 480;
    }

    void Update()
    {
        cameraSize = Camera.main.orthographicSize;

        cameraHeight = cameraSize * 2;
        cameraWidth = cameraHeight * 0.5625f;

        mapHeightBound = mapHeigth / 2;
        mapWidthBound = mapWidth / 2;

        //Zooming and moving the camera
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            IsZooming = true;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float curretnMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = curretnMagnitude - prevMagnitude;

            Zoom(difference * zoomSlow);
        }

        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            IsZooming = false;

        }
        if (IsZooming == false)
        {
            Paning();
        }
    
    Zoom(Input.GetAxis("Mouse ScrollWheel") * 50);

        void Zoom(float increment)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMin, zoomMax);
        }

        //Setting boundaries
        cameraPositionX = transform.position.x;
        cameraPositionY = transform.position.y;

        cameraPositionX = transform.position.x;
        cameraPositionY = transform.position.y;

        cameraBoundYTop = cameraPositionY + (cameraHeight / 2);
        cameraBoundYBot = (-cameraPositionY - (-cameraHeight / 2)) * -1;

        cameraBoundXRight = cameraPositionX + (cameraWidth / 2);
        cameraBoundXLeft = (-cameraPositionX - (-cameraWidth / 2)) * -1;

        if (cameraBoundYTop > mapHeightBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        }
        else if (cameraBoundYBot < -mapHeightBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        }
        if (cameraBoundXRight > mapWidthBound)
        {
            transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);

        }
        else if (cameraBoundXLeft < -mapWidthBound)
        {
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
        }
    }

    // Keeps the camera in zoomed position
    void Paning()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
    }
}
