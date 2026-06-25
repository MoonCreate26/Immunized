using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Antibody : MonoBehaviour
{
    public int targetIdx;
    public Search search;
    public float lifeTime = 15f;

    PathogenicBehavior behavior;
    FixedJoint2D fixedJoint2D;
    Transform target;

    void Start()
    {
        StartCoroutine(delayedDeath());

        search.filterModeActive = true;
        search.filterIdx = targetIdx;
    }

    void Update()
    {
        if (search.destinationSetter.target != null)
        {
            target = search.destinationSetter.target;
            search.searchDisabled = true;
        }

        if (search.destinationSetter.target == null)
        {
            search.searchDisabled = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.transform == target)
        {
            fixedJoint2D = gameObject.AddComponent<FixedJoint2D>();
            fixedJoint2D.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
            fixedJoint2D.enableCollision = true;

            behavior = other.gameObject.GetComponent<PathogenicBehavior>();
            behavior.Disable();
            behavior.gameObject.tag = "Dead";

            search.searchDisabled = true;
        }
    }

    IEnumerator delayedDeath()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
