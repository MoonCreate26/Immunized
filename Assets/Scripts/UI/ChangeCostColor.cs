using UnityEngine;
using UnityEngine.UI;

public class ChangeCostColor : MonoBehaviour
{
    public DragDrop referencedCellUI;

    Image image;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if(!referencedCellUI.purchasable)
        {
            image.color = referencedCellUI.darkGray;
        }

        else
        {
            image.color = Color.white;
        }
    }
}
