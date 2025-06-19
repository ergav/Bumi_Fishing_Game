using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    private Vector2                     _direction;
    private float                       _speed = 10;
    private float                       _lifetime = 10;
    private SpriteRenderer              _spriteRenderer;
    [SerializeField] private int        _maxBounces = 3;

    private int                          _bounces;
    private bool                        _isHoldingItem;

    private Fishable                    _fishedObject;
    public bool                         IsHoldingItem => _isHoldingItem;

    private Rigidbody2D                 _rigidbody;
    private Collider2D                  _collider;

    private CameraFollow                _camera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponentInChildren<Collider2D>();
        _camera = Camera.main.gameObject.GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (_isHoldingItem)
        {
            _collider.isTrigger = true;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            transform.Translate(Vector2.up * (5 * Time.deltaTime), Space.World);

            if (transform.position.y > 3)
            {
                OnReachSurface();
            }

            return;
        }

        if (transform.position.y > 3 && _bounces > 0)
        {
            Destroy(gameObject);
        }

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0)
            Destroy(gameObject);
    }

    public void OnInstantiate(Vector2 dir, float speed = 10, float lifetime = 10, int maxBounces = 3)
    {
        _direction = dir;
        _speed = speed;
        _lifetime = lifetime;
        _maxBounces = maxBounces;
        _rigidbody.AddForce(_direction * speed, ForceMode2D.Impulse);

        //StartCoroutine(LifeSpan());
    }

    //IEnumerator LifeSpan()
    //{
    //    yield return new WaitForSeconds(_lifetime);
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isHoldingItem)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Fishable"))
        {
            Fishable fished = collision.GetComponent<Fishable>();

            if (fished.IsGrabbed)
                return;

            _fishedObject = fished;
            collision.transform.position = transform.position;
            collision.transform.SetParent(transform);
            _isHoldingItem = true;

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            float newSize = (sr.sprite.bounds.size.x * sr.transform.localScale.x) + 2;

            _spriteRenderer.transform.localScale = new Vector2(newSize, newSize);

            _fishedObject.OnGrabbed();

            _camera.ResetTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //_rigidbody.gravityScale = 0.5f;

        //_camera.ResetTarget();

        _bounces++;
        if(_bounces > _maxBounces)
        {
            Destroy(gameObject);
        }

        if (_isHoldingItem)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Fishable"))
        {
            Fishable fished = collision.gameObject.GetComponent<Fishable>();

            if (fished.IsGrabbed)
                return;

            _fishedObject = fished;
            collision.transform.position = transform.position;
            collision.transform.SetParent(transform);
            _isHoldingItem = true;

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            float newSize = (sr.sprite.bounds.size.x * sr.transform.localScale.x) + 2;

            _spriteRenderer.transform.localScale = new Vector2(newSize, newSize);

            _fishedObject.OnGrabbed();

            _camera.ResetTarget();
        }
    }

    public void OnReachSurface()
    {
        _fishedObject.Collect();
        Destroy(gameObject);
    }
}