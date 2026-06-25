using System;
using Unity.VisualScripting;
using UnityEngine;

public class ImmuneCellBehavior : MonoBehaviour
{
    [SerializeField] String message;
    GameManager gameManager;
    Search search;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.setNewInstruction("Message");

        search = gameObject.GetComponentInChildren<Search>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        if(otherObject.TryGetComponent<SignalMolecule>(out SignalMolecule signalMolecule))
        {
            search.destinationSetter.target = signalMolecule.information;
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform == search.destinationSetter.target)// && otherObject.layer == LayerMask.NameToLayer("Cells"))
        {
            search.destinationSetter.target = null;
        }
    }
}
