using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlatformMove1 : MonoBehaviour
{
    [SerializeField] Vector3 _positionA;
    [SerializeField] Vector3 _positionB;
    [SerializeField] float _speed;

    Rigidbody _rigidbody;

    bool _isMoving;
    float _moveRatio;
    float _moveDirection;

    MyCharacterControllerSimple _player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.position = _positionA;
    }

    private void FixedUpdate()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        if (!_isMoving)
            return;

        _moveRatio += _moveDirection * _speed * Time.deltaTime;

        if (_moveDirection == 1f && _moveRatio > 1f)
        {
            _moveRatio = 2f - _moveRatio;
            _moveDirection = -1f;
        }
        else if (_moveDirection == -1f && _moveRatio < 0f)
        {
            _moveRatio = 0f;
            _moveDirection = 1f;
            
            if (_player == null)
            {
                _isMoving = false;
            }
        }
        
        var positionNew = Vector3.Lerp(_positionA, _positionB, _moveRatio);
        
        if (_player != null)
        {
            var position = _rigidbody.position;
            var positionChange = positionNew - position;

            _player.Rigidbody.MovePosition(_player.Rigidbody.position + positionChange);
        }

        _rigidbody.position = positionNew;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enter?");

        if (collision.gameObject.name == "Player")
        {
            _player = collision.gameObject.GetComponent<MyCharacterControllerSimple>();

            if (!_isMoving)
            {
                _isMoving = true;
                _moveRatio = 0f;
                _moveDirection = 1f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit?");

        if (collision.gameObject.name == "Player")
        {
            _player = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(_positionA, 0.5f);
        Gizmos.DrawSphere(_positionB, 0.5f);
    }
}
