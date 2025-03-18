using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour {
    [System.Serializable]
    public struct Wave {
        public int slow;
        public int normal;
        public int fast;

        public Wave(int s, int n, int f) {
            slow = s;
            normal = n;
            fast = f;
        }

        public int getEnemiesLeft() {
            return slow + normal + fast;
        }
    }

    public List<Wave> waves = new List<Wave>();
    public SplineContainer easyPath, normalPath, hardPath;
    public GameObject slowEnemy, normalEnemy, fastEnemy;

    List<EnemyBase> enemies = new List<EnemyBase>();

    public enum WaveState { waveInProgrees, allEnemiesSpawned, waveComplete }
    public WaveState waveState;

    int currentWaveIndex = 0;

    Timer timer = new Timer();
    bool timerRunning = false;
    public float spawnTime = 0.5f;

    private void Start() {
        timer = GetComponent<Timer>();
        if(timer == null) {
            Debug.LogError("Error[EnemySpawner] - No Timer Added");
        }
        timer.timerComplete += SpawnEnemy;
    }

    private void Update() {
        if(!timerRunning) {
            timerRunning = true;
            timer.StartTimer(spawnTime);
        }
    }

    public void SpawnEnemy() {
        SplineContainer path = RandomPath();
        GameObject enemyToSpawn = RandomEnemy();
        if(enemyToSpawn == null) {
            timer.Pause();
            waveState = WaveState.allEnemiesSpawned;
            return;
        }

        GameObject enemy = Instantiate(enemyToSpawn);

        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.SetInfoAndStart(path);
        enemyBase.enemyDeath += EnemyKilled;

        enemies.Add(enemyBase);

        timerRunning = false;
    }

    public void EnemyKilled(EnemyBase enemy) {
        enemies.Remove(enemy);

        if(enemies.Count == 0) {
            waveState = WaveState.waveComplete;
            Debug.Log("Wave Complete");
        }
    }

    public SplineContainer RandomPath() {
        int randomNum = Random.Range(0, 100);

        if(randomNum < 60) {
            return easyPath;
        }

        if(randomNum < 90) {
            return normalPath;
        }

        return hardPath;
    }

    public GameObject RandomEnemy() {
        Wave currentWave = waves[currentWaveIndex];
        int enemiesLeft = currentWave.getEnemiesLeft();
        if(enemiesLeft <= 0) {
            return null;
        }

        int selectedIndex = Random.Range(0, enemiesLeft);

        if(selectedIndex < currentWave.slow) {
            waves[currentWaveIndex] = new Wave(currentWave.slow-1, currentWave.normal, currentWave.fast);
            return slowEnemy;
        }

        if(selectedIndex < currentWave.slow + currentWave.normal) {
            waves[currentWaveIndex] = new Wave(currentWave.slow, currentWave.normal-1, currentWave.fast);
            return normalEnemy;
        }

        waves[currentWaveIndex] = new Wave(currentWave.slow, currentWave.normal, currentWave.fast-1);
        return fastEnemy;
    }
}
