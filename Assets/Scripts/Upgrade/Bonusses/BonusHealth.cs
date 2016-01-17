using UnityEngine;

public class BonusHealth : BonusRatio
{
    public BonusHealth()
    {
        verboseName = "Max health";
        step = 0.15f;
    }

    public override void Apply()
    {
        GameManager.instance.healthMax *= GetMultiplicator();
    }
}
