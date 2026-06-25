using System.Collections;
using UnityEngine;

public class DestroyOnAwake : MonoBehaviour
{
    [SerializeField] float delay = 0f;

    void Start()
    {
        StartCoroutine(destroyOnDelay());
    }

    IEnumerator destroyOnDelay()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
