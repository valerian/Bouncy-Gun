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
