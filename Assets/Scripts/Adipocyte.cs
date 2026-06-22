using UnityEngine;

public class Adipocyte : MonoBehaviour
{
    public int energyStored;
    public float productionSpeed;

    GameManager gameManager;
    Bounds bound;
    bool efficiencyIncreased;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.resourcesCapacity += energyStored; 

        bound = gameObject.GetComponent<Collider2D>().bounds;
        AstarPath.active.UpdateGraphs(bound);

        if(gameManager.generationInterval - productionSpeed >= gameManager.minGenerationInterval)
        {
            gameManager.generationInterval -= productionSpeed;
            efficiencyIncreased = true;
        }
    }

    void OnDestroy()
    {
        AstarPath.active.UpdateGraphs(bound);

        gameManager.resourcesCapacity -= energyStored;

        if(efficiencyIncreased)
        {
            gameManager.generationInterval += productionSpeed;
        }
    }
}
