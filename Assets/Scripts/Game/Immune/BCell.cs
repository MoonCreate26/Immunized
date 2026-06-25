using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BCell : MonoBehaviour
{
    public GameObject antibody;
    public float productionRate = 1f;

    GameManager gameManager;
    UniversalPathogenDictionary dictionary;
    int antibodyTarget;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dictionary = gameManager.pathogenDictionary;

        antibodyTarget = gameManager.adaptiveImmuneTarget;

        StartCoroutine(produceAntibody());
    }

    IEnumerator produceAntibody()
    {
        yield return new WaitForSeconds(productionRate);

        GameObject newAntibody = Instantiate(antibody, transform.position, transform.rotation);
        newAntibody.GetComponent<Antibody>().targetIdx = antibodyTarget;
        newAntibody.GetComponentInChildren<SpriteRenderer>().color = (Color)dictionary.GetPathogenInfo(antibodyTarget, "Color");

        StartCoroutine(produceAntibody());
    }
}
