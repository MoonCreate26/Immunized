using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform transformToCopy;

    void Start()
    {
        transform.position = transformToCopy.position;
        transform.rotation = transformToCopy.rotation;
    }
}
