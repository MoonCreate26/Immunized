using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSpawner : MonoBehaviour
{
    [SerializeField] GameObject entity;
    [SerializeField] int count;
    [SerializeField] float maxSpawnAngle;
    [SerializeField] float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(spawnDelay);

        for(int i = 0; i < count; i++)
        {
            Quaternion randomQuaternion = Quaternion.Euler(0f, 0f, Random.Range(-maxSpawnAngle, maxSpawnAngle));
            Quaternion spawnerQuaternion = transform.rotation * Quaternion.Euler(0f, 0f, 180f) * randomQuaternion;

            GameObject instance = Instantiate(entity, transform.position, spawnerQuaternion);

            Idle idle = instance.GetComponent<Idle>();
            idle.rotateOnStart = false;
            idle.idleTimer = 1f;

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            rb.velocity = instance.transform.up * (idle.moveSpeed); 
        } 
    }
}
