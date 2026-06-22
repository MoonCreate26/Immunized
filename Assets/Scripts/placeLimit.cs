using UnityEngine;

public class placeLimit : MonoBehaviour
{
    public DragDrop limitTarget;

    void OnDestroy()
    {
        limitTarget.currentPlacedCount--;
    }
}
