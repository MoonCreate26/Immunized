using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float minInitialSpawnDelay = 1f;
    public float maxInitialSpawnDelay = 3f;
    public float maxSpawnAngle = 45f;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        refreshSpawner();
    }

    public void refreshSpawner()
    {
        StartCoroutine(SpawnObjects(true));
    }

    IEnumerator SpawnObjects(bool initial_delay = false)
    {
        if(initial_delay)
        {
            yield return new WaitForSeconds(Random.Range(minInitialSpawnDelay, maxInitialSpawnDelay));
        }

        int randomIndex = Random.Range(0, gameManager.CheckPathogenUnlocked() + 1);

        // Adds initial momentum to spawned object; angled direction
        Quaternion randomQuaternion = Quaternion.Euler(0f, 0f, Random.Range(-maxSpawnAngle, maxSpawnAngle));
        Quaternion spawnerQuaternion = transform.rotation * Quaternion.Euler(0f, 0f, 180f) * randomQuaternion;

        GameObject newObject = Instantiate(gameManager.pathogenPrefabs[randomIndex], transform.position, spawnerQuaternion);
        Idle idle = newObject.GetComponent<Idle>();

        idle.rotateOnStart = false;
        idle.idleTimer = 1f;

        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();

        rb.velocity = newObject.transform.up * (idle.moveSpeed);            

        // Delay
        yield return new WaitForSeconds((float)gameManager.pathogenDictionary.GetPathogenInfo(randomIndex, "Delay") / gameManager.difficultyMultiplier);


        // Start next spawn cycle
        StartCoroutine(SpawnObjects());
    }

}