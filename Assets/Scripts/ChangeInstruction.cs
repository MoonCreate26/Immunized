using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeInstruction : MonoBehaviour
{
    [SerializeField] TMP_Text instruction;

    public void change(string text)
    {
        instruction.gameObject.SetActive(false);

        instruction.text = text;
        instruction.gameObject.SetActive(true);

        StartCoroutine(disableAfterAnimation());
    }

    IEnumerator disableAfterAnimation()
    {
        yield return new WaitForSeconds(7f);

        instruction.gameObject.SetActive(false);
    }
}
