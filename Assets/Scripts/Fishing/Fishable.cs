using UnityEngine;

public class Fishable : MonoBehaviour
{
    [Range(1.0f, 5.0f)]
    public float Weight;

    private bool _isGrabbed;

    protected SpriteRenderer _spriteRenderer;

    [HideInInspector] public bool IsGrabbed => _isGrabbed;

    [SerializeField] private int Value = 1;

    public virtual void Start()
    {
        if (Weight < 1)
            Weight = 1;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Collect()
    {
        PointManager.Instance.Addpoints(Value);
        Destroy(gameObject);
    }

    public virtual void OnGrabbed()
    {
        _isGrabbed = true;

        //Make sprite sorting layer above world geometry but below bubble sprite.
        _spriteRenderer.sortingOrder = 9;
    }
}