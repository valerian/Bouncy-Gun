using UnityEngine;

public class BonusEnergyRegen : BonusRatio
{
    public BonusEnergyRegen()
    {
        verboseName = "Energy regeneration rate";
        step = 0.1f;
    }

    public override void Apply()
    {
        GameManager.instance.energyRegen *= GetMultiplicator();
    }
}
