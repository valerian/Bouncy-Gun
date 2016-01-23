using UnityEngine;
using System.Collections;

[System.Serializable]
public struct EnemyDefinition
{
    public GameObject prefab;
    public float spawnRate;
    public int spawnWorth;
}

public class GameManager : MonoBehaviour {
    // Managers instances
    public static GameManager instance;
    [HideInInspector]
    public UpgradeFactory bonusManager = new UpgradeFactory();

    // Public variables
    [Header("Special/Debug")]
    public float skipToLevel = 0;

    [Header("Current Level")]
    public int currentLevel = 0;
    public long currentScore = 0;
    [Space]
    public float healthMax = 100;
    public float energyMax = 100f;
    public float energyRegen = 5f;
    public float energyPerShot = 25f;
    [Space]
    public float fireRate = 1.0f;
    public float bulletVelocity = 2000f;
    public float bulletMass = 2f;
    public float bulletSize = 0.5f;
    public float bulletDuration = 3f;
    [Space]
    public float levelSpawnWorth = 50;
    [Space]
    public float mutationRate = 0f;

    [Header("Game")]
    public float initialSpawnTimeWorth = 8;
    [Space]
    public float outOfScreenY = 26f;
    public float outOfScreenBonusThrustFactor = 4f;
    public float outOfScreenBonusRotateFactor = 10f;
    [Space]
    public float outOfScreenFarY = 33f;
    public float outOfScreenFarBonusThrustFactor = 12f;
    public float outOfScreenFarBonusRotateFactor = 20f;
    [Space]
    public float levelIncreaseSpawnAmountFactor = 1.15f;
    public float levelIncreaseSpawnRateFactor = 1.15f;
    public float levelIncreaseMutateChanceFactor = 0.10f;
    [Space]
    public float cameraShakingTime = 0.5f;
    public float cameraShakingRadius = 1f;

    [Header("Enemies")]
    public float enemySpeedFactor = 1f;
    public float enemySpawnRateFactor = 1f;
    [Space]
    public EnemyDefinition[] enemies;

    [Header("Audio")]
    public AudioSource chargingAudio;
    public AudioSource fireAudio;

    [Header("Global references")]
    public GameObject gameCamera;

    // Player values
    public float energy { get; private set; }
    public float health { get; set; }

    // Gun handling
    public float chargingStartTime { get; private set; }
    public bool isCharged { get; private set; }
    public bool isCharging { get; private set; }
    
    // Level status
    public bool isLevelCleared { get { return enemyAliveCounter == 0 && enemySpawnTotalValue >= levelSpawnWorth; } }
    public bool isPlaying { get; private set; }

    // Level monitoring
    public int enemyAliveCounter { get; set; }    
    private int enemySpawnTotalValue = 0;

    // Application monitoring
    public bool isApplicationQuitting { get; private set; }
    
    void Awake ()
    {
        Application.targetFrameRate = 60;

        if (instance != null)
            Destroy(instance);
        instance = this;

        isApplicationQuitting = false;

        currentLevel = 1;
        currentScore = 0;

        TryLoadGame();
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
        isPlaying = true;
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
        SaveGame();
        StartGame();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private bool TryLoadGame()
    {
        System.Nullable<GameData> data = DataManager.gameData.Load();

        if (!data.HasValue)
            return false;

        bulletDuration  = data.Value.bulletDuration;
        bulletMass      = data.Value.bulletMass;     
        bulletSize      = data.Value.bulletSize;     
        bulletVelocity  = data.Value.bulletVelocity; 
        currentLevel    = data.Value.currentLevel;   
        currentScore    = data.Value.currentScore;   
        energyMax       = data.Value.energyMax;      
        energyPerShot   = data.Value.energyPerShot;  
        energyRegen     = data.Value.energyRegen;    
        fireRate        = data.Value.fireRate;       
        healthMax       = data.Value.healthMax;      
        levelSpawnWorth = data.Value.levelSpawnWorth;
        mutationRate    = data.Value.mutationRate;

        return true;
    }

    private void SaveGame()
    {
        GameData data = new GameData();

        data.bulletDuration  = bulletDuration;
        data.bulletMass      = bulletMass;     
        data.bulletSize      = bulletSize;     
        data.bulletVelocity  = bulletVelocity; 
        data.currentLevel    = currentLevel;   
        data.currentScore    = currentScore;   
        data.energyMax       = energyMax;      
        data.energyPerShot   = energyPerShot;  
        data.energyRegen     = energyRegen;    
        data.fireRate        = fireRate;       
        data.healthMax       = healthMax;      
        data.levelSpawnWorth = levelSpawnWorth;
        data.mutationRate    = mutationRate;

        DataManager.gameData.Save(data);
    }

    void FixedUpdate () {
        if (isPlaying && (health <= 0 || isLevelCleared))
        {
            isPlaying = false;
            isCharged = false;
            isCharging = false;
            chargingAudio.Stop();
        }

        if (!isPlaying)
            return;

        TrySpawnEnemies();

        energy = Mathf.Clamp(energy + (Time.deltaTime * energyRegen) - (isCharging ? ((Time.deltaTime / fireRate) * energyPerShot) : 0), 0f, energyMax);
    }

    void TrySpawnEnemies(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            for (int n = 0; n < enemies.Length; n++)
                TrySpawnEnemy(enemies[n].prefab, enemies[n].spawnRate, enemies[n].spawnWorth);
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
            SimplePool.Spawn(enemyObject, new Vector3(Random.Range(-4.5f, 4.5f), 27f, 0.35f), Quaternion.identity).SendMessage("Mutate", mutationLevel);
        }
    }

    public void ShakeCamera()
    {
        iTween.ShakePosition(gameCamera, new Vector3(cameraShakingRadius, cameraShakingRadius, 0f), cameraShakingTime);
    }

    void Update()
    {
        GunControl();
    }

    void GunControl()
    {
        if (!isPlaying)
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
            if (chargingAudio.time / Mathf.Abs(chargingAudio.pitch) >= fireRate + 0.1f)
                chargingAudio.pitch = -Mathf.Abs(chargingAudio.pitch);
            if (chargingAudio.time / Mathf.Abs(chargingAudio.pitch) <= Mathf.Max(fireRate - 0.1f, 0f))
                chargingAudio.pitch = Mathf.Abs(chargingAudio.pitch);
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
            chargingAudio.pitch = Mathf.Abs(chargingAudio.pitch);
            isCharged = false;
            isCharging = false;
        }
    }

    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
}
