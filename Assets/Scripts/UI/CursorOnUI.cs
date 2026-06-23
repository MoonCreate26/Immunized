using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Pathfinding.Examples;

public class CursorOnUI : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;
    public bool isCursorOnUI = false;
    public PanZoom panZoom;

    PointerEventData pointerEventData;
    List<RaycastResult> results;
    bool overridden = false;

    void Start()
    {
        pointerEventData = new PointerEventData(eventSystem);
        results = new List<RaycastResult>();
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && CheckCursorUI())
        {
            panZoom.enabled = false;
        }

        else if(!overridden)
        {
            panZoom.enabled = true;
        }
    }

    //cursorOnUI only returns true against UI Object with tag "UI"
    public bool CheckCursorUI()
    {
        //Set the Pointer Event Position to that of the mouse position
        pointerEventData.position = Input.mousePosition;

        //Clear result list
        results.Clear();
        
        //Raycast using the Graphics Raycaster and mouse click position
        graphicRaycaster.Raycast(pointerEventData, results);

        //Update isCursorOnUI
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("UI"))
            {
                // Object has the required tag
                return true;
            }
        }

        return false;
    }

    public void overridePanZoom(bool isOverridden)
    {
        overridden = isOverridden;
    }
}