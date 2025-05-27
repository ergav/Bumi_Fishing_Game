using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distanceFromTarget = 5f;

    private float sensitivity = 1000f;

    private float yaw = 0f;
    private float pitch = 0f;

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        // Zoom with scroll wheel
        distanceFromTarget -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, 2f, 15f);

        Quaternion yawRotation = Quaternion.Euler(pitch, yaw, 0f);

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

        yaw += inputDelta.x * sensitivity * Time.deltaTime;
        pitch -= inputDelta.y * sensitivity * Time.deltaTime;
    }

    void RotateCamera(Quaternion rotation)
    {
        Vector3 positionOffset = rotation * new Vector3 (0, 0, -distanceFromTarget);
        transform.position = target.position + positionOffset;
        transform.rotation = rotation;
    }
        
}
