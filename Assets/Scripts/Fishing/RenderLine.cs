using UnityEngine;

[ExecuteInEditMode]
public class RenderLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private Transform[] _points;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        SetUpLine(_points);
    }

    public void SetUpLine(Transform[] points)
    {
        _lineRenderer.positionCount = points.Length;
        this._points = points;
    }

     void Update()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            _lineRenderer.SetPosition(i, _points[i].position);
        }
    }
}