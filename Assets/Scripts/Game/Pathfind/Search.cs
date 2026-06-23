using Pathfinding;
using UnityEngine;

public class Search : MonoBehaviour
{
    public AIDestinationSetter destinationSetter;
    public bool ignoreDead = false;
    public bool searchDisabled = false;

    public bool filterModeActive = false;
    public string filterName;

    void OnTriggerStay2D(Collider2D other)
    {
        PathogenicBehavior behavior = other.GetComponent<PathogenicBehavior>();

        if(!(other.CompareTag("Pathogen") || !ignoreDead && other.CompareTag("Dead")) || searchDisabled)
        {
            return;
        }

        if (filterModeActive && behavior.pathogenName == filterName)
        {
            destinationSetter.target = other.transform;
        }

        else if (!filterModeActive && !behavior.invisible)
        {
            destinationSetter.target = other.transform;
        }
    }
}
