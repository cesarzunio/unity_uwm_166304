using UnityEngine;

public sealed class CubeMove : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _distance;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        float position = Mathf.PingPong(_speed * Time.time, _distance);

        _transform.position = new Vector3(position, 0f, 0f);
    }
}
