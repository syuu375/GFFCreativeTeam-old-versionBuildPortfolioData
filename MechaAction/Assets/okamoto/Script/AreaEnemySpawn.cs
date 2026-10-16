using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemySpawn : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        [Header("出す敵")]
        public GameObject enemyPrefab;

        [Header("出す位置")]
        public Transform spawnPoint;
    }

    [System.Serializable]
    public class Wave
    {
        [Header("このWaveで出す敵たち")]
        public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
    }

    [Header("Wave設定")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    private int currentWaveIndex = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool spawning = false;
    private Cameralimit _limit;

    private void Start()
    {
        _limit = GetComponent<Cameralimit>();
    }

    // Cameralimitから呼び出して最初のWaveを開始
    public void StartSpawn()
    {
       // Debug.Log($"isSpawning : {spawning}");
        if (spawning) return;
        spawning = true;
        //Debug.Log($"Spawn");
        currentWaveIndex = 0;
        SpawnWave(currentWaveIndex);
    }

    // Waveを生成
    private void SpawnWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            Debug.Log("全てのWaveが終了しました！");
            // Cameralimitに通知してエリア解放
            _limit.Clear();
            spawning = false;
            //if (limit != null) limit.OnEnemiesCleared();
            return;
        }

        Debug.Log($"Wave {waveIndex + 1} 開始");

        aliveEnemies.Clear();

        foreach (var enemyData in waves[waveIndex].enemies)
        {
            GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPoint.position, Quaternion.identity);
            aliveEnemies.Add(enemy);

        }
        StartCoroutine(WaitUntilAllDead());

    }

    private IEnumerator WaitUntilAllDead()
    {
        while(aliveEnemies.Exists(enemy => enemy != null))
            yield return null;

        currentWaveIndex++;
        SpawnWave(currentWaveIndex);
    }

    // 敵死亡時の処理
    private void OnEnemyDied(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);

        // Wave全滅 → 次のWaveへ
        if (aliveEnemies.Count == 0)
        {
            currentWaveIndex++;
            SpawnWave(currentWaveIndex);
        }
    }
}
