using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public float initialVelocity = 2000f;
    public GameObject bullet;
    public float fireRate = 2.0f;
    public float boardWidth = 12.5f;
    public float outOfSightY = 30f;
    public Color predictiveLineColor = Color.white;
    public float energyMax = 100f;
    public float energyRegen = 0.1f;
    public float energyPerShot = 25f;
    public AudioSource fireAudio;
    public AudioSource chargingAudio;

    private float chargingStartTime;
    private bool isCharged = false;
    private bool isCharging = false;
    private float energy = 100f;

    private float health;

    // Use this for initialization
    void Start () 
    {

    }
    
    void FixedUpdate ()
    {
        if (GameManager.instance.playing == false)
        {
            return;
        }
        energy = Mathf.Clamp(energy + energyRegen - (isCharging ? ((Time.deltaTime / fireRate) * energyPerShot) : 0 ), 0f, energyMax);
    }

    // Update is called once per frame
    void Update () 
    {
        if (GameManager.instance.playing == false)
        {
            isCharged = false;
            isCharging = false;
            transform.rotation = Quaternion.identity;
            chargingAudio.Stop();
            return;
        }
        UpdateAimAndFire();
    }

    private void UpdateAimAndFire()
    {
        var mouse_pos = Input.mousePosition;
        mouse_pos.z = 20f; //The distance between the camera and object
        var object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        if (true)
        {
            mouse_pos.x = -mouse_pos.x;
            mouse_pos.y = -mouse_pos.y;
        }
        var angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        if (!isCharging && !isCharged && energy >= energyPerShot && Input.GetButton("Fire1"))
        {
            isCharging = true;
            chargingStartTime = Time.time;
            BroadcastMessage("Charging", null, SendMessageOptions.DontRequireReceiver);
            chargingAudio.Play();
        }

        if (isCharging && !isCharged && Input.GetButton("Fire1") && Time.time >= chargingStartTime + fireRate)
        {
            isCharging = false;
            isCharged = true;
            //chargingAudio.Stop();
            BroadcastMessage("Charged", null, SendMessageOptions.DontRequireReceiver);
        }

        if (isCharged)
        {
            if (chargingAudio.time >= 1.2f)
                chargingAudio.pitch = -1.2f;
            if (chargingAudio.time <= 1.0f)
                chargingAudio.pitch = 1.2f;
        }

        if ((isCharging || isCharged) && Input.GetButtonUp("Fire1"))
        {
            float powerRatio = 1.0f;
            if (!isCharged)
            {
                powerRatio = (Time.time - chargingStartTime) / fireRate;
            }
            GameObject instance = (GameObject)Instantiate(bullet, transform.position + (transform.up * 1.5f) + new Vector3(0, 0, -0.2f), transform.rotation);
            instance.GetComponent<Bullet>().Launch(transform.up, initialVelocity * powerRatio);
            BroadcastMessage("Fired", null, SendMessageOptions.DontRequireReceiver);
            fireAudio.Play();
            fireAudio.pitch = 1.3f - (powerRatio * powerRatio * 0.6f);
            fireAudio.volume = 0.1f + (powerRatio * powerRatio * powerRatio * 0.3f);
            chargingAudio.Stop();
            chargingAudio.pitch = 1f;
            isCharged = false;
            isCharging = false;
        }
    }

    void OnGUI()
    {
        drawPredictionLine();
        drawEnergyBar();
    }


    void drawEnergyBar()
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

    void drawPredictionLine()
    {
        if (GameManager.instance.playing == false)
        {
            return;
        }
        if (!Input.GetButton("Fire1"))
            return;

        Vector3 newPosition;
        Vector3 lastPosition = new Vector3(0f, 0f, 0.3f);
        Vector3 lastDirection = transform.up;

        for (int i = 0; i < 18 && Mathf.Abs(lastPosition.x) < outOfSightY; i++)
        {
            newPosition = lastPosition + lastDirection * (Mathf.Abs(boardWidth / (transform.up.x * 4)));
            if (i % 4 == 1)
                lastDirection.x = -lastDirection.x;
            if (i == 0)
                Drawing.DrawLine(
                    new Vector2(Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 1.5f)).x, Screen.height - Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 1.5f)).y),
                    new Vector2(Camera.main.WorldToScreenPoint(newPosition).x, Screen.height - Camera.main.WorldToScreenPoint(newPosition).y),
                    new Color(predictiveLineColor.r, predictiveLineColor.g, predictiveLineColor.b, predictiveLineColor.a * (1f - (i / 18f))) , 4f);
            else
                Drawing.DrawLine(
                    new Vector2(Camera.main.WorldToScreenPoint(lastPosition).x, Screen.height - Camera.main.WorldToScreenPoint(lastPosition).y),
                    new Vector2(Camera.main.WorldToScreenPoint(newPosition).x, Screen.height - Camera.main.WorldToScreenPoint(newPosition).y),
                    new Color(predictiveLineColor.r, predictiveLineColor.g, predictiveLineColor.b, predictiveLineColor.a * (1f - (i / 18f))), 4f);
            lastPosition = newPosition;
        }
    }
}
