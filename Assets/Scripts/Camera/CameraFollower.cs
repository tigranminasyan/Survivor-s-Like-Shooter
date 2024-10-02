using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0f, -10f);
    private Vector3 _moveToPosition;
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (_target && !Application.isPlaying)
        {
            UpdatePositionImmediately();
        }
#endif
    }

    private void Awake()
    {
        CalculateMoveToPosition();
    }

    private void UpdatePositionImmediately()
    {
        CalculateMoveToPosition();
        transform.position = _moveToPosition;
    }

    private void Update()
    {
        CalculateMoveToPosition();

        transform.position = _moveToPosition;
    }

    private void CalculateMoveToPosition()
    {
        Vector3 targetPosition = _target.position;
        _moveToPosition =  targetPosition + _offset;
    }
}
