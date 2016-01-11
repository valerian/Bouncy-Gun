using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public float initialVelocity = 2000f;
    public GameObject bullet;
    public float fireRate = 2.0f;

    private float chargingStartTime;
    private bool isCharged = false;

    // Use this for initialization
    void Start () 
    {

    }
    
    // Update is called once per frame
    void Update () 
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
            GameObject instance = (GameObject) Instantiate(bullet, transform.position + (transform.up * 1.5f) + new Vector3(0, 0, -0.2f), transform.rotation);
            instance.GetComponent<Bullet>().Launch(transform.up, initialVelocity * powerRatio);
            BroadcastMessage("Fired", null, SendMessageOptions.DontRequireReceiver);
            isCharged = false;
        }
    }
}
