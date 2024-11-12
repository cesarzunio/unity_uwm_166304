using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public sealed class RandomCubesGenerator : MonoBehaviour
{
    [SerializeField] MeshRenderer _prefab;
    [SerializeField] int _prefabsCountMax;
    [SerializeField] float _delay;
    [SerializeField] float2 _positionMin;
    [SerializeField] float2 _positionMax;
    [SerializeField] uint _seed;
    [SerializeField] Material[] _materials;

    int _prefabsCount;
    Vector3[] _positions;
    Unity.Mathematics.Random _random;

    private void Start()
    {
        _positions = new Vector3[_prefabsCountMax];
        _random = new Unity.Mathematics.Random(_seed);

        for (int i = 0; i < _prefabsCountMax; i++)
        {
            var positionXZ = _random.NextFloat2(_positionMin, _positionMax);
            _positions[i] = new Vector3(positionXZ.x, 0f, positionXZ.y);
        }

        StartCoroutine(SpawnPrefab());
    }

    IEnumerator SpawnPrefab()
    {
        yield return null;

        while (_prefabsCount < _prefabsCountMax)
        {
            var prefab = GameObject.Instantiate(_prefab, _positions[_prefabsCount++], Quaternion.identity);
            int materialIndex = _random.NextInt(_materials.Length);

            prefab.sharedMaterial = _materials[materialIndex];

            yield return new WaitForSeconds(_delay);
        }
    }
}
