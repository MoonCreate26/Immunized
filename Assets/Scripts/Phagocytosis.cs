using Pathfinding;
using UnityEngine;
using System.Collections;

public class Phagocytosis : MonoBehaviour
{
    public Rigidbody2D rb;
    public AIDestinationSetter aIDestinationSetter;
    public SpriteRenderer spriteRenderer;
    public GameObject detectRadius;
    public Death death;
    
    
    public int hypoPhagiaLimit = 7;

    [Range(0.0f, 10.0f)] public float knockbackForce = 3f;
    [Range(0.0f, 1f)] public float PhagocytosisChance = 0.7f;

    [SerializeField] GameObject corpse;
    

    public bool sampling = false;

    int killCount;
    bool isHypoPhagia = false;
    bool samplingStarted;

    DendriticCell dendriticCell;

    void Start()
    {
        if(sampling)
        {
            dendriticCell = GetComponent<DendriticCell>();
        }
    }

    void Update()
    {
        if(killCount > hypoPhagiaLimit)
        {
            hypoPhagia();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Pathogen") && !isHypoPhagia)
        {
            Engulf(other.collider, other);
        }        
    }

    void Engulf(Collider2D pathogen, Collision2D collision)
    {
        PathogenicBehavior behavior = pathogen.gameObject.GetComponent<PathogenicBehavior>();

        if(sampling && samplingStarted && behavior.pathogenName != dendriticCell.newImmuneTarget)
        {
            return;
        }

        //Successful devouring of pathogen
        if (Random.value < PhagocytosisChance)
        {
            //Disable Bacteria
            behavior.Disable();

            //Attract Bacteria
            behavior.attracter = transform;

            killCount++;
            
            if(sampling)
            {
                dendriticCell.incrementSampleCount();

                if(!samplingStarted)
                {
                    dendriticCell.GetSample(behavior);
                    samplingStarted = true;
                }
            }

            if(behavior.pathogenName == "Tuberculosis" && !sampling)
            {
                death.additionalInstantiation = corpse;
                Destroy(gameObject, 10f);
            }
        }

        //Pathogen slips off
        else if (Random.value >= PhagocytosisChance && behavior.attracter == null)
        {
            pathogen.gameObject.GetComponent<Idle>().idleTimer = 7f;

            Vector2 relativeVelocity = collision.relativeVelocity;
            Vector2 knockbackDirection = -relativeVelocity.normalized; 

            Rigidbody2D pathogenRb = pathogen.gameObject.GetComponent<Rigidbody2D>();

            pathogenRb.velocity = Vector2.zero;
            pathogenRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        else
        {

        }
    }

    void hypoPhagia()
    {
        aIDestinationSetter.enabled = false;
        isHypoPhagia = true;
        spriteRenderer.color = Color.blue;
        detectRadius.SetActive(false);
        Destroy(gameObject, 5f);
    }
}
