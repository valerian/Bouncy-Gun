using UnityEngine;

public class BonusMass : BonusRatio
{
    public BonusMass()
    {
        verboseName = "Projectile power";
        step = 0.12f;
    }

    public override void Apply()
    {
        GameManager.instance.bulletMass *= GetMultiplicator();
    }
}
