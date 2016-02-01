using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public float overrideRatio = 0.003f;
    public float overrideRatioWarming = 0.02f;
    public int warmingPasses = 100;
    public float displayUpdateInterval = 0.25f;
    public Text text;

    private bool firstRun = true;
    private float lastDisplayUpdate = 0;
    private float lastFPSUpdate;
    private float fps = 0;

    void Awake()
    {
        lastFPSUpdate = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (firstRun)
        {
            fps = 1f / (Time.realtimeSinceStartup - lastFPSUpdate);
            firstRun = false;
            lastFPSUpdate = Time.realtimeSinceStartup;
            return;
        }

        if (warmingPasses > 0)
            warmingPasses--;

        fps = Mathf.Lerp(fps, 1f / (Time.realtimeSinceStartup - lastFPSUpdate), warmingPasses > 0 ? overrideRatioWarming : overrideRatio);
        lastFPSUpdate = Time.realtimeSinceStartup;
    }

    void OnGUI()
    {
        if (lastDisplayUpdate + displayUpdateInterval > Time.realtimeSinceStartup)
            return;
        text.text = fps.ToString("F0");
        lastDisplayUpdate = Time.realtimeSinceStartup;
    }
}
