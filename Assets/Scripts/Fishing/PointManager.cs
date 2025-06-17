using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private int                         _points;

    [SerializeField] private Text       _pointText;

    public static PointManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

    }

    public void Addpoints(int points)
    {
        _points += points;
        _pointText.text = _points.ToString();
    }
}