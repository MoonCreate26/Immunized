using TMPro;
using UnityEngine;

public class CopyCost : MonoBehaviour
{
    [SerializeField] DragDrop icon;
    [SerializeField] TMP_Text costText;

    void Start()
    {
        costText.text = "" + icon.cost;
    }
}
