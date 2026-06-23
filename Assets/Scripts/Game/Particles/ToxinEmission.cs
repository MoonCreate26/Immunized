using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class ToxinEmission : MonoBehaviour
{
    public ParticleSystem toxin;
    public AIDestinationSetter aIDestinationSetter;
    public AIPath aIPath;
    
    float fastSpeed = 3f;
    float slowSpeed = 0.75f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.transform == aIDestinationSetter.target)
        {
            aIPath.maxSpeed = slowSpeed;
            ReleaseToxin(true);
        }
        
        else if (other.gameObject.CompareTag("Pathogen"))
        {
            aIPath.maxSpeed = (fastSpeed + slowSpeed) / 2;
        }

        else
        {
            aIPath.maxSpeed = fastSpeed;
        }
    }

    void Update()
    {
        if(aIDestinationSetter.target == null)
        {
            ReleaseToxin(false);
            aIPath.maxSpeed = fastSpeed;
        }
    }

    void ReleaseToxin(bool emitting)
    {
        if(emitting)
        {
            toxin.Play();
        }

        else
        {
            toxin.Stop();
        }
    }
}
