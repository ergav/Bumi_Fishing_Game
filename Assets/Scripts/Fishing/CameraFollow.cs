using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform  _player;
    [SerializeField] private float      _smoothSpeed;
    [SerializeField] private Vector3    _offset;
        
    private float                       _offsetX;

    [SerializeField] private float      _maximumHeight = 0;
    [SerializeField] private float      _minimumHeight = 0;

    private Transform                   _target;

    private void Start()
    {
        _target = _player;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            CameraMovement();
        }
        else
        {
            ResetTarget();
        }
    }

    private void CameraMovement()
    {
        Vector3 desiredPos = _target.position + _offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, _smoothSpeed);
        transform.position = smoothedPos;

        if (transform.position.y < _minimumHeight)
        {
            transform.position = new Vector3(transform.position.x, _minimumHeight, transform.position.z);
        }

        if (transform.position.y > _maximumHeight)
        {
            transform.position = new Vector3(transform.position.x, _maximumHeight, transform.position.z);
        }
    }

    public void SetBubbleTarget(Transform target)
    { 
        _target = target;
    }

    public void ResetTarget()
    {
        _target = _player;
    }
}