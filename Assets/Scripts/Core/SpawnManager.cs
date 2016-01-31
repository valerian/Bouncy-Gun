using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public struct EnemyDefinition
{
    public GameObject prefab;
    public float spawnRate;
    public int spawnWorth;
}

public class SpawnManager : MonoBehaviour
{
    private UnityEvent _onLevelCleared;
    public UnityEvent onLevelCleared { get { return _onLevelCleared ?? (_onLevelCleared = new UnityEvent()); } }

    [SerializeField] private EnemyDefinition[] enemies;
    [SerializeField] private float initialSpawnTimeWorth = 8;
    [SerializeField] private float enemySpawnRateFactor = 1f;
    [SerializeField] private float levelIncreaseSpawnAmountFactor = 1.15f;
    [SerializeField] private float levelIncreaseSpawnRateFactor = 1.15f;
    [SerializeField] private float levelIncreaseMutateChanceFactor = 0.10f;

    public int enemyAliveCounter { get; set; }
    private int enemySpawnTotalValue = 0;

    private bool _isLevelCLeared = false;
    public bool isLevelCLeared
    {
        get { return _isLevelCLeared; }

        private set
        {
            bool changedTrue = _isLevelCLeared == false && value == true;
            _isLevelCLeared = value;
            if (changedTrue)
                onLevelCleared.Invoke();
        }
    }

    private float currentLevelSpawnAmountFactor;
    private float currentLevelSpawnRateFactor;
    private float currentLevelMutateChanceFactor;

    void FixedUpdate()
    {
        if (Game.State != Game.STATE.play)
            return;
    }
}
