using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float initialSpawnDelay = 5f;
    public int initialSpawnCount = 1;
    public float waveDelay = 10f;
    public float timePerWave = 20f;
    public EnemySpawnPoint[] spawnPoints;

    bool waveCleared = false;
    bool gameCleared = false;

    List<GameObject> spawnedEnemyList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        StartCoroutine(StartWaveSpawn());
    }

    void Update()
    {
        timePerWave -= Time.deltaTime;
        if (timePerWave < 0)
        {
            waveCleared = true;
            foreach (GameObject enemyObject in spawnedEnemyList)
            {
                var enemy = enemyObject.GetComponent<EnemyData>();
                if(enemy.IsDead == false)
                {
                    waveCleared = false;
                    break;
                }
            }
        }
    }

    IEnumerator StartWaveSpawn()
    {
        while(true)
        {
            // 게임 클리어 조건 달성
            if (gameCleared)
            {
                // 더 이상 생성할 필요 없다.
                break;
            }

            waveCleared = false;
            timePerWave = 30f;

            Debug.Log("New Wave Start!!");

            // 웨이브당 스폰 시작
            while (true)
            {
                // 웨이브 클리어
                if (waveCleared)
                {
                    Debug.Log("Wave Clear!!");
                    spawnedEnemyList.Clear();
                    break;
                }

                if (timePerWave < 0)
                {
                    yield return null;
                    continue;
                }

                // 스폰 시작
                for (int i = 0; i < initialSpawnCount; ++i)
                {
                    SpawnEnemy();
                }

                yield return new WaitForSeconds(initialSpawnDelay);
            }

            yield return new WaitForSeconds(waveDelay);
        }
    }

    void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
        GameObject enemy = spawnPoints[spawnPointIndex].SpawnEnemy();
        enemy.GetComponent<EnemyLocomotion>().manager = this;
        spawnedEnemyList.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemyList.Remove(enemy);
        Destroy(enemy);
    }
}
