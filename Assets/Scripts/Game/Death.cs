using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject deathParticle;
    [HideInInspector] public GameObject additionalInstantiation;
    
    GameManager gameManager;

    bool isQuitting;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting && !gameManager.GameOver)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);

            if(additionalInstantiation != null)
            {
                Instantiate(additionalInstantiation, transform.position, transform.rotation);
            }
        }
    }
}
