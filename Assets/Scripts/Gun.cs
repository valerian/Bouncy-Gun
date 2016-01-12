using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public static Gun instance;

    public GameObject bullet;
    public float boardWidth = 13f;
    public Color predictiveLineColor = Color.white;


    // Use this for initialization
    void Start () 
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    
    void FixedUpdate ()
    {
        if (GameManager.instance.playing == false)
        {
            return;
        }
    }

    void Update()
    {
        if (GameManager.instance.playing == false)
        {
            transform.rotation = Quaternion.identity;
            return;
        }
        UpdateAimAndFire();
    }

    private void UpdateAimAndFire()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(screenPosition.y - Input.mousePosition.y, screenPosition.x - Input.mousePosition.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Charging()
    {

    }

    void Charged()
    {

    }

    void Fired(float velocity)
    {
        GameObject instance = (GameObject)Instantiate(bullet, transform.position + (transform.up * (1f + (GameManager.instance.bulletSize / 2))) + new Vector3(0, 0, -0.2f), transform.rotation);
        instance.transform.localScale = instance.transform.localScale.normalized * GameManager.instance.bulletSize;
        instance.GetComponent<Rigidbody2D>().mass = GameManager.instance.bulletMass;
        instance.GetComponent<Bullet>().Launch(transform.up, velocity);
    }

    void OnGUI()
    {
        drawPredictionLine();
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

        for (int i = 0; i < 18; i++)
        {
            newPosition = lastPosition + lastDirection * (Mathf.Abs((boardWidth - GameManager.instance.bulletSize) / (transform.up.x * 4)));
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
