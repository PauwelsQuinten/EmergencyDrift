using System;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _spawnPoints = new List<GameObject>();
    [SerializeField]
    private GameObject _enemyPrefab;

    private void Start()
    {
        SpawnAmbulances(this, EventArgs.Empty);
    }
    public void SpawnAmbulances(Component sender, object obj)
    {
        foreach (GameObject spawnPoint in _spawnPoints)
        {
            if (spawnPoint.GetComponentInChildren<Enemy>() != null) continue;
            GameObject enemy = Instantiate(_enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.transform.parent = null;
        }
    }
}
