using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeutrophilDetonation : MonoBehaviour
{
    public GameObject NET;
    public GameObject neutrophil;
    public int requiredPathogenCount = 4;
    public int detonateDelay = 3;
    public float apoptosisDelay = 15;

	public SpriteRenderer spriteRenderer;
	public Material materialGUItext;
	public Material materialSpritesDefault;

    int pathogenInRange = 0;
    bool sequenceStarted = false;

    void Start()
    {
        StartCoroutine(neutrophilApoptosis());
    }

    IEnumerator neutrophilApoptosis()
    {
        yield return new WaitForSeconds(apoptosisDelay);

        if(!sequenceStarted)
        {
            StartCoroutine(detonateSequence());
            sequenceStarted = true;
        }
    }

    void Update()
    {
        if(pathogenInRange >= requiredPathogenCount && !sequenceStarted)
        {
            StartCoroutine(detonateSequence());
            sequenceStarted = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Pathogen"))
        {
            pathogenInRange++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Pathogen"))
        {
            pathogenInRange--;
        }
    }

    IEnumerator detonateSequence()
    {
        StartCoroutine(detonateWaitEffect());

        yield return new WaitForSeconds(detonateDelay);

        Instantiate(NET, transform.position, transform.rotation);
        Destroy(neutrophil);
    }

    IEnumerator detonateWaitEffect()
    {
        whiteSprite();

        yield return new WaitForSeconds(0.25f);

        normalSprite();

        yield return new WaitForSeconds(0.25f);

        StartCoroutine(detonateWaitEffect());
    }

    void whiteSprite()
    {
	    spriteRenderer.material = materialGUItext;
    }

    void normalSprite()
    {
	    spriteRenderer.material = materialSpritesDefault;
	}

}
