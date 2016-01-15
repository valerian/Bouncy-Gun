using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusManager
{
    private List<Bonus> bonusCollection;

    public BonusManager()
    {
        bonusCollection = new List<Bonus>();
        bonusCollection.Add(new BonusBulletEnergy());
        bonusCollection.Add(new BonusChargeSpeed());
        bonusCollection.Add(new BonusDuration());
        bonusCollection.Add(new BonusEnergy());
        bonusCollection.Add(new BonusEnergyRegen());
        bonusCollection.Add(new BonusHealth());
        bonusCollection.Add(new BonusMass());
        bonusCollection.Add(new BonusSize());
        bonusCollection.Add(new BonusVelocity());
    }

    public Bonus[] GetRandomBonus(int nbBonus, int nbMalus, int bonusValue, int malusValue)
    {
        Bonus[] bonusses = new Bonus[(nbBonus + nbMalus)];
        ShuffleBonusCollection();
        for (int i = 0; i < nbBonus + nbMalus; i++)
        {
            bonusses[i] = bonusCollection[i].Clone();
            bonusses[i].value = (i < nbBonus) ? bonusValue : -malusValue;
        }
        return bonusses;
    }

    private void ShuffleBonusCollection()
    {
        int n = bonusCollection.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Bonus value = bonusCollection[k];
            bonusCollection[k] = bonusCollection[n];
            bonusCollection[n] = value;
        }  
    }
}




public abstract class Bonus
{
    public int value = 1;
    public string verboseName { get; protected set; }
    public bool isMalus { get { return value < 0; } }

    abstract public string GetDescriptionText();
    abstract public string GetValueText();
    abstract public void Apply();

    public Bonus Clone()
    {
        return (Bonus) this.MemberwiseClone();
    }
}




public abstract class BonusRatio : Bonus
{
    public float step = 0.12f;
    public float scale
    {
        get { return value * step; }
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

public class BonusHealth : BonusRatio
{
    public BonusHealth()
    {
        verboseName = "Max health";
        step = 0.15f;
    }

    public override void Apply()
    {
        GameManager.instance.maxHealth *= GetMultiplicator();
    }
}

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
