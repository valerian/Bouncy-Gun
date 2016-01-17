using UnityEngine;

public class BonusEnergy : BonusRatio
{
    public BonusEnergy()
    {
        verboseName = "Max energy";
        step = 0.25f;
    }

    public override void Apply()
    {
        GameManager.instance.energyMax *= GetMultiplicator();
    }
}
