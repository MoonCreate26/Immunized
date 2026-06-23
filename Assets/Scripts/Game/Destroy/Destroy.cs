using Unity.VisualScripting;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kill"))
        {
            Destroy(this.gameObject);
        }
    }
}
