using UnityEngine;

public class Idle : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 5f;
    public float rotationSpeed = 5f; // Adjust this value to control rotation speed
    public float idleTimer;
    public bool rotateOnStart = true;


    private Rigidbody2D rb;
    private Quaternion targetRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rotateOnStart)
        {
            idleTimer = 0;
        }
    }

    private void Update()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0)
        {
            rb.angularVelocity = 0;

            float randomAngle = Random.Range(0, Mathf.PI * 2); // Random angle in radians
            Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            rb.velocity = direction * moveSpeed;

            // Calculate target rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            targetRotation = Quaternion.Euler(0, 0, angle);

            idleTimer = Random.Range(idleTimeMin, idleTimeMax);
        }

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
