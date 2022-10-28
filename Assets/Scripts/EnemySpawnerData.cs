using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnerData
{
    public GameObject EnemyType;
    public int MaxSpawnCount;
    private int _currentSpawnCount;

    public int CurrentSpawnCount {
        get{
            return _currentSpawnCount;
        }
        set{
            _currentSpawnCount = value;
        }
    }

    public EnemySpawnerData(){
        _currentSpawnCount = 0;
    }

    public bool CanSpawnEnemy(){
        return _currentSpawnCount < MaxSpawnCount;
    }
}
