using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class WorldItem : MonoBehaviour
{
    Vector3 mousePosition;
    private bool isDragged = false;
    private Slot hoveredSlot;

    public InventoryManager inventoryManager;
    public ItemSO itemSO;

    public static event System.Action<WorldItem> OnDragEnded;

    private Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        isDragged = true;
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    private void OnMouseDrag()
    {
        if (isDragged)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        }
    }

    private void OnMouseUp()
    {
        isDragged = false;
        OnDragEnded?.Invoke(this);

        if (IsMouseOverSlot())
        {
            // Spawn UI item and remove 3D object
            GameObject uiItem = Instantiate(itemSO.iconPrefab, hoveredSlot.transform);
            uiItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            InventoryItem invItem = uiItem.GetComponent<InventoryItem>();
            invItem.Setup(itemSO);

            hoveredSlot.currentItem = uiItem;

            Destroy(gameObject); // Remove the 3D object
            return;
        }
    }

    bool IsMouseOverSlot()
    {
        hoveredSlot = null;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                hoveredSlot = result.gameObject.GetComponent<Slot>();
                return true;
            }
                
        }

        return false;
    }
}

