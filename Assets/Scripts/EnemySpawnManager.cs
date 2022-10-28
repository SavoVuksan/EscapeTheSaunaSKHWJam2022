using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    public bool DrawGizmos = true;
    [SerializeField]
    public float MaxDistanceSpawnRadius;
    [SerializeField]
    public float MinDistanceSpawnRadius;
    [SerializeField]
    public float SpawnCooldown = 2;
    [SerializeField]
    public int MaxEnemiesAlive;
    [SerializeField]
    public List<EnemySpawnerData> EnemySpawnerData;
    private List<Spawnpoint> _spawnpoints;
    private GameObject _player;
    private float _spawnCooldownTimer;
    private int _currentAliveEnemies;

    // Start is called before the first frame update
    void Start()
    {
        InitSpawners();
        _player = GameObject.FindGameObjectWithTag("Player");
        _spawnCooldownTimer = 0;
        _currentAliveEnemies = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Spawnpoint randomSpawnpoint = GetRandomSpawnpointInRange();
        if (randomSpawnpoint != null && _spawnCooldownTimer <= 0 && _currentAliveEnemies < MaxEnemiesAlive)
        {
            EnemySpawnerData enemySpawnerData = GetRandomEnemySpawnerData();
            if (enemySpawnerData != null)
            {
                Instantiate(enemySpawnerData.EnemyType, randomSpawnpoint.transform.position, Quaternion.identity);
                enemySpawnerData.CurrentSpawnCount++;
                _currentAliveEnemies++;
                _spawnCooldownTimer = SpawnCooldown;
                // TODO: Counting down _currentAliveEnemies & CurrentSpawnCount when enemy dies needs still to be done.
            }
        }


        UpdateSpawnpoints();

        DebugDraw();

        _spawnCooldownTimer -= Time.deltaTime;
        _spawnCooldownTimer = Mathf.Max(0, _spawnCooldownTimer);
    }

    public void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            if (_spawnpoints == null)
            {
                InitSpawners();
            }
            _spawnpoints.ForEach((spawnpoint) =>
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(spawnpoint.transform.position, MaxDistanceSpawnRadius);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(spawnpoint.transform.position, MinDistanceSpawnRadius);
            });
        }
    }

    private void InitSpawners()
    {
        _spawnpoints = GetChildren(gameObject)
        .Where((child) => child.tag.Equals("Spawner"))
        .Select((spawner) => spawner.GetComponent<Spawnpoint>()).ToList();
    }

    private List<GameObject> GetChildren(GameObject gameObject)
    {
        List<GameObject> children = new List<GameObject>();
        for (int c = 0; c < gameObject.transform.childCount; c++)
        {
            children.Add(gameObject.transform.GetChild(c).gameObject);
        }

        return children;
    }

    private float GetDistanceToPlayer(GameObject spawner)
    {
        return (_player.transform.position - spawner.transform.position).magnitude;
    }

    private void UpdateSpawnpoints()
    {
        _spawnpoints.ForEach((spawnpoint) =>
        {
            if (GetDistanceToPlayer(spawnpoint.gameObject) <= MaxDistanceSpawnRadius &&
            GetDistanceToPlayer(spawnpoint.gameObject) >= MinDistanceSpawnRadius)
            {
                spawnpoint.IsInRange = true;
            }
            else
            {
                spawnpoint.IsInRange = false;
            }
        });
    }

    private void DebugDraw()
    {
        _spawnpoints.ForEach((spawner) =>
        {
            if (spawner.IsInRange)
            {
                spawner.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else
            {
                spawner.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        });
    }

    private EnemySpawnerData GetRandomEnemySpawnerData()
    {
        var spawnableEnemies = EnemySpawnerData.Where((data) => data.CanSpawnEnemy()).ToList();
        if (spawnableEnemies.Count > 0)
        {
            var rand = Random.Range(0, spawnableEnemies.Count);
            return spawnableEnemies[rand];
        }
        else
        {
            return null;
        }
    }

    private Spawnpoint GetRandomSpawnpointInRange()
    {
        var spawnpointsInRange = _spawnpoints.Where((spawnpoint) => spawnpoint.IsInRange).ToList();
        if (spawnpointsInRange.Count > 0)
        {
            var rand = Random.Range(0, spawnpointsInRange.Count);
            return spawnpointsInRange[rand];
        }
        return null;
    }
}
