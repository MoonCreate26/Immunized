using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BCell : MonoBehaviour
{
    public GameObject antibody;
    public float productionRate = 1f;

    GameManager gameManager;
    UniversalPathogenDictionary pathogenDictionary;
    string antibodyTarget;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        pathogenDictionary = FindObjectOfType<UniversalPathogenDictionary>();

        antibodyTarget = gameManager.adaptiveImmuneTarget;

        StartCoroutine(produceAntibody());
    }

    IEnumerator produceAntibody()
    {
        yield return new WaitForSeconds(productionRate);

        GameObject newAntibody = Instantiate(antibody, transform.position, transform.rotation);
        newAntibody.GetComponent<Antibody>().targetName = antibodyTarget;
        newAntibody.GetComponentInChildren<SpriteRenderer>().color = (Color)pathogenDictionary.GetPathogenInfo(antibodyTarget, "Color");

        StartCoroutine(produceAntibody());
    }
}
