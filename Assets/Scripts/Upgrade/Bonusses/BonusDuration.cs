using UnityEngine;

public class BonusDuration : BonusRatio
{
    public BonusDuration()
    {
        verboseName = "Projectile duration";
        step = 0.3f;
    }

    public override void Apply()
    {
        GameManager.instance.bulletDuration *= GetMultiplicator();
    }
}
