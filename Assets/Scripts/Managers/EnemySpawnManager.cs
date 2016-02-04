using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Reflection;

[System.Serializable]
public struct EnemyDefinition
{
    public GameObject prefab;
    public float spawnRate;
    public int spawnWorth;
}

public class EnemySpawnManager : MonoBehaviour
{
    // INTERNAL OBSERVER COMPONENT

    public class EnemySpawnManagerObserver : MonoBehaviour
    {
        private EnemySpawnManager spawnManager;

        public static EnemySpawnManagerObserver AddOrUpdateComponent(GameObject where, EnemySpawnManager spawnManager)
        {
            EnemySpawnManagerObserver component = where.GetComponent<EnemySpawnManagerObserver>() ?? where.AddComponent<EnemySpawnManagerObserver>();
            component.spawnManager = spawnManager;
            return component;
        }

        void OnDisable()
        {
            spawnManager.OnEnemyDeath(gameObject);
        }
    }

    // SETTINGS

    [SerializeField] private EnemyDefinition[] enemies;
    [SerializeField] private float preSpawnTimeWorth = 8;
    [SerializeField] private float baseLevelSpawnAmount = 30f;
    [SerializeField] private float levelIncreaseSpawnAmountMultiplier = 1.15f;
    [SerializeField] private float levelIncreaseSpawnRateMultiplier = 1.15f;
    [SerializeField] private float levelIncreaseMutateChanceIncrement = 0.10f;
    
    // PRIVATE FIELDS

    private UnityEvent _onLevelCleared;
    private bool _isLevelCleared = false;
    private int enemySpawnTotalValue = 0;
    private float spawnAmountFactor;
    private float spawnRateFactor;
    private float mutateChance;

    // PUBLIC PROPERTIES

    public int enemyAliveCounter { get; private set; }
    public UnityEvent onLevelCleared { get { return _onLevelCleared ?? (_onLevelCleared = new UnityEvent()); } }
    public bool isLevelCleared
    {
        get { return _isLevelCleared; }

        private set
        {
            bool changedTrue = _isLevelCleared == false && value == true;
            _isLevelCleared = value;
            if (changedTrue)
                onLevelCleared.Invoke();
        }
    }

    // METHODS

    void Awake()
    {
        Game.instance.onState[Game.STATE.play].AddListener(OnPlayStart);
        //DebugTestComponents();
    }

    void DebugTestComponents()
    {
        GameObject prefab = enemies[0].prefab;
        Component[] components = prefab.GetComponents<Component>();
        foreach (Component comp in components)
        {
            Debug.Log("Component: " + comp.GetType());
        }

        foreach (Component comp in components)
        {
            Debug.Log("=============\nComponent: " + comp.GetType());
            System.Type type = comp.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties();
            foreach (var pinfo in pinfos)
            {
                Debug.Log("Property " + pinfo.Name + " (CanWrite = " + pinfo.CanWrite + ")");
            }
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                Debug.Log("Field " + finfo.Name);
            }
        }
    }

    void OnPlayStart()
    {
        isLevelCleared = false;
        enemySpawnTotalValue = 0;
        spawnAmountFactor = Mathf.Pow(levelIncreaseSpawnAmountMultiplier, Game.GameData.currentLevel);
        spawnRateFactor = Mathf.Pow(levelIncreaseSpawnRateMultiplier, Game.GameData.currentLevel);
        mutateChance = Game.GameData.currentLevel * levelIncreaseMutateChanceIncrement;
        TrySpawnEnemies(Mathf.RoundToInt((1f / Time.deltaTime) * preSpawnTimeWorth));
    }

    void TrySpawnEnemies(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            for (int n = 0; n < enemies.Length; n++)
                TrySpawnEnemy(enemies[n]);
        }
    }

    void TrySpawnEnemy(EnemyDefinition enemyDefinition)
    {
        if (Random.value <= (enemyDefinition.spawnRate * Time.deltaTime * spawnRateFactor))
        {
            int mutationLevel = 1;
            while (mutateChance - mutationLevel > 0f)
                mutationLevel++;
            if (Random.value < mutateChance + 1f - mutationLevel)
                mutationLevel++;
            enemyAliveCounter++;
            enemySpawnTotalValue += enemyDefinition.spawnWorth * (int)(Mathf.Pow(2, mutationLevel) / 2f);
            GameObject enemy = Pool.Spawn(enemyDefinition.prefab, new Vector3(Random.Range(-4.5f, 4.5f), 27f, 0.35f), Quaternion.identity);
            EnemySpawnManagerObserver.AddOrUpdateComponent(enemy, this);
        }
    }

    void FixedUpdate()
    {
        if (isLevelCleared || Game.State != Game.STATE.play)
            return;
        if (enemySpawnTotalValue < baseLevelSpawnAmount * spawnAmountFactor)
            TrySpawnEnemies();
        else if (enemyAliveCounter <= 0)
            isLevelCleared = true;
    }

    void OnEnemyDeath(GameObject enemy)
    {
        enemyAliveCounter--;
    }
}
