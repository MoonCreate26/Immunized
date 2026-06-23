using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCount : MonoBehaviour
{
    public Necrosis necrosis;
    public float health = 30f;
    public bool takeDamageFromPathogen = true;
    public SpriteRenderer spriteRenderer;
    public GameObject signal;
    
    [SerializeField] bool EmitSignal = false;
    [SerializeField] bool isCivillianCell = false;
    [Range(0f, 100f)] [SerializeField] float signalChance;

    GameManager gameManager;
    Color defaultColor;
    int coroutineCount;
    bool cellIsKilled;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        defaultColor = spriteRenderer.color;
    }

    void OnParticleCollision(GameObject other)
    {
        if(takeDamageFromPathogen && other.CompareTag("PathogenParticle") || other.CompareTag("ImmuneParticle"))
        {
            health--;
            coroutineCount++;
            StartCoroutine(damageEffect());

            if(EmitSignal && Random.value < signalChance / 100)
            {
                ReleaseSignal();
            }
        }
    }

    void Update()
    {
        if(health < 0 && !cellIsKilled)
        {
            cellIsKilled = true;

            if(isCivillianCell)
            {
                gameManager.cellKilled();
            }

            if(necrosis != null)
            {
                necrosis.enabled = true;
                Destroy(this);
            }

            else if(necrosis == null)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator damageEffect()
    {
        spriteRenderer.color = Color.gray;

        yield return new WaitForSeconds(0.3f);

        if(coroutineCount <= 1)
        {
            spriteRenderer.color = Color.white;
        }

        coroutineCount--;
    }

    void ReleaseSignal()
    {
        GameObject newSignal = Instantiate(signal, transform.position, Quaternion.identity);
        newSignal.GetComponent<SignalMolecule>().information = transform;
    }
}

