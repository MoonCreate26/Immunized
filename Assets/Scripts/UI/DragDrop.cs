using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public CanvasGroup thisCanvasGroup;
    public GameManager gameManager;

    public PanZoom panZoom;
    public CursorOnUI cursorOnUI;
    public CursorOnCollider cursorOnCollider;
    public GoToTransform costDisplay;

    public GameObject immuneCell;

    public bool adaptive;
    public int cost;
    public int placeableCount;

    public ScrollRect scrollRect;
 
    [HideInInspector] public bool purchasable = true;
    [HideInInspector] public Color darkGray = new Color(0.27f, 0.27f, 0.27f);
    [HideInInspector] public int currentPlacedCount;

    [SerializeField] TMP_Text placeabilityWarning;
    [SerializeField] string specificInstruction;
    [SerializeField] float placeRadius = 1.5f;

    RectTransform rectTransform;
    Image image;
    Vector3 summonLocation;
    CanvasGroup uICanvasGroup;
    Vector2 defaultPosition;

    bool placeable;
    bool mouseIsOutside;
    bool updateBlock = false;
    bool FirstTimePlaced = true;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
        uICanvasGroup = canvas.gameObject.GetComponent<CanvasGroup>();

        defaultPosition = transform.position;

        thisCanvasGroup.ignoreParentGroups = false;
        uICanvasGroup.alpha = 1f;

        scrollRect.vertical = true;

        if(purchasable)
        {
            image.color = Color.white;
        }

        else
        {
            image.color = darkGray;
        }
    }

    void Update()
    {
        if(updateBlock)
        {
            return;
        }

        defaultPosition = transform.position;

        if(gameManager.resources < cost || adaptive && gameManager.adaptiveImmuneTarget == -1)
        {
            purchasable = false;
            image.color = darkGray;
        }

        else
        {
            purchasable = true;
            image.color = Color.white;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        costDisplay.isOnDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!purchasable)
        {
            return;
        }

        updateBlock = true;

        thisCanvasGroup.ignoreParentGroups = true;
        uICanvasGroup.alpha = 0.3f;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Vector2 view = Camera.main.ScreenToViewportPoint( Input.mousePosition );
        mouseIsOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        
        //cursorOnUI only returns true against UI Object with tag "UI"
        if(!cursorOnCollider.CheckCursorCollider("Cells", placeRadius) && !mouseIsOutside && purchasable && (placeableCount == 0 || placeableCount > currentPlacedCount) && !cursorOnUI.CheckCursorUI())
        {
            placeable = true;
            image.color = Color.white;
            placeabilityWarning.enabled = false;
        }

        else
        {
            placeable = false;
            image.color = Color.gray;
            
            if(cursorOnUI.CheckCursorUI())
            {
                return;
            }
            
            placeabilityWarning.enabled = true;

            if(cursorOnCollider.CheckCursorCollider("Cells", placeRadius))
            {
                placeabilityWarning.text = "Cannot Overlap With Cells";
            }

            else if(placeableCount <= currentPlacedCount)
            {
                placeabilityWarning.text = "Maximum number of this cell reached";
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cursorOnUI.overridePanZoom(true);
        panZoom.enabled = false;
        scrollRect.vertical = false;

        //Centers sprite on the cursor
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        costDisplay.isOnDrag = false;

        updateBlock = false;

        thisCanvasGroup.ignoreParentGroups = false;
        uICanvasGroup.alpha = 1f;

        cursorOnUI.overridePanZoom(false);

        summonLocation = Camera.main.ScreenToWorldPoint(transform.position);
        summonLocation.z = 0f;

        if(placeable && purchasable)
        {
            SummonImmune();
            gameManager.changeResource(-cost);
            currentPlacedCount++;

            if(FirstTimePlaced && specificInstruction != "")
            {
                gameManager.setNewInstruction(specificInstruction);
            }

            FirstTimePlaced = false;
        }

        transform.position = defaultPosition;
        scrollRect.vertical = true;

        placeabilityWarning.enabled = false;
    }

    void SummonImmune()
    {
        GameObject newImmuneCell = Instantiate(immuneCell, summonLocation, transform.rotation);

        if(newImmuneCell.TryGetComponent<placeLimit>(out placeLimit PlaceLimit))
        {
            PlaceLimit.limitTarget = this;
        }
    }
}
