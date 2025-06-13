using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework.Internal.Execution;

public class InventoryManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject[] slots = new GameObject[4];
    [SerializeField] private GameObject inventoryParent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Camera cam;


    private GameObject draggedItem;
    private GameObject lastItemSlot;

    public bool isInventoryOpened;

    private void Update()
    {
        inventoryParent.SetActive(isInventoryOpened);

        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }
    public void ToggleInventory()
    {
        isInventoryOpened = !isInventoryOpened;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log(eventData.pointerCurrentRaycast.gameObject);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            InventorySlot slot = clickedObject.GetComponentInParent<InventorySlot>();

            if (slot != null && slot.heldItem != null)
            {
                draggedItem = slot.heldItem;
                slot.heldItem = null;
                lastItemSlot = clickedObject;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (draggedItem != null && eventData.pointerCurrentRaycast.gameObject != null && eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            InventorySlot slot = clickedObject.GetComponentInParent<InventorySlot>();

            // If there is no item in the slot, place item
            if (slot != null && slot.heldItem == null)
            {
                slot.SetHeldItem(draggedItem);
            }
            // If there is an item in the slot, swap items
            else if (slot != null && slot.heldItem != null)
            {
                lastItemSlot.GetComponentInParent<InventorySlot>().SetHeldItem(slot.heldItem);
                slot.SetHeldItem(draggedItem);
            }
            // Return item to last slot
            else if (clickedObject.name != "DropItem")
            {
                lastItemSlot.GetComponent<InventorySlot>().SetHeldItem(draggedItem);
            }
            // Drop item
            else
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                Vector3 position = ray.GetPoint(3);

                GameObject newItem = Instantiate(draggedItem.GetComponent<InventoryItem>().itemSO.prefab, position, new Quaternion());

                lastItemSlot.GetComponent<InventorySlot>().heldItem = null;
                Destroy(draggedItem);
            }

            draggedItem = null;
        }
    }

    public void ItemPicked(GameObject pickedItem)
    {
        GameObject emptySlot = null;

        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i].GetComponent<InventorySlot>();

            if (slot.heldItem == null)
            {
                emptySlot = slots[i];
                break;
            }
        }

        if (emptySlot != null)
        {
            GameObject newItem = Instantiate(itemPrefab);
            newItem.GetComponent<InventoryItem>().itemSO = pickedItem.GetComponent<Draggable>().itemSO;
            newItem.transform.SetParent(emptySlot.transform.parent.parent.GetChild(2));

            emptySlot.GetComponent<InventorySlot>().SetHeldItem(newItem);

            Destroy(pickedItem);
        }
    }
}