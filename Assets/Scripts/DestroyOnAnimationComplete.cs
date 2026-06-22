using UnityEngine;

public class DestroyOnAnimationComplete : MonoBehaviour
{
    public float delay = 0f;

    void Start()
    {
        float animTIme = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        Destroy(gameObject, animTIme + delay);
    }
}
