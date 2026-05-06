/*using System.Collections.Generic;
using UnityEngine;

public class MapTamplet : MonoBehaviour
{
    public Transform[] _enemySpawnPoint;
    public Transform[] _orbSpawnPoint;

    public Transform[] _playerSpawnPoint;

    [SerializeField] private List<GameObject> _orbList;
    [SerializeField] private List<GameObject> _enemyList;

    private void Awake()
    {
        SpawnOrb();
        SpawnEnemy();
    }

    private void SpawnOrb()
    {
        for (int i = 0; i < _orbSpawnPoint.Length; ++i)
        {
            int luck = Random.Range(0, 101);
            if (luck <= 65)
            {
                int orbVal = Random.Range(0, _orbList.Count);
                Instantiate(_orbList[orbVal], _orbSpawnPoint[i]);
            }
        }
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < _enemySpawnPoint.Length; ++i)
        {
            int luck = Random.Range(0, 101);
            if (luck <= 35)
            {
                int orbVal = Random.Range(0, _enemyList.Count);
                Instantiate(_enemyList[orbVal], _enemySpawnPoint[i]);
            }
        }
    }
}*/