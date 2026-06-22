using Pathfinding;
using UnityEngine;

public class ModeChange : MonoBehaviour
{
    public AIPath active;
    public Idle passive;

    public AIDestinationSetter aIDestinationSetter;

    void Update()
    {
        if (aIDestinationSetter.target == null)
        {
            active.enabled = false;
            passive.enabled = true;
        }

        else
        {
            passive.enabled = false;
            active.enabled = true;
        }
    }
}
