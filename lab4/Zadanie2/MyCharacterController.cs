using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MyCharacterController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _jumpVelocity;
    [SerializeField] float _gravity;
    [SerializeField] float _groundedHeight;
    [SerializeField] bool _isGrounded;

    Transform _transform;
    CharacterController _controller;
    float _velocityY;

    Vector2 _movement;
    bool _jump;

    private void Awake()
    {
        _transform = transform;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _jump = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        bool isGrounded = _controller.isGrounded && _velocityY <= 0f;

        //_isGrounded = isGrounded;

        if (isGrounded)
        {
            var position = _transform.position;
            position.y = _groundedHeight;

            _velocityY = 0f;
            _transform.position = position;

            if (_jump)
            {
                _velocityY += _jumpVelocity;
            }
        }
        else
        {
            _velocityY += _gravity * dt;
        }

        var translation = new Vector3(_movement.x * _speed, _velocityY, _movement.y * _speed) * dt;

        _controller.Move(translation);
    }
}
