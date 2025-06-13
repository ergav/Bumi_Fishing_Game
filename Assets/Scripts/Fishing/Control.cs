using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private Transform      _rod;
    [SerializeField] private Rigidbody2D    _hook;
    [SerializeField] private Transform      _hookItemPoint;
    [SerializeField] private float          _hookSpeed = 5;
    [SerializeField] private float          _boatSpeed = 2.5f;

    [SerializeField] private float          _maximumHookHeight = 0;
    [SerializeField] private float          _minimumHookHeight = -10;

    [SerializeField] private LayerMask      _fishableLayerMask;

    private Rigidbody2D                     _rb;

    [SerializeField] private Fishable       _fishedObject;

    private float                           _currentHookSpeed;

    private SpriteRenderer                  _hookSprite;

    [SerializeField] private Sprite         _hookReachSprite;
    [SerializeField] private Sprite         _hookGrabSprite;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hookSprite = _hook.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (_fishedObject == null)
        {
            Fishing();
            _currentHookSpeed = _hookSpeed;
            _hookSprite.sprite = _hookReachSprite;
        }
        else
        { 
            _currentHookSpeed = _hookSpeed / _fishedObject.Weight;
            _hookSprite.sprite = _hookGrabSprite;
        }
    }

    void FixedUpdate()
    {
        float boatHorizontalMove = Input.GetAxisRaw("Horizontal");
        float hookVerticalMove = Input.GetAxisRaw("Vertical");

        Vector2 boatMove = Vector2.right * boatHorizontalMove * (_boatSpeed * Time.fixedDeltaTime);
        Vector2 hookMove = new Vector2(boatMove.x ,hookVerticalMove * (_currentHookSpeed * Time.fixedDeltaTime));

        _rb.linearVelocity = boatMove;
        _hook.linearVelocity = hookMove;

        Vector2 desiredHookPos = new Vector2(_rod.position.x, _hook.transform.position.y);
        Vector2 smoothHookPos = Vector2.Lerp(_hook.transform.position, desiredHookPos, 0.1f);
        _hook.transform.position = smoothHookPos;

        if (_hook.transform.localPosition.y > _maximumHookHeight)
        {
            _hook.transform.localPosition = new Vector3(_hook.transform.localPosition.x, _maximumHookHeight, _hook.transform.localPosition.z);

            if (_fishedObject != null)
            {
                Destroy(_fishedObject.gameObject);
                _fishedObject = null;
            }
        }
    }

    private void Fishing()
    {
        Collider2D col = Physics2D.OverlapCircle(_hookItemPoint.position, 0.5f, _fishableLayerMask);

        if (col != null)
        {
            col.transform.position = _hookItemPoint.transform.position;
            col.transform.SetParent(_hookItemPoint);
            _fishedObject = col.gameObject.GetComponent<Fishable>();
            _fishedObject.IsGrabbed = true;
        }
    }
}