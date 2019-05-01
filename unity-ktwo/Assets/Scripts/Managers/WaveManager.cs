using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Server authoritative. Tells the UI
// manager when to 
public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public const int POINTS_PER_ZOMBIE = 100;
    public const int POINTS_PER_EARLY_WAVE_BONUS = 5000;
    public const int POINTS_PER_WAVE = 10000;

    public static int SECONDS_IN_A_WAVE = 120;
    public static int SECONDS_IN_A_RESPITE = 10;

    public const int BASE_TOTAL_ZOMBIES = 3;
    public const int BASE_ZOMBIES_PER_TICK = 3;

    public const int WAVE_DIFFICULTY_FACTOR = 7;

    [Tooltip("In seconds")]
    public const int BASE_ZOMBIE_TICK_DURATION = 5;

    // Used only if a team wipes, we take the remaining
    // seconds, multiply it by this and add it to their
    // score.
    public const int POINTS_PER_SECONDS_SURVIVED = 50;

    // In seconds.
    public float currentWaveTime = 0;
    public float timeSinceLastTick = 0;

    public int zombiesKilled = 0;
    public int currentPoints = 0;
    public int currentWave = 1;

    public int currentZombiesPerTick = BASE_ZOMBIES_PER_TICK;
    public int currentZombieTickDuration = BASE_ZOMBIE_TICK_DURATION;

    // This all gets modified as the wave counter increments.
    public int waveTotalZombies = BASE_TOTAL_ZOMBIES;
    public int zombiesSpawnedInWave = 0;

    void Awake()
    {
        instance = this;
    }

    // Starting point to begin a wave.
    public void BeginWave()
    {
        StopAllCoroutines();
        currentWaveTime = 0;
        timeSinceLastTick = 0;
        zombiesSpawnedInWave = 0;
        StartCoroutine(ManageWave());
        WaveUIManager.instance.OnWaveBegin();
    }

    IEnumerator ManageWave()
    {
        while (currentWaveTime < SECONDS_IN_A_WAVE)
        {
            if (CheckAllZombiesDead()) EndWaveEarly();
            if (CheckAllPlayersDead()) OnAllPlayersDead();
            SpawnZombies();
            currentWaveTime += Time.deltaTime;
            timeSinceLastTick += Time.deltaTime;
            yield return null;
        }

        EndWave();
    }

    void OnAllPlayersDead()
    {
        StopAllCoroutines();
        WaveUIManager.instance.OnAllPlayersDead();
    }

    public bool CheckAllPlayersDead() 
    {
        int totalPlayers = PlayerManager.instance.players.Count;
        int deadPlayers = 0;

        // We can probably skip a for loop if we just keep track
        // of deaths in the player manager.
        foreach (var player in PlayerManager.instance.players)
        {
            if (player.GetComponent<PlayerBehaviour>().isDead)
            {
                deadPlayers++;
            }
        }

        return deadPlayers == totalPlayers;
    }

    public bool CheckAllZombiesDead()
    {
        return zombiesKilled == waveTotalZombies;
    }

    public void OnZombieDeath()
    {
        currentPoints += POINTS_PER_ZOMBIE;
    }

    public void UpdateVariablesForNextWave()
    {
        waveTotalZombies = BASE_TOTAL_ZOMBIES + currentWave * WAVE_DIFFICULTY_FACTOR;
    }

    void SpawnZombies()
    {
        if (timeSinceLastTick >= currentZombieTickDuration && zombiesSpawnedInWave < waveTotalZombies)
        {
            SpawnManager.instance.SpawnMultipleZombiesAtRandomPoints(currentZombiesPerTick);
            zombiesSpawnedInWave += currentZombiesPerTick;
            timeSinceLastTick = 0;
        }
    }

    void EndWave()
    {
        StopAllCoroutines();
        SpawnManager.instance.StopAllCoroutines();
        WaveUIManager.instance.OnWaveBegin();
        zombiesKilled = 0;
        UpdateVariablesForNextWave();
        currentPoints += POINTS_PER_ZOMBIE;
    }

    void EndWaveEarly()
    {
        currentPoints += POINTS_PER_EARLY_WAVE_BONUS;
        EndWave();
    }

    void BeginRespite()
    {
        WaveUIManager.instance.OnRespiteBegin();
        StartCoroutine(ManageRespite());
    }

    void EndRespite()
    {
        BeginWave();
    }

    IEnumerator ManageRespite()
    {
        while (currentWaveTime > 0)
        {
            currentWaveTime -= Time.deltaTime;
            yield return null;
        }

        EndRespite();
    }
}
