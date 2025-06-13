// REPLACE your entire Draggable.cs with this simple version:

using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector3 mousePosition;
    private bool isDragged = false;
    public ItemSO itemSO;
    public InventoryManager inventoryManager;

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
}

