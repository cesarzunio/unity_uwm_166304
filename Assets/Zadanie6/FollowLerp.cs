using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FollowLerp : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _lerpT;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        _transform.position = Vector3.Lerp(_transform.position, _target.position, _lerpT * Time.deltaTime);
    }
}
