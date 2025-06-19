using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _spawnDistance = 3f; // Distance from the camera to spawn the item in the world

    public ItemSO itemSO;
    public Image itemIcon;

    private Transform _originalParent;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(ItemSO newItemSO)
    {
        itemSO = newItemSO;

        if (itemIcon != null)
        {
            itemIcon.sprite = itemSO.icon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
        transform.SetParent(transform.root); // Move to root to avoid being blocked by other UI elements
        _canvasGroup.blocksRaycasts = false; // Disable raycast blocking to allow drop detection
        _canvasGroup.alpha = 0.6f; // Make the item semi-transparent while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Update position to follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true; // Re-enable raycast blocking
        _canvasGroup.alpha = 1f; // Reset transparency

        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot originalSlot = _originalParent?.GetComponent<Slot>();

        if (dropSlot == null && IsMouseOverDropArea())
        {
            // Spawn 3D object and remove from UI
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _spawnDistance;
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(mousePos);

            Instantiate(itemSO.prefab, spawnPos, Quaternion.identity);

            if (originalSlot != null)
                originalSlot.currentItem = null;

            Destroy(gameObject); // Remove the dragged UI item
            return;
        }
        // Check if dropped over a valid drop slot

        if (dropSlot != null && dropSlot != originalSlot)
        {
            if (dropSlot.currentItem != null)
            {
                // If the drop slot already has an item, swap the items
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                if (originalSlot != null)
                    originalSlot.currentItem = null; // Clear the original slot if the item is dropped in a new slot
            }

            transform.SetParent(dropSlot.transform); // Move the dragged item to the new slot
            dropSlot.currentItem = gameObject; // Set the current item of the drop slot
        }

        else
        {
            // If not dropped in a valid area, return to original position
            transform.SetParent(_originalParent);

            // Restore reference to the original slot
            if (originalSlot != null && originalSlot.currentItem == null)
                originalSlot.currentItem = gameObject;
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Reset anchored position
    }

    bool IsMouseOverDropArea()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("DropArea"))
                return true;
        }

        return false;
    }
}
