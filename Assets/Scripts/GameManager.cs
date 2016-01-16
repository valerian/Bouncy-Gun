using UnityEngine;
using System.Collections;

[System.Serializable]
public struct EnemyDefinition
{
    public GameObject prefab;
    public float spawnRate;
    public int scoreValue;
    public int damage;
}

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public float skipToLevel = 0;

    public float healthMax = 100;
    public float enemyEscapeDamage = 10f;
    public float fireRate = 1.0f;
    public float energyMax = 100f;
    public float energyRegen = 5f;
    public float energyPerShot = 25f;
    public float bulletVelocity = 2000f;
    public float bulletMass = 2f;
    public float bulletSize = 0.5f;
    public float bulletDuration = 3f;
    public float initialSpawnTimeWorth = 8;
    public float levelSpawnWorth = 50;

    public float outOfScreenY = 27f;
    public float outOfScreenBonusThrustFactor = 5f;
    public float outOfScreenBonusRotateFactor = 10f;

    public float levelIncreaseSpawnAmountFactor = 1.15f;
    public float levelIncreaseSpawnRateFactor = 1.15f;
    public float levelIncreaseMutateChanceFactor = 0.10f;

    public float enemySpeedFactor = 1f;

    public float enemySpawnRateFactor = 1f;

    public EnemyDefinition[] enemies;

    public AudioSource chargingAudio;
    public AudioSource fireAudio;

    public int currentLevel { get; private set; }

    public float chargingStartTime { get; private set; }
    public bool isCharged { get; private set; }
    public bool isCharging { get; private set; }

    public float energy { get; private set; }
    public float health { get; private set; }
    public bool playing { get; private set; }

    private int enemySpawnTotalValue = 0;
    private float mutationRate = 0f;
    public int enemyAliveCounter {get; set;}
    public bool levelCleared { get { return enemyAliveCounter == 0 && enemySpawnTotalValue >= levelSpawnWorth; } }

    public long score { get; set; }

    private BonusManager bonusManager = new BonusManager();

    void Awake ()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        currentLevel = 1;
        score = 0;
        StartGame();
    }

    void Destroy()
    {
        instance = null;
    }

    void StartGame()
    {
        // Clean memory
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        health = healthMax;
        playing = true;
        energy = energyMax;
        isCharged = false;
        isCharging = false;
        enemySpawnTotalValue = 0;
        enemyAliveCounter = 0;

        if (skipToLevel > 0)
        {
            skipToLevel--;
            enemySpawnTotalValue = (int) levelSpawnWorth + 1;
            return;
        }

        // Instantly spawn X seconds worth of enemies
        TrySpawnEnemies(Mathf.RoundToInt((1f / Time.deltaTime) * initialSpawnTimeWorth));
    }

    public void NextLevel()
    {
        levelSpawnWorth *= levelIncreaseSpawnAmountFactor;
        enemySpawnRateFactor *= levelIncreaseSpawnRateFactor;
        mutationRate += levelIncreaseMutateChanceFactor;
        currentLevel++;
        StartGame();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void FixedUpdate () {
        if (health <= 0 || levelCleared)
        {
            playing = false;
            isCharged = false;
            isCharging = false;
            chargingAudio.Stop();
        }

        TrySpawnEnemies();

        energy = Mathf.Clamp(energy + (Time.deltaTime * energyRegen) - (isCharging ? ((Time.deltaTime / fireRate) * energyPerShot) : 0), 0f, energyMax);
    }

    void TrySpawnEnemies(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            for (int n = 0; n < enemies.Length; n++)
                TrySpawnEnemy(enemies[n].prefab, enemies[n].spawnRate, enemies[n].scoreValue);
        }
    }

    void TrySpawnEnemy(GameObject enemyObject, float spawnRate, int scoreValue)
    {
        if (enemySpawnTotalValue >= levelSpawnWorth)
            return;
        if (Random.value <= (spawnRate * Time.deltaTime * enemySpawnRateFactor))
        {
            int mutationLevel = 1;
            while (mutationRate - mutationLevel > 0f)
                mutationLevel++;
            if (Random.value < mutationRate + 1f - mutationLevel)
                mutationLevel++;
            enemyAliveCounter++;
            enemySpawnTotalValue += scoreValue * (int) (Mathf.Pow(2, mutationLevel) / 2f);
            SimplePool.Spawn(enemyObject, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity).SendMessage("Mutate", mutationLevel);
        }
    }

    public Bonus[] GetNewRandomBonusses()
    {
        return bonusManager.GetRandomBonus(2, 1, 2, 3);
    }

    void Update()
    {
        GunControl();
    }

    void GunControl()
    {
        if (!playing)
            return;
        if (!isCharging && !isCharged && energy >= energyPerShot && Input.GetButton("Fire1"))
        {
            isCharging = true;
            chargingStartTime = Time.time;
            BroadcastMessage("Charging", fireRate, SendMessageOptions.DontRequireReceiver);
            chargingAudio.Play();
        }

        if (isCharging && !isCharged && Input.GetButton("Fire1") && Time.time >= chargingStartTime + fireRate)
        {
            isCharging = false;
            isCharged = true;
            BroadcastMessage("Charged", null, SendMessageOptions.DontRequireReceiver);
        }

        if (isCharged)
        {
            if (chargingAudio.time >= fireRate * 1.2f)
                chargingAudio.pitch = -1.2f;
            if (chargingAudio.time <= fireRate * 1.0f)
                chargingAudio.pitch = 1.2f;
        }

        if ((isCharging || isCharged) && Input.GetButtonUp("Fire1"))
        {
            float powerRatio = 1.0f;
            if (!isCharged)
            {
                powerRatio = (Time.time - chargingStartTime) / fireRate;
            }
            BroadcastMessage("Fired", powerRatio * bulletVelocity * bulletMass, SendMessageOptions.DontRequireReceiver);
            fireAudio.Play();
            fireAudio.pitch = 1.3f - (powerRatio * powerRatio * 0.6f);
            fireAudio.volume = 0.066f + (powerRatio * powerRatio * 0.2f);
            chargingAudio.Stop();
            chargingAudio.pitch = 1f;
            isCharged = false;
            isCharging = false;
        }
    }

    public void EnemyEscaped(GameObject enemy)
    {
        health -= enemyEscapeDamage;
    }
}
