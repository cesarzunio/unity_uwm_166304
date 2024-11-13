using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class PlatformMovePath : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] Vector3[] _points;

    Transform _transform;

    int _pointIndexCurrent;
    float _distanceTravelled;
    float _distanceToNext;
    int _direction;

    private void Awake()
    {
        _transform = transform;
        _transform.position = _points[0];

        _pointIndexCurrent = 0;
        _distanceTravelled = 0f;
        _distanceToNext = Vector3.Distance(_points[0], _points[1]);
        _direction = 1;
    }

    private void Update()
    {
        UpdateDistanceCurrent();

        _transform.position = Vector3.Lerp(_points[_pointIndexCurrent], _points[_pointIndexCurrent + _direction], _distanceTravelled / _distanceToNext);
    }

    void UpdateDistanceCurrent()
    {
        _distanceTravelled += _speed * Time.deltaTime;

        if (_distanceTravelled < _distanceToNext)
            return;

        _distanceTravelled -= _distanceToNext;
        
        if (_direction == 1 && _pointIndexCurrent == _points.Length - 2)
        {
            _direction = -1;
            _pointIndexCurrent = _points.Length - 1;
        }
        else if (_direction == -1 && _pointIndexCurrent == 1)
        {
            _direction = 1;
            _pointIndexCurrent = 0;
        }
        else
        {
            _pointIndexCurrent += _direction;
        }

        _distanceToNext = Vector3.Distance(_points[_pointIndexCurrent], _points[_pointIndexCurrent + _direction]);
    }

    private void OnDrawGizmos()
    {
        if (_points == null)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _points.Length - 1; i++)
        {
            Gizmos.DrawLine(_points[i], _points[i + 1]);
            Gizmos.DrawSphere(_points[i], 0.2f);
        }

        Gizmos.DrawSphere(_points[^1], 0.2f);
    }
}
