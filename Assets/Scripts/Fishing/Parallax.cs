using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float   _length;
    private float   _startPos;
    private float   _startPosY;
    [SerializeField] private Camera  _cam;
    public float    _parallaxEffect;

    [SerializeField] private bool _dontLoop;
    void Start()
    {
        _startPos = transform.position.x;
        _startPosY = transform.position.y;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (_cam.transform.localPosition.x * (1 - _parallaxEffect));
        float dist = (_cam.transform.localPosition.x * _parallaxEffect);

        float disty = (_cam.transform.localPosition.y * _parallaxEffect);

        transform.localPosition = new Vector3(_startPos + dist, _startPosY + disty, transform.localPosition.z);

        if (!_dontLoop)
        {
            if (temp > _startPos + _length)
            {
                _startPos += _length;
            }
            else if (temp < _startPos - _length)
            {
                _startPos -= _length;
            }
        }

    }
}
