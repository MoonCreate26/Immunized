using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathogenicBehavior : MonoBehaviour
{
    public string pathogenType;
    public int pathogenIdx;
    public int pathogenResource;
    [Range(0f, 100f)] public float resourceDropChance;
    public float deathDelayAmount = 5f;
    public bool invisible = false;
    public Material grayScale;

    float speed = 5f;

    [HideInInspector] public Transform attracter;

    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    DamageCount damageCount;
    float deafultHealth;
    bool invisibleRemoved = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        damageCount = gameObject.GetComponent<DamageCount>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();


        deafultHealth = damageCount.health;

        if(invisible)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.25f);
        }
    }

    void Update()
    {
        if(gameObject.tag == "Dead")
        {
            spriteRenderer.material = grayScale;

            StartCoroutine(DelayedDeath());
        }

        if(!invisible && !invisibleRemoved)
        {
            spriteRenderer.color = Color.white;
            invisibleRemoved = true;
        }

        else if(damageCount.health < deafultHealth)
        {
            invisible = false;
        }

        if(attracter != null)
        {
            Attracted(attracter);
        }
    }

    IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(deathDelayAmount);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.gameObject.CompareTag("Immune") && invisible && other.collider.gameObject.GetComponent<Antibody>() == null)
        {
            invisible = false;
            //StartCoroutine(DelayedInvisibilityRemoval());
        }
    }

    IEnumerator DelayedInvisibilityRemoval()
    {
        yield return new WaitForSeconds(0.5f);

        invisible = false;
    }

    void OnDestroy()
    {
        if(Random.value < resourceDropChance / 100f)
        {
            gameManager.changeResource(pathogenResource);
        }

        gameManager.CheckPathogenEliminated();
    }

    public void Disable()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        //Remove Collision
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        rb.isKinematic = true;


        //Disable Movement
        gameObject.GetComponentInChildren<Idle>().enabled = false;

        //Destroy Toxin
        Destroy(gameObject.GetComponentInChildren<ParticleSystem>().gameObject);

        //Set velocity to 0
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    void Attracted(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
