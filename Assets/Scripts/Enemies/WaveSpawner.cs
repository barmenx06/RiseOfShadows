using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dalga bazl� d��man spawn ak���n� y�netir. 
/// �lk dalga: 10 sn, sonraki dalgalar: 30 sn bekleyerek ba�lar.
/// Her bir EnemySpawnInfo i�in �amount� ve �enemyPrefab� bilgisini kullan�r.
/// �nce SessionManager.StartSession() �a�r�l�r, ard�ndan dalga geri say�m� ve spawn ak��� gelir.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [Header("Dalga Ayarlar�")]
    [Tooltip("T�m dalga tan�mlar�n� i�eren liste")]
    public List<EnemyWave> waves;

    [Tooltip("Her d��man spawn�� aras�nda ge�ecek s�re (saniye)")]
    public float timeBetweenSpawns = 0.5f;

    [Header("Countdown S�releri (saniye)")]
    [Tooltip("�lk dalga ba�lamadan �nce bekleme s�resi")]
    public int firstCountdownTime = 10;

    [Tooltip("�lk dalgadan sonraki her dalga i�in bekleme s�resi")]
    public int regularCountdownTime = 30;

    [Header("Spawn Noktalar�")]
    [Tooltip("D��manlar�n spawn edilece�i Transform dizisi")]
    public Transform[] spawnPoints;

    [Header("UI")]
    [Tooltip("Dalga geri say�m� metnini y�neten controller")]
    public WaveUIController waveUIController;

    private Coroutine spawnRoutine;
    private bool isSpawning = false;
    private bool sessionStarted = false;
    private SessionManager sessionManager;

    private void Start()
    {
        // SessionManager referans�n� al
        sessionManager = FindObjectOfType<SessionManager>();

        // Dalga ak���n� ba�lat
        StartSpawning();
    }

    /// <summary>
    /// Dalga spawn ak���n� ba�lat�r (ilk kez tetiklemek i�in).
    /// </summary>
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            spawnRoutine = StartCoroutine(SpawnAllWaves());
        }
    }

    /// <summary>
    /// Dalga ak���n� durdurur (�rne�in, oyun bitince).
    /// </summary>
    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        isSpawning = false;
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int i = 0; i < waves.Count && isSpawning; i++)
        {
            // �� �lk dalga �ncesi, timer'� ba�lat �� 
            if (!sessionStarted)
            {
                if (sessionManager != null)
                    sessionManager.StartSession();
                sessionStarted = true;
            }

            // Bekleme s�resini belirle (ilk dalga m� kontrol�)
            int waitTime = (i == 0) ? firstCountdownTime : regularCountdownTime;
            yield return StartCoroutine(WaveCountdown(i, waitTime));

            // Dalga i�erisindeki d��manlar� s�rayla spawn et
            yield return StartCoroutine(SpawnWave(waves[i]));
        }
    }

    private IEnumerator WaveCountdown(int waveIndex, int waitTime)
    {
        int t = waitTime;
        string waveName = $"Wave {waveIndex + 1}";

        if (waveUIController != null)
            waveUIController.ShowWaveCountdown(waveName, t);

        while (t > 0 && isSpawning)
        {
            yield return new WaitForSeconds(1f);
            t--;
            if (waveUIController != null)
                waveUIController.ShowWaveCountdown(waveName, t);
        }

        // Geri say�m s�f�rland�ktan sonra son mesaj� 3 sn daha g�ster
        if (waveUIController != null)
            waveUIController.ShowWaveCountdown(waveName, 0);

        yield return new WaitForSeconds(3f);

        if (waveUIController != null)
            waveUIController.HideWaveText();
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        // Her EnemySpawnInfo i�in "amount" kadar "enemyPrefab" spawn et
        foreach (EnemySpawnInfo info in wave.enemiesInWave)
        {
            for (int i = 0; i < info.amount && isSpawning; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Vector3 spawnPosition = spawnPoint.position;
                spawnPosition.z = 0f; // 2D oyun i�in z = 0
                GameObject enemy = Instantiate(info.enemyPrefab, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }
}
