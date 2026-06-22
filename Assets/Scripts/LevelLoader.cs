using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    [SerializeField] float transitionTime = 1f;

    public void LoadTargetScene(string targetScene)
    {
        StartCoroutine(WaitAndLoadScene(targetScene));
    }

    IEnumerator WaitAndLoadScene (string targetScene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(targetScene);
    }
}
