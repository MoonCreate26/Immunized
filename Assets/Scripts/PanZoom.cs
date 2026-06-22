using UnityEngine;

public class PanZoom : MonoBehaviour
{
    //Variables
    public GameManager gameManager;

    Vector3 touchStart; //Coordinate of where the screen was tapped
    private bool multiTouch = false; //Boolean to distinguish panning from zooming

    public float zoomOutMin = 1f; //Control minimum orthographic size (how close camera can be to the sprite)
    public float zoomOutMax = 8f; //Control maximum orthographic size (how far camera can be from the sprite)
    public float zoomSpeed = 0.03f; //Control zooming speed

    public SpriteRenderer spriteRenderer; //Reference to SpriteRender
    private float spriteMinX, spriteMaxX, spriteMinY, spriteMaxY; //Coordinates of the 4 Corners of the periodic table sprite

    private void Awake()
    {
        SetLimit();
    }

    public void SetLimit()
    {
        //transform.position is always the center.
        //bounds.size is the total size of the sprite (ex. size.y is the total height of the sprite), so
        //we divide them by 2 to get the coordinates that is from the center to the edge.

        spriteMinX = spriteRenderer.transform.position.x - spriteRenderer.bounds.size.x / 2f;
        spriteMaxX = spriteRenderer.transform.position.x + spriteRenderer.bounds.size.x / 2f;

        spriteMinY = spriteRenderer.transform.position.y - spriteRenderer.bounds.size.y / 2f;
        spriteMaxY = spriteRenderer.transform.position.y + spriteRenderer.bounds.size.y / 2f;
    }

    public void RemoveLimit()
    {
        spriteMaxX = Mathf.Infinity;
        spriteMaxY = Mathf.Infinity;

        spriteMinX = Mathf.NegativeInfinity;
        spriteMinY = Mathf.NegativeInfinity;
    }

    void Update()
    {
        if(gameManager.Paused)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            //Zoom error prevention
            multiTouch = false;

            //Obtaining the initial coordinate where the screen was touched
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //Pinch Zooming
        //Checking for # of touches detected
        if(Input.touchCount == 2)
        {
            //Zoom error prevention
            multiTouch = true;

            //Getting data from each touch
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //Previous position of each touches
            Vector2 touchZeroPreviousPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPos = touchOne.position - touchOne.deltaPosition;

            //Magnitude calculates the distance between two points
            float previousMagnitude = (touchZeroPreviousPos - touchOnePreviousPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            //Get the difference between the previous magnitude and current magnitude
            float difference = currentMagnitude - previousMagnitude;

            //zoomSpeed is to slow down the zooming
            zoom(difference * zoomSpeed);
        }

        //Panning
        else if(Input.GetMouseButton(0) && multiTouch == false)
        {
            //Obtaining direction to move the camera
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);  

            //Moving Camera
            Camera.main.transform.position = ClampCamera(Camera.main.transform.position + direction);

            
        }

        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        //Increment = how much the camera moves

        //Mathf.Clamp() Set the float to be within the min and max given.
        //If float > max, set float to max. If float < min, set float to min. If float is within the value, it stays the same.
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
    }

    //Keeping Camera within boundary
    //For more details: https://youtu.be/R6scxu1BHhs?t=491
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        //Setting the width/height of camera
        float camHeight = Camera.main.orthographicSize;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect; //aspect is ratio of the camera (Heigth:Width)

        //Setting the min/max for where camera can go
        float minX = spriteMinX + camWidth;
        float maxX = spriteMaxX - camWidth;
        float minY = spriteMinY + camHeight;
        float maxY = spriteMaxY - camHeight;

        //Coordinates that keeps camera within the boundary
        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        //Returning the value
        return new Vector3(newX, newY, targetPosition.z);
    }
}
