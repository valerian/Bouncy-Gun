using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public Scene gameScene;
    public GameObject menuTank;
    public float spawnFrequency;

    private float lastSpawn = -1f;

    public void play()
    {
        SceneManager.LoadScene("Game");
    }

    void Update()
    {
        if (Time.time > lastSpawn + spawnFrequency)
        {
            Pool.Spawn(menuTank, new Vector3(1.5f, -0.35f, 1.5f), Quaternion.Euler(new Vector3(90, 0, 0)));
            Pool.Spawn(menuTank, new Vector3(5f, -0.35f, -45f), Quaternion.Euler(new Vector3(90, 180, 0)));
            lastSpawn = Time.time + Random.Range(0f, spawnFrequency * 2);
        }
    }
}
