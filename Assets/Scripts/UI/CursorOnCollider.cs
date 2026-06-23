using System;
using UnityEngine;

public class CursorOnCollider : MonoBehaviour
{
    public bool CheckCursorCollider(string layerMaskName, float radius = 1.5f)
    {
        LayerMask layerMask = LayerMask.GetMask(layerMaskName);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.CircleCast(ray.origin, radius, ray.direction, Mathf.Infinity, layerMask);

        Debug.Log(radius);

        return hit;
    }
}
