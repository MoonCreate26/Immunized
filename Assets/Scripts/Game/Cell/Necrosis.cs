using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necrosis : MonoBehaviour
{
    public GameObject cellShard;

    void Start()
    {
        Bounds bound = gameObject.GetComponent<Collider2D>().bounds;

        Destroy(gameObject);
        Instantiate(cellShard, gameObject.transform.position, gameObject.transform.rotation);
        AstarPath.active.UpdateGraphs(bound);
    }
}
