using UnityEngine;

public class Fishable : MonoBehaviour
{
    [Range(1.0f, 5.0f)]
    public float Weight;
    [HideInInspector] public bool IsGrabbed;

    [SerializeField] private int Value = 1;

    void Start()
    {
        if (Weight < 1)
            Weight = 1;
    }

    public void Collect()
    {
        PointManager.Instance.Addpoints(Value);
        Destroy(gameObject);
    }
}