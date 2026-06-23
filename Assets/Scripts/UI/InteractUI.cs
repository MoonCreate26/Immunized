using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InteractUI : MonoBehaviour
{    
    public Camera mainCamera;
    public PanZoom panZoom;

    public Material unlitDefault;
    public Material outline;

    public GameObject UICanvas;
    public CursorOnUI cursorOnUI;
    public UISlideControl uISlideControl;

    int layerMask;
    Transform targetTransform = null;

    void Start()
    {
        layerMask = LayerMask.GetMask("Immune", "Pathogen");
    }

    void Update()
    {
        if(targetTransform != null)
        {
            //this.gameObject.transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, -10);
            //mainCamera.orthographicSize = 5;

            //panZoom.RemoveLimit();

            //uISlideControl.SetCurrentBool(false);
        }

        else
        {
            uISlideControl.SetCurrentBool(true);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(targetTransform != null)
            {
                targetTransform.gameObject.GetComponentInChildren<SpriteRenderer>().material = unlitDefault;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

            if (hit)
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if(cursorOnUI.CheckCursorUI()) return;

                if (hitGameObject.CompareTag("Immune") || hitGameObject.CompareTag("Pathogen"))
                {
                    targetTransform = hit.transform;
                    targetTransform.gameObject.GetComponentInChildren<SpriteRenderer>().material = outline;
                }
            }

            else
            {
                targetTransform = null;
                panZoom.SetLimit();
            }
        }

    }
}
