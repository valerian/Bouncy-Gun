using UnityEngine;

public class BonusBulletEnergy : BonusRatio
{
    public BonusBulletEnergy()
    {
        verboseName = "Projectile cost";
        step = -0.1f;
    }

    public override void Apply()
    {
        GameManager.instance.energyPerShot *= GetMultiplicator();
    }
}
