using UnityEngine;

[ExecuteInEditMode]
public class RenderLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform[] points;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        SetUpLine(points);
    }

    public void SetUpLine(Transform[] points)
    {
        lineRenderer.positionCount = points.Length;
        this.points = points;
    }

     void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }
}