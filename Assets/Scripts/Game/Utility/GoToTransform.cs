using UnityEngine;

public class GoToTransform : MonoBehaviour
{
    public Transform targetTransform;
    public float offsetX;
    public float offsetY;
    [HideInInspector] public bool isOnDrag; 

    Vector2 newPosition;

    void Update()
    {
        if(!isOnDrag)
        {
            newPosition = new Vector2(targetTransform.position.x + offsetX, targetTransform.position.y + offsetY);
            transform.position = newPosition;
        }
    } 
}
