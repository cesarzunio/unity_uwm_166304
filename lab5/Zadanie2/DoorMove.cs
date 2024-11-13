using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DoorMove : MonoBehaviour
{
    [SerializeField] Transform _transformPlayer;
    [SerializeField] float _distanceMin;
    [SerializeField] Vector3 _positionB;
    [SerializeField] float _speed;

    Rigidbody _rigidbody;

    Vector3 _positionA;
    float _moveRatio;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _positionA = transform.position;
    }

    private void FixedUpdate()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        float distanceSq = Vector3.SqrMagnitude(_transformPlayer.position - _positionA);
        float moveDirection = distanceSq < _distanceMin * _distanceMin ? 1f : -1f;

        _moveRatio += moveDirection * _speed * Time.deltaTime;
        _moveRatio = Mathf.Clamp(_moveRatio, 0f, 1f);

        _rigidbody.position = Vector3.Lerp(_positionA, _positionB, _moveRatio);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(_positionB, 0.5f);
    }
}
