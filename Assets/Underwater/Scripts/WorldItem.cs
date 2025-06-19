using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldItem : MonoBehaviour
{
    Vector3 mousePosition;
    private bool _isDragged = false;
    private Slot _hoveredSlot;
    private Rigidbody _rb;

    public InventoryManager inventoryManager;
    public ItemSO itemSO;

    public static event System.Action<WorldItem> OnDragEnded;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        _isDragged = false;
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    private void OnMouseDrag()
    {
        if (_isDragged)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        }
    }

    private void OnMouseUp()
    {
        _isDragged = false;
        OnDragEnded?.Invoke(this);

        if (IsMouseOverSlot())
        {
            // Spawn UI item and remove 3D object
            GameObject uiItem = Instantiate(itemSO.iconPrefab, _hoveredSlot.transform);
            uiItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            InventoryItem invItem = uiItem.GetComponent<InventoryItem>();
            invItem.Setup(itemSO);

            //invItem.originalParent = hoveredSlot.transform;

            _hoveredSlot.currentItem = uiItem;

            Destroy(gameObject); // Remove the 3D object
            return;
        }
    }

    bool IsMouseOverSlot()
    {
        _hoveredSlot = null;

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
                _hoveredSlot = result.gameObject.GetComponent<Slot>();
                return true;
            }
                
        }

        return false;
    }
}

