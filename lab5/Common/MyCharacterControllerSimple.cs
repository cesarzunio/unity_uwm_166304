using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MyCharacterControllerSimple : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _jumpVelocity;
    [SerializeField] float _gravity;
    [SerializeField] float _drag;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] bool _isGrounded;

    Transform _transform;
    Rigidbody _rigidbody;
    CapsuleCollider _collider;

    Vector2 _movement;
    bool _jump;

    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _jump = Input.GetKey(KeyCode.Space);
    }

    Vector3 RayStart => _rigidbody.position - new Vector3(0f, (_collider.height / 2) - 0.01f, 0f);

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        var velocity = _rigidbody.velocity;

        bool isGroundBelow = Physics.Raycast(RayStart, Vector3.down, 0.3f, _groundMask);
        _isGrounded = isGroundBelow && velocity.y <= 0.001f;

        if (_isGrounded && _jump)
        {
            velocity.y += _jumpVelocity;
            _isGrounded = false;
        }

        if (_isGrounded)
        {
            _rigidbody.useGravity = false;
            _rigidbody.drag = _drag;
        }
        else
        {
            _rigidbody.useGravity = true;
            _rigidbody.drag = 0f;
        }

        var right = _transform.right;
        var forward = _transform.forward;

        var acceleration = (right * _movement.x) + (forward * _movement.y);
        acceleration.y = 0f;

        if (!_isGrounded)
        {
            acceleration *= 0.1f;
        }

        velocity += _speed * dt * acceleration;

        _rigidbody.velocity = velocity;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RayStart, 0.05f);
    }
}
