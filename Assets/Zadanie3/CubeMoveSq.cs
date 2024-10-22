using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CubeMoveSq : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _speedAngular;
    [SerializeField] float _distance;

    Transform _transform;
    int _index;

    (CubeMoveSqMode Mode, Vector3 Vector)[] _modeDatas;

    private void Awake()
    {
        _transform = transform;
        _index = 0;

        _modeDatas = new (CubeMoveSqMode, Vector3)[]
        {
            (CubeMoveSqMode.Rotating, Vector3.right),
            (CubeMoveSqMode.Moving, Vector3.right),
            (CubeMoveSqMode.Rotating, Vector3.forward),
            (CubeMoveSqMode.Moving, Vector3.right + Vector3.forward),
            (CubeMoveSqMode.Rotating, Vector3.left),
            (CubeMoveSqMode.Moving, Vector3.forward),
            (CubeMoveSqMode.Rotating, Vector3.back),
            (CubeMoveSqMode.Moving, Vector3.zero),
        };
    }

    private void FixedUpdate()
    {
        switch (_modeDatas[_index].Mode)
        {
            case CubeMoveSqMode.Rotating:
                Rotate(); return;

            case CubeMoveSqMode.Moving:
                Move(); return;

            default:
                throw new Exception($"CubeMoveSq :: FixedUpdate :: Cannot match CubeMoveSqMode ({_modeDatas[_index].Mode})!");
        }
    }

    void Rotate()
    {
        var currentQ = _transform.rotation;
        var targetQ = Quaternion.LookRotation(_modeDatas[_index].Vector, Vector3.up);

        float angleToTarget = Quaternion.Angle(currentQ, targetQ); // unsigned angle
        float angleChangeVal = _speedAngular * Time.deltaTime;

        if (angleToTarget < angleChangeVal)
        {
            _transform.rotation = targetQ;
            _index = (_index + 1) % _modeDatas.Length;
            return;
        }

        _transform.rotation = Quaternion.RotateTowards(currentQ, targetQ, angleChangeVal);
    }

    void Move()
    {
        var current = _transform.position;
        var target = _modeDatas[_index].Vector * _distance / 2f;

        float distanceToTarget = Vector3.Distance(current, target);
        float distanceChangeVal = _speed * Time.deltaTime;

        if (distanceToTarget < distanceChangeVal)
        {
            _transform.position = target;
            _index = (_index + 1) % _modeDatas.Length;
            return;
        }

        _transform.position = Vector3.MoveTowards(current, target, distanceChangeVal);
    }
}

public enum CubeMoveSqMode
{
    Moving,
    Rotating,
}
