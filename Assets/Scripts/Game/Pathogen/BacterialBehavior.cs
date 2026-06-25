using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BacterialBehavior : MonoBehaviour
{
    // Replication
    public float timeToReplicate = 10f;
    public int replicatableCount = 1;
    public bool childReplication = false;
    float elapsedTime = 0f;
    int count = 0;

    // UI
    InteractUI interactUI;
    PathogenicBehavior baseBehavior;

    void Start()
    {
        interactUI = FindObjectOfType<InteractUI>();
        baseBehavior = GetComponent<PathogenicBehavior>();

        mutate();
        StartCoroutine(replicateWait());
    }

    void mutate()
    {
        timeToReplicate = Random.Range(Mathf.Clamp(timeToReplicate - 1f, 3f, Mathf.Infinity), timeToReplicate + 1f);
    }

    IEnumerator replicateWait()
    {
        while (elapsedTime < timeToReplicate || baseBehavior.checkPathogenCount())
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if(!baseBehavior.checkPathogenCount() && replicatableCount > 0)
        {
            replicate();
        }
    }

    void replicate()
    {
        GameObject newBacteria = Instantiate(gameObject, transform.position, transform.rotation);
        BacterialBehavior behavior = newBacteria.GetComponent<BacterialBehavior>();

        behavior.timeToReplicate = timeToReplicate;
        
        if(childReplication)
        {
            behavior.replicatableCount = replicatableCount;
        }

        else
        {
            behavior.replicatableCount = 0;
        }

        newBacteria.GetComponent<BacterialBehavior>().timeToReplicate = timeToReplicate;
        newBacteria.GetComponentInChildren<SpriteRenderer>().material = interactUI.unlitDefault;

        replicatableCount--;

        if(replicatableCount > 0 && !baseBehavior.checkPathogenCount())
        {
            elapsedTime = 0f;
            StartCoroutine(replicateWait());
        }

    }


}
