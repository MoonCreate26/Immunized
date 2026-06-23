using UnityEngine;

public class SignalMolecule : MonoBehaviour
{
    public Transform information;
    public float speed;
    
    [SerializeField] float delay;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Random.insideUnitCircle.normalized * speed);

        Destroy(gameObject, delay);
    }
}
