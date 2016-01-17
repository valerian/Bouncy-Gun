using UnityEngine;

public class BonusChargeSpeed : BonusRatio
{
    public BonusChargeSpeed()
    {
        verboseName = "Gun charging time";
        step = -0.15f;
    }

    public override void Apply()
    {
        GameManager.instance.fireRate *= GetMultiplicator();
    }
}
