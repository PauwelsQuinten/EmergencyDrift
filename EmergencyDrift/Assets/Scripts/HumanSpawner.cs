using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _HumanPrefab;
    [SerializeField] private float _SpawnTime;

    private GameObject _spawnedHuman;
    private bool _isSpawning = false;

    private void Start()
    {
        SpawnHuman();
    }

    private void Update()
    {
        if (_spawnedHuman == null && !_isSpawning)
        {
            _isSpawning = true;
            StartCoroutine(DoTimer());

        }
    }

        private void SpawnHuman()
    {
        _spawnedHuman = Instantiate(_HumanPrefab,transform.position, transform.rotation);
    }

    private IEnumerator DoTimer()
    {
        yield return new WaitForSeconds(_SpawnTime);
        SpawnHuman();
        _isSpawning = false;
    }
}
