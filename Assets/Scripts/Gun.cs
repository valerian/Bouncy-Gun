using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public float initialVelocity = 2000f;
    public GameObject bullet;
    public float fireRate = 2.0f;
    public float boardWidth = 12.5f;
    public float outOfSightY = 30f;
    public Color predictiveLineColor = Color.white;

    private float chargingStartTime;
    private bool isCharged = false;

    private float health;

    // Use this for initialization
    void Start () 
    {

    }
    
    // Update is called once per frame
    void Update () 
    {
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


        if (Input.GetButtonDown("Fire1"))
        {
            chargingStartTime = Time.time;
            BroadcastMessage("Charging", null, SendMessageOptions.DontRequireReceiver);
        }

        if (!isCharged && Input.GetButton("Fire1") && Time.time >= chargingStartTime + fireRate)
        {
            isCharged = true;
            BroadcastMessage("Charged", null, SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            float powerRatio = 1.0f;
            if (!isCharged)
            {
                powerRatio = (Time.time - chargingStartTime) / fireRate;
            }
            GameObject instance = (GameObject)Instantiate(bullet, transform.position + (transform.up * 1.5f) + new Vector3(0, 0, -0.2f), transform.rotation);
            instance.GetComponent<Bullet>().Launch(transform.up, initialVelocity * powerRatio);
            BroadcastMessage("Fired", null, SendMessageOptions.DontRequireReceiver);
            isCharged = false;
        }
    }

    void OnGUI()
    {
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
