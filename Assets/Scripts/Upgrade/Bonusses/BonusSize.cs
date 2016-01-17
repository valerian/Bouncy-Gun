using UnityEngine;

public class BonusSize : BonusRatio
{
    public BonusSize()
    {
        verboseName = "Projectile size";
        step = 0.2f;
    }

    public override void Apply()
    {
        GameManager.instance.bulletSize *= GetMultiplicator();
    }
}
