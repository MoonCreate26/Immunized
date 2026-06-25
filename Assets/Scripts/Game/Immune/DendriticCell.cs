using Pathfinding;
using UnityEngine;

public class DendriticCell : MonoBehaviour
{
    public Search search;
    public int requiredSampleCount = 3;
    public Transform lymphNode;
    
    [HideInInspector] public int newImmuneTarget;

    GameManager gameManager;
    ImmuneCellBehavior immuneCellBehavior;
    int sampleCount = 0;
    bool samplingEnded;

    void Start()
    {
        immuneCellBehavior = GetComponent<ImmuneCellBehavior>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(sampleCount >= requiredSampleCount && !samplingEnded)
        {
            search.destinationSetter.target = FindNearestNode().transform;

            Destroy(search.gameObject);
            Destroy(GetComponent<Phagocytosis>());
            Destroy(immuneCellBehavior);
            
            gameObject.layer = LayerMask.NameToLayer("Dendritic");
            samplingEnded = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("LymphNode"))
        {
            gameManager.adaptiveImmuneTarget = newImmuneTarget;
            gameManager.showAdaptiveCellButton();
            gameManager.setNewInstruction("Target of Adaptive Immune System Updated!");

            Destroy(gameObject);
        }
    }

    public void GetSample(PathogenicBehavior behavior)
    {
        newImmuneTarget = behavior.pathogenIdx;
        search.filterModeActive = true;
        search.filterIdx = newImmuneTarget;
    }

    public void incrementSampleCount()
    {
        sampleCount++;
    }

    private GameObject FindNearestNode()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("LymphNode");

        if (taggedObjects.Length == 0)
        {
            return null; // No objects with the given tag found
        }

        GameObject nearestNode = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject taggedObject in taggedObjects)
        {
            float distance = Vector3.Distance(transform.position, taggedObject.transform.position);

            if (distance < nearestDistance)
            {
                nearestNode = taggedObject;
                nearestDistance = distance;
            }
        }

        return nearestNode;
    }
}
