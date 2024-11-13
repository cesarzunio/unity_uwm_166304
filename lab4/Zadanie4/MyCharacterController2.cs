using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public sealed class MyCharacterController2 : MonoBehaviour
{
    [SerializeField] Transform _transformCamera;
    [SerializeField] float _speed;
    [SerializeField] float _speedCamera;
    [SerializeField] float _jumpVelocity;
    [SerializeField] float _gravity;
    [SerializeField] float _groundedHeight;

    Transform _transform;
    CharacterController _controller;
    float _velocityY;

    Vector2 _movement;
    bool _jump;

    Vector3 _mousePositionOld;
    Vector2 _rotationAngles;

    private void Awake()
    {
        _transform = transform;
        _controller = GetComponent<CharacterController>();

        GetMouseDelta();
    }

    private void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _jump = Input.GetKey(KeyCode.Space);

        _rotationAngles += GetMouseDelta().normalized * _speedCamera;
        _rotationAngles.y = Mathf.Clamp(_rotationAngles.y, -90f, 90f);

        _transform.rotation = Quaternion.Euler(0f, -_rotationAngles.x, 0f);
        _transformCamera.localRotation = Quaternion.Euler(_rotationAngles.y, 0f, 0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 GetMouseDelta()
    {
        var mousePosition = Input.mousePosition;
        var delta = _mousePositionOld - mousePosition;
        _mousePositionOld = mousePosition;

        return (Vector2)delta;
    }

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        bool isGrounded = _controller.isGrounded && _velocityY <= 0f;

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

        var movementDt = _speed * dt * _movement;
        var right = _transform.right;
        var forward = _transform.forward;

        var translation = (right * movementDt.x) + (forward * movementDt.y);
        translation.y = _velocityY * dt;

        _controller.Move(translation);
    }
}
