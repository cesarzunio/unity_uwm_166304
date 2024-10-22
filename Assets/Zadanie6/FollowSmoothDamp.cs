using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FollowSmoothDamp : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _smoothTime;

    Transform _transform;
    Vector3 _velocity;

    private void Awake()
    {
        _transform = transform;
        _velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _transform.position = Vector3.SmoothDamp(_transform.position, _target.position, ref _velocity, _smoothTime, float.PositiveInfinity, Time.deltaTime);
    }
}
