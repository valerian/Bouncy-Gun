using UnityEngine;
using System.Collections;

public abstract class BonusRatio : Bonus
{
    public float step = 0.12f;
    public float scale
    {
        get { return value * step; }
    }

    // resets step to 0.01!!
    public void percentModeSet(float percent)
    {
        step = 0.01f;
        value = (int) (percent * 100f);
    }

    public override string GetDescriptionText()
    {
        return verboseName;
    }

    public override string GetValueText()
    {
        return GetChangePercentString() + " %";
    }

    public float GetIncreaseRatio()
    {
        return scale;
    }

    public float GetDecreaseRatio()
    {
        return 1f - (1f / (1f - scale));
    }

    public float GetChangeRatio()
    {
        return scale > 0 ? GetIncreaseRatio() : -GetDecreaseRatio();
    }

    public int GetChangePercent()
    {
        return Mathf.RoundToInt(100 * GetChangeRatio());
    }

    public string GetChangePercentString()
    {
        return (scale > 0 ? "+" : "") + GetChangePercent();
    }

    public float GetMultiplicator()
    {
        return 1f + GetChangeRatio();
    }
}
