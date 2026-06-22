using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BacterialBehavior : MonoBehaviour
{
    public float timeToReplicate = 10f;
    public int replicatableCount = 1;
    public int countLimit = 20;

    float elapsedTime = 0f;
    int count = 0;
    InteractUI interactUI;

    void Start()
    {
        interactUI = FindObjectOfType<InteractUI>();

        mutate();
        StartCoroutine(replicateWait());
    }

    void mutate()
    {
        timeToReplicate = Random.Range(Mathf.Clamp(timeToReplicate - 1f, 3f, Mathf.Infinity), timeToReplicate + 1f);
    }

    IEnumerator replicateWait()
    {
        while (elapsedTime < timeToReplicate || checkPathogenCount())
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if(!checkPathogenCount() && replicatableCount > 0)
        {
            replicate();
        }
    }

    void replicate()
    {
        GameObject newBacteria = Instantiate(gameObject, transform.position, transform.rotation);
        newBacteria.GetComponent<BacterialBehavior>().timeToReplicate = timeToReplicate;
        newBacteria.GetComponentInChildren<SpriteRenderer>().material = interactUI.unlitDefault;

        replicatableCount--;

        if(replicatableCount > 0 && !checkPathogenCount())
        {
            elapsedTime = 0f;
            StartCoroutine(replicateWait());
        }

    }

    //Check if number of copies of specific pathogen exceeds accepted value
    bool checkPathogenCount()
    {
        PathogenicBehavior[] pathogens = FindObjectsOfType<PathogenicBehavior>();
        PathogenicBehavior thisPathogen = gameObject.GetComponent<PathogenicBehavior>();

        count = 0;
    
        foreach (PathogenicBehavior pathogen in pathogens)
        {
            // Check for the target string in the script
            if (pathogen.pathogenName == thisPathogen.pathogenName)
            {
                count++;
            }
        }

        if(count > countLimit)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
