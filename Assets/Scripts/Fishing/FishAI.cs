using UnityEngine;

public class FishAI : Fishable
{
    [SerializeField] private float  _swimSpeed = 5;

    private Rigidbody2D             _rigidbody;
    private Collider2D              _collider;

    private Control                 _player;

    public override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _player = FindFirstObjectByType<Control>();
    }

    void FixedUpdate()
    {
        if (!IsGrabbed)
        {
            _rigidbody.linearVelocity = new Vector2(_swimSpeed * Time.fixedDeltaTime, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _swimSpeed = -_swimSpeed;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    public override void OnGrabbed()
    {
        base.OnGrabbed();
        _collider.isTrigger = true;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.linearVelocity = Vector2.zero;
        _spriteRenderer.flipY = true;
    }
}