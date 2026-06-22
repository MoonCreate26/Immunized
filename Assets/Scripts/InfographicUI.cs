using UnityEngine;
using System.Collections;

public class InfographicUI : MonoBehaviour
{
    public Animator infographicAnimator;
    public bool infoCurrentlyShown = false;
    public GameObject informations;

    float currentTime;
    Transform[] infographics;
    
    [SerializeField] float acceptedTimeInterval;

    void Start()
    {
        infographics = informations.GetComponentsInChildren<Transform>();
        disableAllInfographic();

        StopAllCoroutines();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && infoCurrentlyShown)
        {
            StartCoroutine(StartCountDown());
        }
    }

    public void ShowInfo()
    {
        infographicAnimator.SetTrigger("ShowInfographic");
        infoCurrentlyShown = true;
    }

    IEnumerator StartCountDown()
    {
        currentTime = Time.time;

        while (!Input.GetMouseButtonUp(0))
        {
            yield return null;
        }

        if(Time.time - currentTime < acceptedTimeInterval)
        {
            infographicAnimator.SetTrigger("HideInfographic");
            infoCurrentlyShown = false;

            yield return new WaitForSeconds(1.3f);

            disableAllInfographic();
        }
    }

    public void enableInfographic(Transform target)
    {
        target.gameObject.SetActive(true);
    }

    public void disableAllInfographic()
    {
        foreach (Transform infographic in infographics)
        {
            if (infographic != informations.transform && infographic.CompareTag("Infographic")) // Avoid disabling the parent itself
            {
                infographic.gameObject.SetActive(false);
            }
        }
    }
}
