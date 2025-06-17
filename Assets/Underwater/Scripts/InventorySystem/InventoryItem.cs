using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSO itemSO;
    public Image itemIcon;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        originalParent = transform.parent;
        transform.SetParent(transform.root); // Move to root to avoid being blocked by other UI elements
        canvasGroup.blocksRaycasts = false; // Disable raycast blocking to allow drop detection
        canvasGroup.alpha = 0.6f; // Make the item semi-transparent while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Update position to follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Re-enable raycast blocking
        canvasGroup.alpha = 1f; // Reset transparency

        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot originalSlot = originalParent?.GetComponent<Slot>();

        if (dropSlot != null)
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
                originalSlot.currentItem = null; // Clear the original slot if the item is dropped in a new slot
            }

            transform.SetParent(dropSlot.transform); // Move the dragged item to the new slot
            dropSlot.currentItem = gameObject; // Set the current item of the drop slot
        }
        else
        {
            // If not dropped in a valid area, return to original position
            transform.SetParent(originalParent);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Reset anchored position
    }
}
