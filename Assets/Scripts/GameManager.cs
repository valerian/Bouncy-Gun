using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject enemy;
    public GameObject enemySpeeder;
    public GameObject enemyBoss;
    public int bossSpawnRate = 15;
    public int speederSpawnRate = 5;
    public float maxHealth = 100;
    public float enemyEscapeDamage = 10f;
    public float fireRate = 1.0f;
    public float energyMax = 100f;
    public float energyRegen = 5f;
    public float energyPerShot = 25f;
    public float bulletVelocity = 2000f;
    public float bulletMass = 2f;
    public float bulletSize = 0.5f;
    public float bulletDuration = 3f;
    public int initialEnemySpawn = 8;
    public float averageSpawnPerSecond = 0.25f;
    public AudioSource chargingAudio;
    public AudioSource fireAudio;

    public float chargingStartTime { get; private set; }
    public bool isCharged { get; private set; }
    public bool isCharging { get; private set; }

    public float energy { get; private set; }
    public float health { get; private set; }
    public bool playing { get; private set; }

    public int enemyCounter { get; private set; }

    private int spawnCounter = 0;

    void Awake ()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        StartGame();

    }

    void StartGame()
    {
        health = maxHealth;
        playing = true;
        energy = energyMax;
        isCharged = false;
        isCharging = false;
        enemyCounter = 0;

        for (int i = 0; i <= initialEnemySpawn; i++)
        {
            spawnCounter++;
            Instantiate(enemy, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
        }
    }

    void FixedUpdate () {
        if (Random.value <= (averageSpawnPerSecond * Time.deltaTime))
        {
            spawnCounter++;
            if (spawnCounter % bossSpawnRate == 0)
                Instantiate(enemyBoss, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
            else if (spawnCounter % speederSpawnRate == 0)
                Instantiate(enemySpeeder, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
            else
                Instantiate(enemy, new Vector3((Random.value * 6f) - 3f, 27f, 0.35f), Quaternion.identity);
        }

        if (health <= 0)
        {
            playing = false;
            isCharged = false;
            isCharging = false;
            chargingAudio.Stop();
        }

        energy = Mathf.Clamp(energy + (Time.deltaTime * energyRegen) - (isCharging ? ((Time.deltaTime / fireRate) * energyPerShot) : 0), 0f, energyMax);
    }

    void Update()
    {
        GunControl();
    }

    void GunControl()
    {
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

    void OnGUI()
    {
        drawBars();
    }

    void drawBars()
    {
        // Generate a single pixel texture if it doesn't exist
        var lineTex = new Texture2D(1, 1);

        Color savedColor = GUI.color;


        Vector2 topLeft = Camera.main.WorldToScreenPoint(new Vector3(-7.5f, 20f, -0.5f));
        Vector2 bottomRight = Camera.main.WorldToScreenPoint(new Vector3(-6.4f, 0f, -0.5f));

        //Draw background
        GUI.color = new Color(0f, 0f, 0f);
        GUI.DrawTexture(new Rect(topLeft.x, 0, bottomRight.x - topLeft.x, Screen.height), lineTex);

        //Draw energy bar
        GUI.color = energy > energyPerShot ? new Color(0.4f, 0.4f, 1f) : new Color(0.8f, 0.2f, 0.2f);
        GUI.DrawTexture(new Rect(topLeft.x + ((bottomRight.x - topLeft.x) / 2f), Screen.height * (1f - (energy / energyMax)), (bottomRight.x - topLeft.x) / 2f, Screen.height * (energy / energyMax)), lineTex);

        //Draw charge bar
        if (isCharged || isCharging)
        {
            GUI.color = new Color(1.0f, 0.8f, 0.0f);
            GUI.DrawTexture(new Rect(topLeft.x, Screen.height * (1f - ((Time.time - chargingStartTime) / fireRate)), (bottomRight.x - topLeft.x) / 2f, Screen.height * ((Time.time - chargingStartTime) / fireRate)), lineTex);
        }

        //Draw limit bar
        GUI.color = new Color(1f, 0f, 0f);
        GUI.DrawTexture(new Rect(topLeft.x + ((bottomRight.x - topLeft.x) / 2f), Screen.height * (1f - (energyPerShot / energyMax)), (bottomRight.x - topLeft.x) / 2f, 3), lineTex);

        topLeft = Camera.main.WorldToScreenPoint(new Vector3(6.4f, 20f, -0.5f));
        bottomRight = Camera.main.WorldToScreenPoint(new Vector3(7.5f, 0f, -0.5f));

        //Draw background
        GUI.color = new Color(0f, 0f, 0f);
        GUI.DrawTexture(new Rect(topLeft.x, 0, bottomRight.x - topLeft.x, Screen.height), lineTex);

        //Draw health bar
        GUI.color = new Color(1f, 0.2f, 0.2f);
        GUI.DrawTexture(new Rect(topLeft.x, Screen.height * (1f - (GameManager.instance.health / GameManager.instance.maxHealth)), bottomRight.x - topLeft.x, Screen.height * (GameManager.instance.health / GameManager.instance.maxHealth)), lineTex);

        // We're done.  Restore the GUI matrix and GUI color to whatever they were before.
        GUI.color = savedColor;
    }
}
