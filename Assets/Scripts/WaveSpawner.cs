using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;
    public TextMeshProUGUI waveCountdownText;

    private float countdown = 2f;
    private int waveNumber = 0;

    void Update()
    {
        if (waveCountdownText == null)
            return;

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        if (waveNumber < waves.Length)
        {
            Wave wave = waves[waveNumber];

            for (int i = 0; i < wave.count; i++)
            {
                SpawnEnemy(wave.enemyPrefab);
                yield return new WaitForSeconds(1f / wave.rate);
            }

            waveNumber++;
        }
        else
        {
            waveCountdownText.text = "";
            Debug.Log("ALL WAVES COMPLETE!");
            this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}