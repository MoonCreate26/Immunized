using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateWall : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "WallTrigger")
        {
            StartCoroutine(DelayActivation());
        }
    }

    IEnumerator DelayActivation()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Pathogen");
    }
}
