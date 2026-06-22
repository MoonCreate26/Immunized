using UnityEngine;

public class RandomizedEmission : MonoBehaviour
{
    public float noToxinMinDuration = 5f;
    public float noToxinMaxDuration = 7f;

    public float toxinMinDuration = 5f;
    public float toxinMaxDuration = 7f;

    public ParticleSystem toxin;

    bool emitting = false;
    float nextFlipTime;

    void Start ()
    {
        toxin.Pause();
        nextFlipTime = Time.time + Random.Range(noToxinMinDuration, noToxinMaxDuration);
    }

    void Update()
    {
        if(Time.time >= nextFlipTime)
        {
            emitting = !emitting;
            setNextFlipTiime(emitting);

            if(emitting)
            {
                toxin.Play();
            }

            else
            {
                toxin.Stop();
            }
        }
    }

    void setNextFlipTiime(bool isEmitting)
    {
        if(isEmitting)
        {
            nextFlipTime = Time.time + Random.Range(toxinMinDuration, toxinMaxDuration);
        }

        else
        {
            nextFlipTime = Time.time + Random.Range(noToxinMinDuration, noToxinMaxDuration);
        }
    }
}
