using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class CubeInstantiator : MonoBehaviour
{
    const int TRIES_MAX = 100;
    const float GIZMOS_Y = 1f;

    [SerializeField] GameObject _prefab;
    [SerializeField] int _cubesToSpawn;
    [SerializeField] uint _seed;
    [SerializeField] Vector2 _boundsMin;
    [SerializeField] Vector2 _boundsMax;
    [SerializeField] float _cubeSize;

    Collider[] _overlaps;
    List<GameObject> _cubes;

    private void Awake()
    {
        _overlaps = new Collider[1];
        _cubes = new List<GameObject>(_cubesToSpawn);
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (!Application.isPlaying)
            throw new Exception("CubeInstantiator :: Spawn :: Spawn only in play-mode!");

        Clear();

        var random = new Unity.Mathematics.Random(_seed);
        var transform = this.transform;

        for (int i = 0; i < _cubesToSpawn; i++)
        {
            Physics.SyncTransforms();

            var position = GetNewPosition(ref random);
            var cube = GameObject.Instantiate(_prefab, position, Quaternion.identity);

            cube.name = $"Cube_{i}";
            cube.transform.parent = transform;

            _cubes.Add(cube);
        }
    }

    void Clear()
    {
        foreach (var cube in _cubes)
        {
            GameObject.Destroy(cube);
        }

        _cubes.Clear();

        Physics.SyncTransforms();
    }

    Vector3 GetNewPosition(ref Unity.Mathematics.Random random)
    {
        int tries = 0;

        while (tries++ < TRIES_MAX)
        {
            float cubeSizeHalf = _cubeSize / 2;

            float x = random.NextFloat(_boundsMin.x + cubeSizeHalf, _boundsMax.x - cubeSizeHalf);
            float z = random.NextFloat(_boundsMin.y + cubeSizeHalf, _boundsMax.y - cubeSizeHalf);

            var position = new Vector3(x, 0f, z);
            var halfExtends = new Vector3(cubeSizeHalf, cubeSizeHalf, cubeSizeHalf);

            int hitsCount = Physics.OverlapBoxNonAlloc(position, halfExtends, _overlaps);

            if (hitsCount == 0)
                return position;
        }

        throw new Exception("CubeInstantiator :: GetNewPosition :: Tries limit reached!");
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

[CustomEditor(typeof(CubeInstantiator))]
public sealed class CubeInstantiatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var ci = (CubeInstantiator)target;

        if (GUILayout.Button("Spawn"))
        {
            ci.Spawn();
        }
    }
}
