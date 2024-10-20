using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] int initialEnemies;
    [SerializeField] int maxEnemies;
    [SerializeField] Vector2 spawnMaxLimits;
    [SerializeField] Vector2 playerOffSetLimits;
    [SerializeField] int difficultIncreases = 2;
    int currentDifficultyIncrease = 0;
    [SerializeField] float initialWaveRateTime = 10;
    [SerializeField] float timeDecreasedBetweenWaves = -1;
    [SerializeField] int enemyRateIncrease = 7;
    [SerializeField] float timeBeforeIncreaseDifficulty = 15;
    [SerializeField] Slider timerSlider;
    [SerializeField] TextMeshProUGUI dangerLevelText;
    float timeBetweenWaves;

    private void Start()
    {
        SpawnWave(initialEnemies);
        timeBetweenWaves = initialWaveRateTime;
        StartCoroutine(Timer());
        StartCoroutine(WaitForWave());
    }

    void SpawnRandomEnemy()
    {
        if (transform.childCount > maxEnemies) return;

        int i = Random.Range(0, enemies.Count);
        Instantiate(enemies[i], CalculatePos(), Quaternion.identity, transform);
    }

    void SpawnWave(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnRandomEnemy();
        }
    }

    Vector3 CalculatePos()
    {
        Vector3 vector3;
        float randomX;
        float randomY;

        randomX = Random.Range(-spawnMaxLimits.x, spawnMaxLimits.x);
        randomY = Random.Range(-spawnMaxLimits.y, spawnMaxLimits.y);
        vector3 = new Vector3(randomX, randomY, 0);
        return vector3;
    }

    IEnumerator WaitForWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        SpawnWave(enemyRateIncrease * currentDifficultyIncrease + initialEnemies);
        StartCoroutine(WaitForWave());
    }
    IEnumerator Timer()
    {
        for (float i = 0; i < timeBeforeIncreaseDifficulty; i += Time.deltaTime)
        {
            timerSlider.value = Mathf.Lerp(timerSlider.minValue, timerSlider.maxValue, i / timeBeforeIncreaseDifficulty);
            yield return null;
        }
        IncreaseDifficulty();
        if (currentDifficultyIncrease < difficultIncreases)
            StartCoroutine(Timer());
    }

    void IncreaseDifficulty()
    {
        currentDifficultyIncrease++;
        timeBetweenWaves -= timeDecreasedBetweenWaves;
        dangerLevelText.SetText((currentDifficultyIncrease + 1).ToString());
    }
}
