using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleLauncher : MonoBehaviour
{
    [SerializeField] private GameObject         _bubblePrefab;
    [SerializeField] private Transform          _muzzle;
    [SerializeField] private float              _bubbleSpeed = 10;
    [SerializeField] private float              _bubbleLifetime = 10;
    [SerializeField] private SpriteRenderer     _crosshair;
    [SerializeField] private GameObject         _bubbleGunUI;
    [SerializeField] private bool               _willCameraFollowBubble;
    [SerializeField] private int                _maxBounces = 3;

    private Bubble                              _firedBubble;
    private CameraFollow                        _camera;
    private bool                                _isAimModeOn;

    private void Start()
    {
        _camera = Camera.main.gameObject.GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);

            if (IsMouseOverUI())
                return;

            SetCrosshair();
            ToggleUIOn();
        }

        if (_firedBubble != null)
        {
            if (_firedBubble.IsHoldingItem)
            {
                _firedBubble = null;
            }
        }

        if (_isAimModeOn)
        {
            float angle = Mathf.Atan2(_crosshair.transform.position.y - transform.position.y, _crosshair.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.rotation = targetRotation;
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    ShootBubble();
        //}
    }

    private void SetCrosshair()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _crosshair.transform.position = mousePos;
    }

    public void ShootBubble()
    {
        if (_firedBubble != null)
        { 
            return;
        }

        Vector2 forward = transform.TransformDirection(Vector2.left);
        Vector2 toOther = Vector3.Normalize(_crosshair.transform.position - transform.position);

        if (Vector3.Dot(forward, Vector2.up) < 0.1)
        {
            return;
        }

        _firedBubble = Instantiate(_bubblePrefab.GetComponent<Bubble>(), _muzzle.position, Quaternion.identity);
        _firedBubble.OnInstantiate(toOther, _bubbleSpeed, _bubbleLifetime, _maxBounces);

        ToggleUIOff();

        if (_willCameraFollowBubble)
        {
            _camera.SetBubbleTarget(_firedBubble.transform);
        }
    }

    public void ToggleUIOn()
    {
        _bubbleGunUI.SetActive(true);
        _crosshair.enabled = true;
        _isAimModeOn = true;
    }
    public void ToggleUIOff()
    {
        _bubbleGunUI.SetActive(false);
        _crosshair.enabled = false;
        _isAimModeOn = false;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}