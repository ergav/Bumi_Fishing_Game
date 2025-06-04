using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector3 mousePosition;

    public delegate void DragEndedDelegate(Draggable draggableObject);

    public DragEndedDelegate dragEndedCallback;

    private bool isDragged = false;

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
        dragEndedCallback(this);
    }
}

