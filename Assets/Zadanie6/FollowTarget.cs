using UnityEngine;
using UnityEngine.UIElements;

public sealed class FollowTarget : MonoBehaviour
{
    [SerializeField] float _speed;

    Transform _transform;
    Vector2 _input;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        if (_input.x == 0f && _input.y == 0f)
            return;

        _transform.position += new Vector3(_input.x * _speed, 0f, _input.y * _speed) * Time.deltaTime;
    }
}
