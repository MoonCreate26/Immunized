using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathogenicBehavior : MonoBehaviour
{
    // Base Identifier
    public string pathogenType;
    public int pathogenIdx;

    // Resources
    public int pathogenResource;
    [Range(0f, 100f)] public float resourceDropChance;
    
    // Status
    float deafultHealth;
    float speed = 5f;
    public int countLimit = 20;

    // Invisibility
    public bool invisible = false;
    bool invisibleRemoved = false;
    
    // Death
    public float deathDelayAmount = 5f;
    public Material grayScale;

    // Pathfind
    [HideInInspector] public Transform attracter;

    // References
    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    DamageCount damageCount;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        damageCount = gameObject.GetComponent<DamageCount>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        gameManager.spawnCount++;
        gameManager.pathogenCount[pathogenIdx]++;

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

    
    //Check if number of copies of specific pathogen exceeds accepted value
    public bool checkPathogenCount()
    {
        return gameManager.pathogenCount[pathogenIdx] > countLimit;
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

        gameManager.spawnCount--;
        gameManager.pathogenCount[pathogenIdx]--;
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
