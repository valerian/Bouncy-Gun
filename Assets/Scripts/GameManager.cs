using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject enemy;
    public GameObject enemyBoss;
    public int bossRate = 15;

    private int spawnCounter = 0;

    void FixedUpdate () {
        if (Random.value >= 0.995f)
        {
            spawnCounter++;
            if (spawnCounter % bossRate == 0)
                Instantiate(enemyBoss, new Vector3((Random.value * 6f) - 3f, 23f, 0.35f), Quaternion.identity);
            else
                Instantiate(enemy, new Vector3((Random.value * 6f) - 3f, 23f, 0.35f), Quaternion.identity);
        }
    }
}
