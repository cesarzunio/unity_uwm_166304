using UnityEngine;

public sealed class CubePlayerMove : MonoBehaviour
{
    const float GIZMOS_Y = 1f;

    [SerializeField] float _speed;
    [SerializeField] float _speedAngularLerpT;
    [SerializeField] Vector2 _boundsMin;
    [SerializeField] Vector2 _boundsMax;
    [SerializeField] float _cubeSize;

    Transform _transform;

    Vector2 _input;
    Vector2 _lookDir;

    private void Awake()
    {
        _transform = transform;

        var forward = _transform.forward;
        _lookDir = new Vector2(forward.x, forward.z);
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (_input.x != 0f || _input.y != 0f)
        {
            _lookDir = _input;
        }
    }

    private void FixedUpdate()
    {
        float dt = Time.deltaTime; 

        _transform.GetPositionAndRotation(out var position, out var rotation);

        Move(ref position, dt);
        Rotate(ref rotation, dt);

        _transform.SetPositionAndRotation(position, rotation);
    }

    void Rotate(ref Quaternion rotation, float dt)
    {
        var target = Quaternion.LookRotation(new Vector3(_lookDir.x, 0f, _lookDir.y));
        rotation = Quaternion.Lerp(_transform.rotation, target, _speedAngularLerpT * dt);
    }

    void Move(ref Vector3 position, float dt)
    {
        if (_input.x == 0f && _input.y == 0f)
            return;

        position = _transform.position + new Vector3(_input.x * _speed, 0f, _input.y * _speed) * dt;

        float cubeSizeHalf = _cubeSize / 2f;

        position.x = Mathf.Max(position.x, _boundsMin.x + cubeSizeHalf);
        position.z = Mathf.Max(position.z, _boundsMin.y + cubeSizeHalf);

        position.x = Mathf.Min(position.x, _boundsMax.x - cubeSizeHalf);
        position.z = Mathf.Min(position.z, _boundsMax.y - cubeSizeHalf);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        var leftBot = new Vector3(_boundsMin.x, GIZMOS_Y, _boundsMin.y);
        var rightBot = new Vector3(_boundsMax.x, GIZMOS_Y, _boundsMin.y);
        var rightTop = new Vector3(_boundsMax.x, GIZMOS_Y, _boundsMax.y);
        var leftTop = new Vector3(_boundsMin.x, GIZMOS_Y, _boundsMax.y);

        Gizmos.DrawLine(leftBot, rightBot);
        Gizmos.DrawLine(rightBot, rightTop);
        Gizmos.DrawLine(rightTop, leftTop);
        Gizmos.DrawLine(leftTop, leftBot);
    }
}
