using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceFromTarget = 5f;
    [SerializeField] private Camera _cam;
    [SerializeField] private InventoryManager _inventoryManager;

    private float _sensitivity = 1000f;
    private float _yaw = 0f;
    private float _pitch = 0f;

    void Update()
    {
        HandleInput();

        // Zoom with scroll wheel
        _distanceFromTarget -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        _distanceFromTarget = Mathf.Clamp(_distanceFromTarget, 2f, 10f);

        Quaternion yawRotation = Quaternion.Euler(_pitch, _yaw, 0f);

        RotateCamera(yawRotation);
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

        _yaw += inputDelta.x * _sensitivity * Time.deltaTime;
        _pitch -= inputDelta.y * _sensitivity * Time.deltaTime;

        _pitch = Mathf.Clamp(_pitch, 0f, 45f);
    }

    void RotateCamera(Quaternion rotation)
    {
        Vector3 positionOffset = rotation * new Vector3(0, 0, -_distanceFromTarget);
        transform.position = _target.position + positionOffset;
        transform.rotation = rotation;
    }
}
