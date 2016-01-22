using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public GameObject bullet;
    public float boardWidth = 13f;
    public Color predictiveLineColor = Color.white;
    public float ballisticPlaneZ = 0.35f;
    
    void FixedUpdate ()
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;
        }
    }

    void Update()
    {
        if (GameManager.instance.isPlaying == false)
        {
            transform.rotation = Quaternion.identity;
            return;
        }
        UpdateAimAndFire();
    }

    private void UpdateAimAndFire()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        float translationFactor = (ballisticPlaneZ - worldMousePosition.z) / Camera.main.transform.forward.z;
        worldMousePosition += translationFactor * Camera.main.transform.forward;
        var angle = Mathf.Atan2(transform.position.x - worldMousePosition.y, transform.position.y - worldMousePosition.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Fired(float velocity)
    {
        Vector3 bulletPosition = transform.position + (transform.up * (0.5f + (GameManager.instance.bulletSize / 2.8f)));
        bulletPosition.z = 0.35f;
        GameObject instance = SimplePool.Spawn(bullet,  bulletPosition, Quaternion.identity);
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
        if (GameManager.instance.isPlaying == false)
        {
            return;
        }
        if (!Input.GetButton("Fire1"))
            return;

        Vector3 newPosition;
        Vector3 lastPosition = new Vector3(0f, 0f, ballisticPlaneZ);
        Vector3 lastDirection = transform.up;

        int lineThickness = Mathf.RoundToInt(Screen.height / 150f);

        if (Vector3.Dot(lastDirection, new Vector3(0, 1, 0)) >= 0.999f)
            Drawing.DrawLine(
                new Vector2(Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 1.5f)).x, Screen.height - Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 1.5f)).y),
                new Vector2(Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 40f)).x, Screen.height - Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 40f)).y),
                predictiveLineColor, lineThickness);
        else
            for (int i = 0; i < 18; i++)
            {
                newPosition = lastPosition + lastDirection * (Mathf.Abs((boardWidth - (GameManager.instance.bulletSize / 2f)) / (transform.up.x * 4)));
                if (i % 4 == 1)
                    lastDirection.x = -lastDirection.x;
                if (i == 0)
                    Drawing.DrawLine(
                        new Vector2(Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 1.5f)).x, Screen.height - Camera.main.WorldToScreenPoint(lastPosition + (lastDirection * 1.5f)).y),
                        new Vector2(Camera.main.WorldToScreenPoint(newPosition).x, Screen.height - Camera.main.WorldToScreenPoint(newPosition).y),
                        new Color(predictiveLineColor.r, predictiveLineColor.g, predictiveLineColor.b, predictiveLineColor.a * (1f - (i / 18f))), lineThickness);
                else
                    Drawing.DrawLine(
                        new Vector2(Camera.main.WorldToScreenPoint(lastPosition).x, Screen.height - Camera.main.WorldToScreenPoint(lastPosition).y),
                        new Vector2(Camera.main.WorldToScreenPoint(newPosition).x, Screen.height - Camera.main.WorldToScreenPoint(newPosition).y),
                        new Color(predictiveLineColor.r, predictiveLineColor.g, predictiveLineColor.b, predictiveLineColor.a * (1f - (i / 18f))), lineThickness);
                lastPosition = newPosition;
            }
    }
}
