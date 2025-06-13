using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    private Vector2                     _direction;
    private float                       _speed = 10;
    private float                       _lifetime = 10;
    private SpriteRenderer              _spriteRenderer;

    private bool                        _isHoldingItem;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (!_isHoldingItem)
        {
            transform.Translate(_direction * (_speed * Time.deltaTime));
        }
        else
        {
            transform.Translate(Vector2.up * (5 * Time.deltaTime));

            if (transform.position.y > 3)
            {
                OnReachSurface();
            }
        }
    }

    public void OnInstantiate(Vector2 _dir, float _speed = 10, float _lifetime = 10)
    {
        _direction = _dir;
        this._speed = _speed;
        this._lifetime = _lifetime;

        StartCoroutine(LifeSpan());
    }

    IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fishable"))
        {
            Fishable fished = collision.GetComponent<Fishable>();

            if (fished.IsGrabbed)
                return;

            collision.transform.position = transform.position;
            collision.transform.SetParent(transform);
            _isHoldingItem = true;

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            float newSize = (sr.sprite.bounds.size.x * sr.transform.localScale.x) + 2;

            _spriteRenderer.transform.localScale = new Vector2(newSize, newSize);
            Debug.Log("Grab");

            fished.IsGrabbed = true;
        }
    }

    public void OnReachSurface()
    {
        Destroy(gameObject);
    }
}