using UnityEngine;

public class BonusVelocity : BonusRatio
{
    public BonusVelocity()
    {
        verboseName = "Projectile speed";
        step = 0.12f;
    }

    public override void Apply()
    {
        GameManager.instance.bulletVelocity *= GetMultiplicator();
    }
}
