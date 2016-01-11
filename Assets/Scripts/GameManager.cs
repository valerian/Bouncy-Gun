using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject enemy;
    public GameObject enemyBoss;
    public int bossRate = 15;
    public float maxHealth = 100;
    public float health;
    public bool playing = true;

    private int spawnCounter = 0;

    void Awake ()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        health = maxHealth;
        for (int i = 0; i <= 8; i++)
        {
            spawnCounter++;
            Instantiate(enemy, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
        }
    }

    void FixedUpdate () {
        if (Random.value >= 0.995f)
        {
            spawnCounter++;
            if (spawnCounter % bossRate == 0)
                Instantiate(enemyBoss, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
            else
                Instantiate(enemy, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
        }

        if (health <= 0)
        {
            playing = false;
        }
    }
}
