using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<PathogenList> WavesList;
    public List<PathogenList> OneTimeList;

    [HideInInspector] public int currentSpawnCount;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    public float maxSpawnAngle = 45f;

    [SerializeField] bool useWaves = true;

    PathogenList currentPathogenList;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        refreshSpawner();
    }

    public void refreshSpawner()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        if(useWaves)
        {
            currentPathogenList = WavesList[gameManager.currentWave];
            currentSpawnCount = WavesList[gameManager.currentWave].waveSpawnCount;
        }

        else
        {
            currentPathogenList = OneTimeList[0];
            currentSpawnCount = OneTimeList[0].waveSpawnCount;
        }

        while (currentSpawnCount > 0)
        {
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            int randomIndex = Random.Range(0, currentPathogenList.Pathogens.Count);

            yield return new WaitForSeconds(randomInterval);

            Quaternion randomQuaternion = Quaternion.Euler(0f, 0f, Random.Range(-maxSpawnAngle, maxSpawnAngle));
            Quaternion spawnerQuaternion = transform.rotation * Quaternion.Euler(0f, 0f, 180f) * randomQuaternion;

            GameObject newObject = Instantiate(currentPathogenList.Pathogens[randomIndex], transform.position, spawnerQuaternion);
            Idle idle = newObject.GetComponent<Idle>();

            idle.rotateOnStart = false;
            idle.idleTimer = 1f;

            Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();

            rb.velocity = newObject.transform.up * (idle.moveSpeed);            

            if(!useWaves)
            {
                newObject.layer = LayerMask.NameToLayer("Pathogen");
            }

            currentSpawnCount--;
        }

        if(!useWaves)
        {
            Destroy(gameObject);

            yield break;
        }

        gameManager.CheckSpawnerEmptied();
    }

}

[System.Serializable]
public class PathogenList
{
    public List<GameObject> Pathogens;
    public int waveSpawnCount;
}