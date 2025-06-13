using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget = 5f;
    [SerializeField] private Camera cam;
    [SerializeField] private InventoryManager inventoryManager;

    private float sensitivity = 1000f;
    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        HandleInput();

        // Zoom with scroll wheel
        distanceFromTarget -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, 2f, 15f);

        Quaternion yawRotation = Quaternion.Euler(pitch, yaw, 0f);

        RotateCamera(yawRotation);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3))
            {
                Draggable item = hit.collider.GetComponent<Draggable>();

                if (item != null)
                {
                    inventoryManager.ItemPicked(hit.collider.gameObject);
                }
            }
        }
    }

    public void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputDelta = touch.deltaPosition;
        }

        else if (Input.GetMouseButton(1))
        {
            inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        yaw += inputDelta.x * sensitivity * Time.deltaTime;
        pitch -= inputDelta.y * sensitivity * Time.deltaTime;
    }

    void RotateCamera(Quaternion rotation)
    {
        Vector3 positionOffset = rotation * new Vector3(0, 0, -distanceFromTarget);
        transform.position = target.position + positionOffset;
        transform.rotation = rotation;
    }
}
