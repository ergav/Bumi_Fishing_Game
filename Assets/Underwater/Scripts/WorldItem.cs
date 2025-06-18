using UnityEngine;


public class WorldItem : MonoBehaviour
{
    Vector3 mousePosition;
    private bool isDragged = false;

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
    }
}

