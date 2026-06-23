using UnityEngine;

public class NET : MonoBehaviour
{
    public int durationLimit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pathogen"))
        {
            PathogenicBehavior pathogen = other.GetComponent<PathogenicBehavior>();
            pathogen.Disable();
            pathogen.tag = "Dead";
        }        
    }
}
