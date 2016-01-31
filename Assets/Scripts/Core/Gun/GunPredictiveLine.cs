using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GunStatusControl))]
public class GunPredictiveLine : MonoBehaviour
{
    [SerializeField]
    private float ballisticPlaneZ = 0.35f;
    [SerializeField]
    private float boardWidth = 13f;
    [SerializeField]
    private Color predictiveLineColor = new Color(0f, 0f, 0f, 0.25f);

    GunStatusControl gunStatusControl;

    void Awake()
    {
        gunStatusControl = GetComponent<GunStatusControl>();
    }

    void OnGUI()
    {
        if (!(gunStatusControl.status == GunStatusControl.STATUS.charging || gunStatusControl.status == GunStatusControl.STATUS.charged))
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
                newPosition = lastPosition + lastDirection * (Mathf.Abs((boardWidth - (Game.GameData.bulletSize / 2f)) / (transform.up.x * 4)));
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
