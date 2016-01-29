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
    private float fps = 0;

    void Update()
    {
        if (firstRun)
        {
            fps = 1f / Time.deltaTime;
            firstRun = false;
            return;
        }

        if (warmingPasses > 0)
            warmingPasses--;

        fps = Mathf.Lerp(fps, 1f / Time.deltaTime, warmingPasses > 0 ? overrideRatioWarming : overrideRatio);
    }

    void OnGUI()
    {
        if (lastDisplayUpdate + displayUpdateInterval > Time.time)
            return;
        text.text = fps.ToString("F0");
        lastDisplayUpdate = Time.time;
    }
}
